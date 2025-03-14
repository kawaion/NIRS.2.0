﻿using NIRS.Boundary_Interfaces;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Data_Transmitters;
using NIRS.Grid_Folder;
using MyDouble;
using NIRS.Parameter_Type;
using NIRS.Cannon_Folder.Barrel_Folder;
using NIRS.Cannon_Folder.Powder_Folder;
using NIRS.Functions_for_numerical_method;
using NIRS.Projectile_Folder;
using NIRS.Numerical_solution;
using NIRS.Interfaces;

namespace NIRS.Numerical_Method
{
    class SEL : INumericalMethod
    {       
        private readonly IMainData _mainData;
        private bool isBeltIntact = true;
        private readonly double FORCING_PRESSURE; 
        public SEL(IMainData mainData)
        {
            _mainData = mainData;
            FORCING_PRESSURE = mainData.ConstParameters.forcingPressure;
        }
        
        private readonly IOutputDataTransmitter outputDataTransmitter = new OutputDataTransmitter();


        public IGrid Calculate()
        {
            IGrid grid = new TimeSpaceGrid();

            var gridBorderFiller = GetGridBorderFiller();
            var gridWithFilledBorders = gridBorderFiller.FillAtZeroTime(grid);
            var numericalSolution = GetNumericalSolution(gridWithFilledBorders, gridBorderFiller);
            return outputDataTransmitter.GetOutputData(numericalSolution);
        }

        private IGridBorderFiller GetGridBorderFiller()
        {
            FunctionsBuilder functionsBuilder = new FunctionsBuilder(_mainData);
            var boundaryFunctions = functionsBuilder.BoundaryFunctionsBuild();
            return new GridBorderFiller(boundaryFunctions, _mainData);
        }
        private IGrid GetNumericalSolution(IGrid grid, IGridBorderFiller gridBorderFiller)
        {
            LimitedDouble n = new LimitedDouble(-0.5);

            while (!IsEndConditionNumericalSolution(grid,n))
            {
                n += 0.5;

                grid = gridBorderFiller.FillBarrelBorders(grid, n);
                grid = GetNumericalSolutionAtNodeN(grid, n);
                grid = GetNumericalSolutionInProjectile(grid, n);
                grid = GetInterpolateSolutionAtInaccessibleNodes(grid, n);
            }
            return grid;


            
        }
        bool IsEndConditionNumericalSolution(IGrid grid, LimitedDouble n)
        {
            var x = grid[n].sn.x;
            var lengthBarrel = _mainData.Barrel.Length;
            return x >= lengthBarrel;
        }
        private IGrid GetNumericalSolutionAtNodeN(IGrid grid, LimitedDouble n)
        {
            LimitedDouble k = new LimitedDouble(-0.5);

            bool isEnd = false;
            while (!isEnd)
            {
                k += 0.5;
                if(ParameterTypeGetter.IsDynamic(n, k) || ParameterTypeGetter.IsMixture(n, k))
                {
                    (grid,isEnd) = GetNumericalSolutionAtNodeNK(grid, n, k );
                }
            }
            return grid;


        }
        //bool IsEndConditionNumericalSolutionAtNodeN()
        //{

        //}

        private (IGrid grid,bool isEnd)  GetNumericalSolutionAtNodeNK(IGrid grid, LimitedDouble n, LimitedDouble k)
        {
            bool isEnd = false;
            FunctionsBuilder functionsBuilder = new FunctionsBuilder(_mainData);
            var functionsNewLayer = functionsBuilder.FunctionsParametersOfTheNextLayerBuild(grid);
            INumericalSolutionInNodes numericalSolutionInNodes = new NumericalSolutionInNodes(functionsNewLayer);
            
            try
            {
                grid = numericalSolutionInNodes.Get(grid, n, k);
            }
            catch
            {
                isEnd = true;
            }
            
            return (grid,isEnd);
        }
        private IGrid GetNumericalSolutionInProjectile(IGrid grid, LimitedDouble n)
        {
            FunctionsBuilder functionsBuilder = new FunctionsBuilder(_mainData);
            var projectileFunctions = functionsBuilder.ProjectileFunctionsBuild(grid);
            INumericalSolutionProjectile numericalSolutionProjectile = new NumericalSolutionProjectile(projectileFunctions);

            if(isBeltIntact == true)
                if (grid[n].sn.p > FORCING_PRESSURE)

            grid = numericalSolutionProjectile.Get(grid, n, isBeltIntact);

            return grid;
        }

        private IGrid GetInterpolateSolutionAtInaccessibleNodes(IGrid grid, LimitedDouble n)
        {
            FunctionsBuilder functionsBuilder = new FunctionsBuilder(_mainData);
            var parameterInterpolationFunctions = functionsBuilder.ParameterInterpolationFunctionsBuild(grid);
            INumericalSolutionInterpolation numericalSolutionInterpolation = new NumericalSolutionInterpolation(parameterInterpolationFunctions,_mainData);

            grid = numericalSolutionInterpolation.Get(grid, n);

            return grid;
        }
    }
}

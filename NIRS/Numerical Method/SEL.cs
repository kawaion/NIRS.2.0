using NIRS.Boundary_Interfaces;
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
using NIRS.Verifer;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;
using System.Net.Http.Headers;
using System.Diagnostics;
using NIRS.Parameter_names;
using NIRS.Helpers;
using System.Runtime.InteropServices.ComTypes;
using System;
using System.Collections.Generic;

namespace NIRS.Numerical_Method
{
    class SEL : INumericalMethod
    {       
        private readonly IMainData _mainData;
        private bool isBeltIntact = true;
        private readonly double FORCING_PRESSURE;
        private readonly double lengthBarrel;
        private IConstParameters constP;
        private int kChamber;

        private KGetter _k;
        public SEL(IMainData mainData)
        {
            _mainData = mainData;
            FORCING_PRESSURE = mainData.ConstParameters.forcingPressure;
            _k = new KGetter(mainData.ConstParameters);
            lengthBarrel = _mainData.Barrel.Length;
            constP = mainData.ConstParameters;
            kChamber = constP.countDivideChamber;
        }
        
        private readonly IOutputDataTransmitter outputDataTransmitter = new OutputDataTransmitter();
        FunctionsBuilder functionsBuilder;

        public IGrid Calculate()
        {
            IGrid grid = new TimeSpaceGridTest();

            functionsBuilder = new FunctionsBuilder(_mainData);
            functionsBuilder.Build(grid);
            CreateObjectsForNumericalSolution(grid, functionsBuilder);

            var xEndChamber = _mainData.Barrel.EndChamberPoint.X;
            var KChamber = constP.countDivideChamber;

            var gridBorderFiller = GetGridBorderFiller();
            var gridWithFilledBorders = gridBorderFiller.FillAtZeroTime(grid, KChamber);
            var gridWithNumericalSolution = GetNumericalSolution(gridWithFilledBorders, gridBorderFiller, KChamber);
            return outputDataTransmitter.GetOutputData(gridWithNumericalSolution);
        }

        private IGridBorderFiller GetGridBorderFiller()
        {
            FunctionsBuilder functionsBuilder = new FunctionsBuilder(_mainData);
            var boundaryFunctions = functionsBuilder.BoundaryFunctionsBuild();
            return new GridBorderFiller(boundaryFunctions, _mainData);
        }
        private IGrid GetNumericalSolution(IGrid grid, IGridBorderFiller gridBorderFiller, int KChamber)
        {
            double n = 0;

            while (isBeltIntact)
            {
                n += 0.5;

                grid = gridBorderFiller.FillBarrelBordersN(grid, n, KChamber);
                //grid = gridBorderFiller.FillBarrelBordersK(grid, n, KChamber);
                grid = GetNumericalSolutionAtNodesNIfBeltIntact(grid, n);
                //grid = gridBorderFiller.FillLastNodeOfMixture(grid, n);
                grid = GetInterpolateSolutionInKChamber(grid, n, gridBorderFiller);

                AttemptRipOffBelt(grid, n);
            }

            while (!IsEndConditionNumericalSolution(grid,n))// && n!=2363)
            {
                n += 0.5;

                grid = gridBorderFiller.FillBarrelBordersN(grid, n, KChamber);
                grid = GetNumericalSolutionAtNodesN(grid, n);
                
                grid = GetNumericalSolutionInProjectile(grid, n, gridBorderFiller);
                grid = GetInterpolateSolutionAtInaccessibleNodes(grid, n, isBeltIntact); 
                AttemptRipOffBelt(grid, n);         
            }
            return grid;
        }
        private double GetKDynamicLast(double Ksn)
        {
            var lastIntNode = (int)Math.Floor(Ksn);
            if (Ksn > lastIntNode + 0.5)
                return lastIntNode + 0.5;
            else return lastIntNode - 0.5;
        }
        bool IsEndConditionNumericalSolution(IGrid grid, double n)
        {
            var x = grid.GetSn(PN.x, n);
            return x >= lengthBarrel;
        }
        private double GetFirstK(double n)
        {
            if (n.IsInt())
                return -0.5;
            else
                return 0;
        }
        private IGrid GetNumericalSolutionAtNodesNIfBeltIntact(IGrid grid, double n)
        {
            double k = GetFirstK(n);
            bool isKLimit = CheckLimit(k);
            while (!isKLimit)
            {
                k += 1;
                grid = GetNumericalSolutionAtNodeNK(grid, n, k);
                isKLimit = CheckLimit(k);
            }
            return grid;
        }
        private IGrid GetInterpolateSolutionInKChamber(IGrid grid, double n, IGridBorderFiller gridBorderFiller)
        {
            return gridBorderFiller.FillLastNodeOfMixture(grid, n);
        }
        private bool CheckLimit(double k)
        {
            return (int)k >= kChamber;
        }
        private IGrid GetNumericalSolutionAtNodesN(IGrid grid, double n)
        {
            double k = GetFirstK(n);

            var snKPrevious = _k[grid.GetSn(PN.x, n - 1)];

            while(k + 2 <= snKPrevious)
            {
                k += 1;
                grid = GetNumericalSolutionAtNodeNK(grid, n, k );
            }
            
            return grid;
        }
        INumericalSolutionInNodes numericalSolutionInNodes;
        INumericalSolutionProjectile numericalSolutionProjectile;
        INumericalSolutionInterpolation numericalSolutionInterpolation;

        VerifierAbilityCalculateNode verifier;
        private IGrid GetNumericalSolutionAtNodeNK(IGrid grid, double n, double k)
        {
            //bool isPossible=verifier.Check(n, k);

            grid = numericalSolutionInNodes.Get(grid, n, k);

            return grid;
        }
        private IGrid GetNumericalSolutionInProjectile(IGrid grid, double n,IGridBorderFiller gridBorderFiller)
        {
            if (isBeltIntact == true)
            {
                grid = gridBorderFiller.FillProjectileAtFixedBorder(grid, n);
            }
            grid = numericalSolutionProjectile.Get(grid, n, isBeltIntact);

            return grid;
        }
        private void AttemptRipOffBelt(IGrid grid, double n)
        {
            if(isBeltIntact == true && n.IsInt())
            {
                var K = grid.LastIndexK(PN.p, n);
                if (grid[PN.p,n,K] > FORCING_PRESSURE)
                    isBeltIntact = false;
            }

        }

        private IGrid GetInterpolateSolutionAtInaccessibleNodes(IGrid grid, double n, bool isBeltIntact)
        {
            if(!isBeltIntact)
                grid = numericalSolutionInterpolation.Get(grid, n);

            return grid;
        }


        private void CreateObjectsForNumericalSolution(IGrid grid,FunctionsBuilder functionsBuilder)
        {
            var functionsNewLayer = functionsBuilder.FunctionsParametersOfTheNextLayerUpdate(grid);
            var projectileFunctions = functionsBuilder.ProjectileFunctionsUpdate(grid);
            var parameterInterpolationFunctions = functionsBuilder.ParameterInterpolationFunctionsUpdate(grid);

            numericalSolutionInNodes = new NumericalSolutionInNodes(functionsNewLayer);
            numericalSolutionProjectile = new NumericalSolutionProjectile(projectileFunctions);
            numericalSolutionInterpolation = new NumericalSolutionInterpolation(parameterInterpolationFunctions, _mainData);

            verifier = new VerifierAbilityCalculateNode(grid, _mainData);
        }
    }
}

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

namespace NIRS.Numerical_Method
{
    class SEL : INumericalMethod
    {       
        private readonly IMainData _mainData;
        private bool isBeltIntact = true;
        private readonly double FORCING_PRESSURE;

        private KGetter k;
        public SEL(IMainData mainData)
        {
            _mainData = mainData;
            FORCING_PRESSURE = mainData.ConstParameters.forcingPressure;
            k = new KGetter(mainData.ConstParameters);
        }
        
        private readonly IOutputDataTransmitter outputDataTransmitter = new OutputDataTransmitter();
        FunctionsBuilder functionsBuilder;

        public IGrid Calculate()
        {
            IGrid grid = new TimeSpaceGridTest();

            functionsBuilder = new FunctionsBuilder(_mainData);
            functionsBuilder.Build(grid);
            CreateNumericalSolutions(grid, functionsBuilder);

            var xEndChamber = _mainData.Barrel.EndChamberPoint.X;
            var KSn = k[xEndChamber];

            var gridBorderFiller = GetGridBorderFiller();
            var gridWithFilledBorders = gridBorderFiller.FillAtZeroTime(grid, KSn);
            var numericalSolution = GetNumericalSolution(gridWithFilledBorders, gridBorderFiller, KSn);
            return outputDataTransmitter.GetOutputData(numericalSolution);
        }

        private IGridBorderFiller GetGridBorderFiller()
        {
            FunctionsBuilder functionsBuilder = new FunctionsBuilder(_mainData);
            var boundaryFunctions = functionsBuilder.BoundaryFunctionsBuild();
            return new GridBorderFiller(boundaryFunctions, _mainData);
        }
        private IGrid GetNumericalSolution(IGrid grid, IGridBorderFiller gridBorderFiller, double KSn)
        {
            var KDynamicLast = GetKDynamicLast(KSn);
            double n = 0;
            while (!IsEndConditionNumericalSolution(grid,n))// && n!=2363)
            {
                n += 0.5;

                grid = gridBorderFiller.FillBarrelBorders(grid, n, isBeltIntact, KDynamicLast);
                grid = GetNumericalSolutionAtNodesN(grid, n);
                grid = gridBorderFiller.FillLastNodeOfMixture(grid, n, isBeltIntact);
                grid = GetNumericalSolutionInProjectile(grid, n, gridBorderFiller);
                grid = GetInterpolateSolutionAtInaccessibleNodes(grid, n); 
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
            var lengthBarrel = _mainData.Barrel.Length;
            return x >= lengthBarrel;
        }
        private IGrid GetNumericalSolutionAtNodesN(IGrid grid, double n)
        {
            double k;
            if (n.IsInt())
                k = -0.5;
            else
                k = 0;

                bool isEnd = false;
            while (!isEnd)
            {
                k += 1;
                (grid,isEnd) = GetNumericalSolutionAtNodeNK(grid, n, k );
            }
            return grid;
        }
        INumericalSolutionInNodes numericalSolutionInNodes;
        INumericalSolutionProjectile numericalSolutionProjectile;
        INumericalSolutionInterpolation numericalSolutionInterpolation;

        VerifierAbilityCalculateNode verifier;
        private (IGrid grid,bool isEnd)  GetNumericalSolutionAtNodeNK(IGrid grid, double n, double k)
        {
            bool isEnd = false;
            bool isPossible=verifier.Check(n, k);

            if(isPossible)
            {
                grid = numericalSolutionInNodes.Get(grid, n, k);
            }
            else
            {
                isEnd = true;
            }
            
            return (grid,isEnd);
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

        private IGrid GetInterpolateSolutionAtInaccessibleNodes(IGrid grid, double n)
        {
            grid = numericalSolutionInterpolation.Get(grid, n);

            return grid;
        }


        private void CreateNumericalSolutions(IGrid grid,FunctionsBuilder functionsBuilder)
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

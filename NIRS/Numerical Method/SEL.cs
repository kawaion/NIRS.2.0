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
using System.Threading.Tasks;
using NIRS.Visualization.Progress;

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

        Progresser _progresser = null;
        public void ProgressActivate(Progresser progresser)
        {
            _progresser = progresser;
        }

        public SEL(IMainData mainData)
        {
            //3.1718749999999996E-06
            //0.012687499999999999
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
            IGrid grid = new TimeSpaceGrid();

            functionsBuilder = new FunctionsBuilder(_mainData);
            functionsBuilder.Build(grid);
            CreateObjectsForNumericalSolution(grid, functionsBuilder);

            var xEndChamber = _mainData.Barrel.EndChamberPoint.X;
            var KChamber = new LimitedDouble(constP.countDivideChamber);

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
        private IGrid GetNumericalSolution(IGrid grid, IGridBorderFiller gridBorderFiller, LimitedDouble KChamber)
        {
            LimitedDouble n = new LimitedDouble(0);

            double lastpsi;
            double laste;
            double lasta;
            double lastp;
            double lastr;
            double lastrho;
            double lastz;

            double lastdynamic_m;
            double lastM;
            double lastv;
            double lastw;

            List<double> lastrS = new List<double>();

            while (isBeltIntact || n.IsHalfInt())
            {
                isBeltIntact = AttemptRipOffBelt(grid, n, isBeltIntact);
                n += 0.5;

                grid = gridBorderFiller.FillBarrelBordersN(grid, n);
                //grid = gridBorderFiller.FillBarrelBordersK(grid, n, KChamber);
                grid = GetNumericalSolutionAtNodesNIfBeltIntact(grid, n);
                //grid = gridBorderFiller.FillLastNodeOfMixture(grid, n);
                grid = GetInterpolateSolutionInKChamber(grid, n, gridBorderFiller);
                grid = GetProjectileParametersBeforeBeltIntact(grid, n);

                _progresser.Update(grid.LastIndexK(n).GetIndex(), n.GetDouble());

                //LimitedDouble lastK;

                //if (n.IsHalfInt())
                //{
                //    lastK = grid.LastIndexK(PN.dynamic_m, n);
                //    lastdynamic_m = grid[PN.dynamic_m, n, lastK];
                //    lastM = grid[PN.M, n, lastK];
                //    lastv = grid[PN.v, n, lastK];
                //    lastw = grid[PN.w, n, lastK];
                //}

                //if (n.IsInt())
                //{
                //    lastK = grid.LastIndexK(PN.p, n);
                //    lastpsi = grid[PN.psi, n, lastK];
                //    laste = grid[PN.e, n, lastK];
                //    lasta = grid[PN.a, n, lastK];
                //    lastp = grid[PN.p, n, lastK];
                //    lastr = grid[PN.r, n, lastK];
                //    lastrho = grid[PN.rho, n, lastK];
                //    lastz = grid[PN.z, n, lastK];

                //    lastrS.Add(lastp);
                //}
            }

            while (!IsEndConditionNumericalSolution(grid, n))// && n!=2363)
            {
                n += 0.5;

                grid = gridBorderFiller.FillBarrelBordersN(grid, n);
                grid = GetNumericalSolutionAtNodesN(grid, n);
                
                grid = GetNumericalSolutionInProjectile(grid, n, gridBorderFiller);
                grid = GetInterpolateSolutionAtInaccessibleNodes(grid, n);
                double x = grid.GetSn(PN.x, n);

                _progresser.Update(grid.LastIndexK(n).GetIndex(), n.GetDouble());

                //LimitedDouble lastK;

                //if (n.IsInt())
                //{

                //    lastK = grid.LastIndexK(PN.p, n);
                //    lastp = grid[PN.p, n, lastK];
                //    lastrS.Add(lastp);
                //}

            } 
            return grid;
        }

        private IGrid GetProjectileParametersBeforeBeltIntact(IGrid grid, LimitedDouble n)
        {
            grid = numericalSolutionProjectile.GetProjectileParametersBeforeBeltIntact(grid, n);

            return grid;
        }

        bool IsEndConditionNumericalSolution(IGrid grid, LimitedDouble n)
        {
            var x = grid.GetSn(PN.x, n);
            return x >= lengthBarrel;
        }
        private LimitedDouble GetFirstK(LimitedDouble n)
        {
            if (n.IsInt())
                return new LimitedDouble(0.5);
            else
                return new LimitedDouble(1);
        }
        private IGrid GetNumericalSolutionAtNodesNIfBeltIntact(IGrid grid, LimitedDouble n)
        {
            LimitedDouble k = GetFirstK(n);
            bool isKLimit = CheckLimit(k);
            List<double> listp = new List<double>(); 
            while (!isKLimit)
            {
                grid = GetNumericalSolutionAtNodeNK(grid, n, k);
                listp.Add(grid[PN.p, n, k]);
                k += 1;
                isKLimit = CheckLimit(k);
                
            }
            return grid;
        }
        private IGrid GetInterpolateSolutionInKChamber(IGrid grid, LimitedDouble n, IGridBorderFiller gridBorderFiller)
        {
            return gridBorderFiller.FillLastNodeOfMixture(grid, n);
        }
        private bool CheckLimit(LimitedDouble k)
        {
            return k > kChamber - 0.5;//1;
        }
        private IGrid GetNumericalSolutionAtNodesN(IGrid grid, LimitedDouble n)
        {
            LimitedDouble k = GetFirstK(n);

            var KsnPrevious = new LimitedDouble(_k[grid.GetSn(PN.x, n - 1)]);

            do
            {
                grid = GetNumericalSolutionAtNodeNK(grid, n, k);
                k += 1;
            } while (k <= KsnPrevious - 1);
            
            return grid;
        }
        INumericalSolutionInNodes numericalSolutionInNodes;
        INumericalSolutionProjectile numericalSolutionProjectile;
        INumericalSolutionInterpolation numericalSolutionInterpolation;

        VerifierAbilityCalculateNode verifier;
        private IGrid GetNumericalSolutionAtNodeNK(IGrid grid, LimitedDouble n, LimitedDouble k)
        {
            grid = numericalSolutionInNodes.Get(grid, n, k);

            return grid;
        }
        private IGrid GetNumericalSolutionInProjectile(IGrid grid, LimitedDouble n,IGridBorderFiller gridBorderFiller)
        {
            //grid = gridBorderFiller.FillProjectileAtFixedBorder(grid, n);

            grid = numericalSolutionProjectile.Get(grid, n);

            return grid;
        }
        private bool AttemptRipOffBelt(IGrid grid, LimitedDouble n, bool isBeltIntact)
        {
            if(isBeltIntact == true && n.IsInt())
            {
                var K = grid.LastIndexK(PN.p, n);
                if (grid[PN.p,n,K] > FORCING_PRESSURE)
                    return false;
            }
            return isBeltIntact;

        }

        private IGrid GetInterpolateSolutionAtInaccessibleNodes(IGrid grid, LimitedDouble n)
        {
            grid = numericalSolutionInterpolation.Get(grid, n);

            return grid;
        }


        private void CreateObjectsForNumericalSolution(IGrid grid,FunctionsBuilder functionsBuilder)
        {
            var functionsNewLayer = functionsBuilder.FunctionsParametersOfTheNextLayerBuild(grid);
            var projectileFunctions = functionsBuilder.ProjectileFunctionsBuild(grid);
            var parameterInterpolationFunctions = functionsBuilder.ParameterInterpolationFunctionsBuild(grid);

            numericalSolutionInNodes = new NumericalSolutionInNodes(functionsNewLayer);
            numericalSolutionProjectile = new NumericalSolutionProjectile(projectileFunctions, _mainData);
            numericalSolutionInterpolation = new NumericalSolutionInterpolation(parameterInterpolationFunctions, _mainData);

            verifier = new VerifierAbilityCalculateNode(grid, _mainData);
        }
    }
}

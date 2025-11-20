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
using NIRS.Numerical_Method.Method_SEL.SolutionCalculatorAfterTheBeltBreaks;
using NIRS.Numerical_Method.Method_SEL.Solution_Calculator_Before_The_Belt_Breaks;
using NIRS.Numerical_Method.Method_SEL.Solution.Helper;

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

        private KGetter kGetter;

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
            kGetter = new KGetter(mainData.ConstParameters);
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

            List<double> lastrS = new List<double>();

            Visualizator visualizator = new Visualizator(_progresser);

            SolutionCalculatorAfterTheBeltBreaks solutionCalculatorAfterTheBeltBreaks = new SolutionCalculatorAfterTheBeltBreaks(lengthBarrel, gridBorderFiller, numericalSolutionInNodes, numericalSolutionProjectile, numericalSolutionInterpolation, visualizator, kGetter);
            SolutionCalculatorBeforeTheBeltBreaks solutionCalculatorBeforeTheBeltBreaks = new SolutionCalculatorBeforeTheBeltBreaks(FORCING_PRESSURE, kChamber, gridBorderFiller, numericalSolutionInNodes, numericalSolutionProjectile, visualizator);

            grid = solutionCalculatorBeforeTheBeltBreaks.Calculate(grid, n);
            grid = solutionCalculatorAfterTheBeltBreaks.Calculate(grid, n);

            return grid;
        }

        private IGrid GetProjectileParametersBeforeBeltIntact(IGrid grid, LimitedDouble n)
        {
            grid = numericalSolutionProjectile.GetProjectileParametersBeforeBeltIntact(grid, n);

            return grid;
        }
        INumericalSolutionInNodes numericalSolutionInNodes;
        INumericalSolutionProjectile numericalSolutionProjectile;
        INumericalSolutionInterpolation numericalSolutionInterpolation;

        VerifierAbilityCalculateNode verifier;
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

using MyDouble;
using NIRS.Functions_for_numerical_method;
using NIRS.Grid_Folder;
using NIRS.Helpers;
using NIRS.Interfaces;
using NIRS.Numerical_Method.Method_SEL.Solution.Helper;
using NIRS.Numerical_Method.Method_SEL.Solution_Calculator_After_The_Belt_Breaks;
using NIRS.Numerical_Method.Method_SEL.Solution_Calculator_Before_The_Belt_Breaks;
using NIRS.Numerical_solution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace NIRS.Numerical_Method.Method_SEL.Solution
{
    internal class NumericalSolution
    {
        private readonly IMainData _mainData;
        private readonly IBoundaryFunctions _boundaryFunctions;
        private readonly IFunctionsParametersOfTheNextLayer _functionsNewLayer;
        private readonly IProjectileFunctions _projectileFunctions;
        private readonly IParameterInterpolationFunctions _parameterInterpolationFunctions;
        private readonly Visualizator _visualizator;

        private readonly LimitedDouble kSn;
        private readonly double forsingPressure;

        private readonly INumericalSolutionInNodes numericalSolutionInNodes;
        private readonly INumericalSolutionProjectile numericalSolutionProjectile;
        private readonly INumericalSolutionInterpolation numericalSolutionInterpolation;

        private readonly double lengthBarrel;
        private KGetter kGetter;

        public NumericalSolution(
            IMainData mainData, 
            IBoundaryFunctions boundaryFunctions, 
            IFunctionsParametersOfTheNextLayer functionsNewLayer, 
            IProjectileFunctions projectileFunctions, 
            IParameterInterpolationFunctions parameterInterpolationFunctions,
            Visualizator visualizator)
        {
            _mainData = mainData;
            _boundaryFunctions = boundaryFunctions;
            _functionsNewLayer = functionsNewLayer;
            _projectileFunctions = projectileFunctions;
            _parameterInterpolationFunctions = parameterInterpolationFunctions;
            _visualizator = visualizator;

            var constParam = _mainData.ConstParameters;
            kSn = new LimitedDouble(constParam.countDivideChamber);
            forsingPressure = constParam.forcingPressure;

            numericalSolutionInNodes = new NumericalSolutionInNodes(functionsNewLayer);
            numericalSolutionProjectile = new NumericalSolutionProjectile(projectileFunctions, mainData);
            numericalSolutionInterpolation = new NumericalSolutionInterpolation(parameterInterpolationFunctions, mainData);

            lengthBarrel = _mainData.Barrel.Length;
            kGetter = new KGetter(constParam);
        }
        public IGrid Calculate(IGrid grid)
        {
            IGridBorderFiller gridBorderFiller = new GridBorderFiller(_boundaryFunctions, _mainData);
            SolutionCalculatorBeforeTheBeltBreaks solutionCalculatorBeforeTheBeltBreaks = 
                new SolutionCalculatorBeforeTheBeltBreaks(forsingPressure, kSn, gridBorderFiller, numericalSolutionInNodes, numericalSolutionProjectile, _visualizator);
            SolutionCalculatorAfterTheBeltBreaks solutionCalculatorAfterTheBeltBreaks =
                new SolutionCalculatorAfterTheBeltBreaks(lengthBarrel, gridBorderFiller, numericalSolutionInNodes, numericalSolutionProjectile, numericalSolutionInterpolation, _visualizator, kGetter);
            grid = gridBorderFiller.FillAtZeroTime(grid, kSn);
            grid = solutionCalculatorBeforeTheBeltBreaks.Calculate(grid);
            grid = solutionCalculatorAfterTheBeltBreaks.Calculate(grid);

            return grid;
        }
    }
}

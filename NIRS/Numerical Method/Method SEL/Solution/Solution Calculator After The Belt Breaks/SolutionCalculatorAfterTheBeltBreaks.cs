using MyDouble;
using NIRS.Helpers;
using NIRS.Interfaces;
using NIRS.Numerical_Method.Method_SEL.Solution;
using NIRS.Numerical_Method.Method_SEL.Solution.Helper;
using NIRS.Parameter_names;
using NIRS.Visualization.Progress;

namespace NIRS.Numerical_Method.Method_SEL.Solution_Calculator_After_The_Belt_Breaks
{
    class SolutionCalculatorAfterTheBeltBreaks
    {
        private readonly double _lengthBarrel;
        private readonly IGridBorderFiller _gridBorderFiller;
        private readonly INumericalSolutionInNodes _numericalSolutionInNodes;
        private readonly INumericalSolutionProjectile _numericalSolutionProjectile;
        private readonly INumericalSolutionInterpolation _numericalSolutionInterpolation;
        private readonly Visualizator _visualizator;
        private readonly Progresser _progresser;
        private readonly KGetter _kGetter;

        public SolutionCalculatorAfterTheBeltBreaks(double lengthBarrel, 
                                                    IGridBorderFiller gridBorderFiller, 
                                                    INumericalSolutionInNodes numericalSolutionInNodes,
                                                    INumericalSolutionProjectile numericalSolutionProjectile,
                                                    INumericalSolutionInterpolation numericalSolutionInterpolation,
                                                    Visualizator visualizator,
                                                    KGetter kGetter)
        {
            _lengthBarrel = lengthBarrel;
            _gridBorderFiller = gridBorderFiller;
            _numericalSolutionInNodes = numericalSolutionInNodes;
            _numericalSolutionProjectile = numericalSolutionProjectile;
            _numericalSolutionInterpolation = numericalSolutionInterpolation;
            _visualizator = visualizator;
            _kGetter = kGetter;
        }
        public IGrid Calculate(IGrid grid)
        {
            LimitedDouble n = grid.LastIndexN();
            while (!IsEndConditionNumericalSolution(grid, n))// && n!=2363)
            {
                n += 0.5;
                if(n == 1215.5)
                {
                    int c = 0;
                }

                //
                //if (n.IsHalfInt())
                //{
                //    grid = _gridBorderFiller.FillBarrelBordersN(grid, n);
                //    grid = _gridBorderFiller.FillBarrelBordersN(grid, n + 0.5);

                //    grid = GetNumericalSolutionAtNodesN(grid, n);
                //    grid = GetNumericalSolutionAtNodesN(grid, n + 0.5);

                //    grid = _numericalSolutionProjectile.Get(grid, n);
                //    grid = _numericalSolutionInterpolation.GetAtUnavailableNodes(grid, n);

                //    grid = _numericalSolutionProjectile.Get(grid, n + 0.5);
                //    grid = _numericalSolutionInterpolation.GetAtUnavailableNodes(grid, n + 0.5);
                //}
                //
                grid = _gridBorderFiller.FillBarrelBordersN(grid, n);
                grid = GetNumericalSolutionAtNodesN(grid, n);
                grid = _numericalSolutionProjectile.Get(grid, n);
                grid = _numericalSolutionInterpolation.GetAtUnavailableNodes(grid, n);

                _visualizator.VisualizationProgress(grid, n);
            }
            return grid;
        }
        private bool IsEndConditionNumericalSolution(IGrid grid, LimitedDouble n)
        {
            var x = grid.GetSn(PN.x, n);
            return x >= _lengthBarrel;
        }
        private IGrid GetNumericalSolutionAtNodesN(IGrid grid, LimitedDouble n)
        {
            LimitedDouble k = FirstKGetter.Get05_or_1(n);

            var KsnPrevious = new LimitedDouble(_kGetter[grid.GetSn(PN.x, n - 1)]);

            do
            {
                grid = _numericalSolutionInNodes.GetNodeNK(grid, n, k);
                k += 1;
            } while (k <= KsnPrevious - 1);

            return grid;
        }
    }
}

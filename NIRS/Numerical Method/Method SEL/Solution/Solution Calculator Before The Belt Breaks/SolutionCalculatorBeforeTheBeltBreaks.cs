using MyDouble;
using NIRS.Grid_Folder;
using NIRS.Interfaces;
using NIRS.Numerical_Method.Method_SEL.Solution;
using NIRS.Numerical_Method.Method_SEL.Solution.Helper;
using NIRS.Numerical_solution;
using NIRS.Parameter_names;
using NIRS.Visualization.Progress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Numerical_Method.Method_SEL.Solution_Calculator_Before_The_Belt_Breaks
{
    class SolutionCalculatorBeforeTheBeltBreaks
    {
        private readonly double _forsingPressure;
        private readonly int _kChamber;
        private readonly IGridBorderFiller _gridBorderFiller;
        private readonly INumericalSolutionInNodes _numericalSolutionInNodes;
        private readonly INumericalSolutionProjectile _numericalSolutionProjectile;
        private readonly Visualizator _visualizator;
        private readonly Progresser _progresser;
        private bool isBeltIntact;
        public SolutionCalculatorBeforeTheBeltBreaks(double forsingPressure,
                                                     int kChamber,
                                                     IGridBorderFiller gridBorderFiller,
                                                     INumericalSolutionInNodes numericalSolutionInNodes,
                                                     INumericalSolutionProjectile numericalSolutionProjectile,
                                                     Visualizator visualizator)
        {
            isBeltIntact = true;

            _forsingPressure = forsingPressure;
            _kChamber = kChamber;
            _gridBorderFiller = gridBorderFiller;
            _numericalSolutionInNodes = numericalSolutionInNodes;
            _numericalSolutionProjectile = numericalSolutionProjectile;
            _visualizator = visualizator;
        }
        public IGrid Calculate(IGrid grid, LimitedDouble n)
        {
            while (isBeltIntact || n.IsHalfInt())
            {
                isBeltIntact = AttemptRipOffBelt(grid, n, isBeltIntact);
                n += 0.5;

                grid = _gridBorderFiller.FillBarrelBordersN(grid, n);
                //grid = gridBorderFiller.FillBarrelBordersK(grid, n, KChamber);
                grid = GetNumericalSolutionAtNodesNIfBeltIntact(grid, n);
                //grid = gridBorderFiller.FillLastNodeOfMixture(grid, n);
                grid = _gridBorderFiller.FillLastNodeOfMixture(grid, n);
                grid = _numericalSolutionProjectile.GetProjectileParametersBeforeBeltIntact(grid, n);

                _visualizator.VisualizationProgress(grid, n);
            }
            return grid;
        }
        private bool AttemptRipOffBelt(IGrid grid, LimitedDouble n, bool isBeltIntact)
        {
            if (isBeltIntact == true && n.IsInt())
            {
                var K = grid.LastIndexK(PN.p, n);
                if (grid[PN.p, n, K] > _forsingPressure)
                    return false;
            }
            return isBeltIntact;
        }
        private IGrid GetNumericalSolutionAtNodesNIfBeltIntact(IGrid grid, LimitedDouble n)
        {
            LimitedDouble k = FirstKGetter.Get05_or_1(n);
            bool isKLimit = CheckLimit(k);
            List<double> listp = new List<double>();
            while (!isKLimit)
            {
                grid = _numericalSolutionInNodes.GetNodeNK(grid, n, k);
                listp.Add(grid[PN.p, n, k]);
                k += 1;
                isKLimit = CheckLimit(k);

            }
            return grid;
        }
        private bool CheckLimit(LimitedDouble k)
        {
            return k > _kChamber - 0.5;//1;
        }
    }
}

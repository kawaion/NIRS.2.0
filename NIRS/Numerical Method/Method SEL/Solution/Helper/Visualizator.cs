using MyDouble;
using NIRS.Interfaces;
using NIRS.Visualization.Progress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Numerical_Method.Method_SEL.Solution.Helper
{
    class Visualizator
    {
        private readonly Progresser _progresser;

        public Visualizator(Progresser progresser)
        {
            _progresser = progresser;
        }
        public void VisualizationProgress(IGrid grid, LimitedDouble n)
        {
            if (_progresser != null)
                _progresser.Update(grid.LastIndexK(n).GetIndex(), n.GetDouble());
        }
    }
}

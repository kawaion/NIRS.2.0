using MyDouble;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Helpers
{
    class InterpolateStep
    {
        private IGrid g;

        public InterpolateStep(IGrid grid)
        {
            g = grid;
        }
        public int Get(double n, double k, PN pn)
        {
            var kLast = g.LastIndex(pn, n);

            var distance = k - kLast;
            validation(distance);
            return (int)(distance);
        }

        private void validation(double distance)
        {
            if (distance < 0 || distance.IsHalfInt()) throw new Exception();
        }
    }
}

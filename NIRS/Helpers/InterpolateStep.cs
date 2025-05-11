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
        public int Get(LimitedDouble n, LimitedDouble k, PN pn)
        {
            var kLast = g.LastIndex(pn, n);

            var distance = k - kLast;
            validation(distance);
            return (int)(distance).Value;
        }

        private void validation(LimitedDouble distance)
        {
            if (distance < 0 || distance.Type == DoubleType.HalfInt) throw new Exception();
        }
    }
}

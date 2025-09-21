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
            var kLast = g.LastIndexK(pn, n);

            var distance = k - kLast;
            validation(distance);
            return distance.GetInt();
        }

        private void validation(LimitedDouble distance)
        {
            if (distance.GetDouble() < 0 || distance.IsHalfInt()) throw new Exception();
        }
    }
}

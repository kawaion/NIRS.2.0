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
            var kLast = g[n].LastIndex(pn);

            var distanceToPointK = k - kLast;
            if (distanceToPointK < 0) throw new Exception();
            else return (int)(distanceToPointK).Value;
        }
    }
}

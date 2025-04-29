using MyDouble;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Verifer
{
    class VerifierAbilityCalculateNode
    {
        private IGrid g;
        public VerifierAbilityCalculateNode(IGrid grid)
        {
            g = grid;
        }
        public bool Check(LimitedDouble n, LimitedDouble k)
        {
            bool good = true;
            List<PN> pnd = new List<PN> { PN.dynamic_m, PN.M, PN.v, PN.w };
            List<PN> pnm = new List<PN> { PN.r, PN.e, PN.psi, PN.z, PN.a, PN.m, PN.p, PN.ro, };
            if (n.IsHalfInt() & k.IsInt())
            {
                foreach (var pn in pnd)
                {
                    if (g[n - 1][k + 1].isNull(pn))
                        good = false;
                }
                foreach (var pn in pnm)
                {
                    if (g[n - 0.5][k + 0.5].isNull(pn))
                        good = false;
                }
            }
            else if (n.IsInt() & k.IsHalfInt())
            {
                foreach (var pn in pnm)
                {
                    if (g[n - 1][k + 1].isNull(pn))
                        good = false;
                }
                foreach (var pn in pnd)
                {
                    if (g[n - 0.5][k + 0.5].isNull(pn))
                        good = false;
                }
            }
            else throw new Exception();
            return good;
        }
    }
}

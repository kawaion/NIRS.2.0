using MyDouble;
using NIRS.Helpers;
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
        private XGetter _x;
        public VerifierAbilityCalculateNode(IGrid grid, IMainData mainData)
        {
            g = grid;
            _x = new XGetter(mainData.ConstParameters);
        }
        public bool Check(double n, double k)
        {
            return _x[k+1] <= g.GetSn(PN.x, n - 1);
        }
        //public bool Check(LimitedDouble n, LimitedDouble k)
        //{
        //    bool good = true;
        //    //List<PN> pnd = new List<PN> { PN.dynamic_m, PN.M, PN.v, PN.w };
        //    //List<PN> pnm = new List<PN> { PN.r, PN.e, PN.psi, PN.z, PN.a, PN.m, PN.p, PN.ro, };

        //    List<PN> pnd = new List<PN> { PN.dynamic_m};
        //    List<PN> pnm = new List<PN> { PN.r};                

        //    var g_nM1_kP1 = g[n - 1][k + 1];
        //    var g_nM05_kP05 = g[n - 0.5][k + 0.5];

        //    if (n.IsHalfInt() & k.IsInt())
        //    {

        //        foreach (var pn in pnd)
        //        {
        //            if (g_nM1_kP1.isNull(pn))
        //                good = false;
        //        }
        //        foreach (var pn in pnm)
        //        {
        //            if (g_nM05_kP05.isNull(pn))
        //                good = false;
        //        }
        //    }
        //    else if (n.IsInt() & k.IsHalfInt())
        //    {
        //        foreach (var pn in pnm)
        //        {
        //            if (g_nM1_kP1.isNull(pn))
        //                good = false;
        //        }
        //        foreach (var pn in pnd)
        //        {
        //            if (g_nM05_kP05.isNull(pn))
        //                good = false;
        //        }
        //    }
        //    else throw new Exception();
        //    return good;
        //}
    }
}

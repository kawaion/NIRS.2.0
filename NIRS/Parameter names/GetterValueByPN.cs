using MyDouble;
using NIRS.Grid_Folder;
using NIRS.Grid_Folder.Mediator;
using NIRS.Interfaces;
using System;

namespace NIRS.Parameter_names
{
    class GetterValueByPN
    {
        private IGrid g;
        public GetterValueByPN(IGrid grid)
        {
            g = grid;        
        }
        public double GetParamCell(PN param, LimitedDouble n, LimitedDouble k)
        {
            switch (param)
            {
                case PN.dynamic_m: return g.dynamic_m(n, k);
                case PN.M: return g.M(n, k);
                case PN.v: return g.v(n, k);
                case PN.w: return g.w(n, k);
                case PN.r: return g.r(n, k);
                case PN.e: return g.e(n, k);
                case PN.eps: return g.eps(n, k);
                case PN.psi: return g.psi(n, k);
                case PN.z: return g.z(n, k);
                case PN.a: return g.a(n, k);
                case PN.m: return g.m(n, k);
                case PN.p: return g.p(n, k);
                case PN.ro: return g.ro(n, k);
                case PN.One_minus_m: return 1 - g.m(n, k);

                default: throw new Exception("нереализованный параметр");
            }
        }
        public double GetParamCellSn(PN param, LimitedDouble n)
        {
            switch (param)
            {
                case PN.dynamic_m: return g[n].sn.D.dynamic_m;
                case PN.M: return g[n].sn.D.M;
                case PN.v: return g[n].sn.D.v;
                case PN.w: return g[n].sn.D.w;
                case PN.r: return g[n].sn.M.r;
                case PN.e: return g[n].sn.M.e;
                case PN.eps: return g[n].sn.M.eps;
                case PN.psi: return g[n].sn.M.psi;
                case PN.z: return g[n].sn.M.z;
                case PN.a: return g[n].sn.M.a;
                case PN.m: return g[n].sn.M.m;
                case PN.p: return g[n].sn.M.p;
                case PN.ro: return g[n].sn.M.ro;
                case PN.One_minus_m: return 1 - g[n].sn.M.m;

                default: throw new Exception("нереализованный параметр");
            }
        }
    }
}

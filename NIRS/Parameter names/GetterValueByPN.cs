using MyDouble;
using NIRS.Grid_Folder;
using NIRS.Grid_Folder.Mediator;
using System;

namespace NIRS.Parameter_names
{
    class GetterValueByPN
    {
        private IGrid g;
        public GetterValueByPN(IGrid grid)
        {
            g=grid        
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
    }
}

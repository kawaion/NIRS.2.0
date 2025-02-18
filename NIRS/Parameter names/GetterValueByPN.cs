using MyDouble;
using NIRS.Grid_Folder;
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
                case PN.dynamic_m: return g[n][k].dynamic_m;
                case PN.M: return g[n][k].M;
                case PN.v: return g[n][k].v;
                case PN.w: return g[n][k].w;
                case PN.r: return g[n][k].r;
                case PN.e: return g[n][k].e;
                case PN.eps: return g[n][k].eps;
                case PN.psi: return g[n][k].psi;
                case PN.z: return g[n][k].z;
                case PN.a: return g[n][k].a;
                case PN.m: return g[n][k].m;
                case PN.p: return g[n][k].p;
                case PN.ro: return g[n][k].ro;
                case PN.One_minus_m: return 1 - g[n][k].m;

                default: throw new Exception("нереализованный параметр");
            }
        }
        public double GetParamCellSn(PN param, LimitedDouble n)
        {
            switch (param)
            {
                case PN.dynamic_m: return g[n].sn.dynamic_m;
                case PN.M: return g[n].sn.M;
                case PN.v: return g[n].sn.v;
                case PN.w: return g[n].sn.w;
                case PN.r: return g[n].sn.r;
                case PN.e: return g[n].sn.e;
                case PN.eps: return g[n].sn.eps;
                case PN.psi: return g[n].sn.psi;
                case PN.z: return g[n].sn.z;
                case PN.a: return g[n].sn.a;
                case PN.m: return g[n].sn.m;
                case PN.p: return g[n].sn.p;
                case PN.ro: return g[n].sn.ro;
                case PN.One_minus_m: return 1 - g[n].sn.m;

                default: throw new Exception("нереализованный параметр");
            }
        }
    }
}

using System;
using NIRS.Parameter_names;
using MyDouble;

namespace NIRS.Grid_Folder.Mediator
{
    static class GridMediator
    {
        public static double Get(this Grid grid, PN pn, LimitedDouble n, LimitedDouble k)
        {
            switch (pn)
            {
                case PN.dynamic_m: return grid[n][k].D.dynamic_m;
                case PN.M: return grid[n][k].D.M;
                case PN.v: return grid[n][k].D.v;
                case PN.w: return grid[n][k].D.w;

                case PN.a: return grid[n][k].M.a;
                case PN.e: return grid[n][k].M.e;
                case PN.eps: return grid[n][k].M.eps;
                case PN.m: return grid[n][k].M.m;
                case PN.p: return grid[n][k].M.p;
                case PN.psi: return grid[n][k].M.psi;
                case PN.r: return grid[n][k].M.r;
                case PN.ro: return grid[n][k].M.ro;
                case PN.z: return grid[n][k].M.z;

                default: throw new Exception();
            }
        }
        //сделать тесты
        public static double dynamic_m(this Grid grid, LimitedDouble n, LimitedDouble k)
        {
            return grid[n][k].D.dynamic_m;
        }
        public static double M(this Grid grid, LimitedDouble n, LimitedDouble k)
        {
            return grid[n][k].D.M;
        }
        public static double v(this Grid grid, LimitedDouble n, LimitedDouble k)
        {
            return grid[n][k].D.v;
        }
        public static double w(this Grid grid, LimitedDouble n, LimitedDouble k)
        {
            return grid[n][k].D.w;
        }



        public static double a(this Grid grid, LimitedDouble n, LimitedDouble k)
        {
            return grid[n][k].M.a;
        }
        public static double e(this Grid grid, LimitedDouble n, LimitedDouble k)
        {
            return grid[n][k].M.e;
        }
        public static double eps(this Grid grid, LimitedDouble n, LimitedDouble k)
        {
            return grid[n][k].M.eps;
        }
        public static double m(this Grid grid, LimitedDouble n, LimitedDouble k)
        {
            return grid[n][k].M.m;
        }
        public static double p(this Grid grid, LimitedDouble n, LimitedDouble k)
        {
            return grid[n][k].M.p;
        }
        public static double psi(this Grid grid, LimitedDouble n, LimitedDouble k)
        {
            return grid[n][k].M.psi;
        }
        public static double r(this Grid grid, LimitedDouble n, LimitedDouble k)
        {
            return grid[n][k].M.r;
        }
        public static double ro(this Grid grid, LimitedDouble n, LimitedDouble k)
        {
            return grid[n][k].M.p;
        }
        public static double z(this Grid grid, LimitedDouble n, LimitedDouble k)
        {
            return grid[n][k].M.z;
        }
    }
}

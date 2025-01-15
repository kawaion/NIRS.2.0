using System;
using NIRS.Parameter_names;
using MyDouble;

namespace NIRS.Grid_Folder.Mediator
{
    static class GridMediator
    {
        public static double Get(this Grid grid,PN pn, LimitedDouble n, LimitedDouble k)
        {
            switch (pn)
            {
                case PN.dynamic_m : return grid[n][k].D.dynamic_m;
                case PN.M : return grid[n][k].D.M;
                case PN.v: return grid[n][k].D.v;
                case PN.w: return grid[n][k].D.w;

                case PN.a : return grid[n][k].M.a;
                case PN.e : return grid[n][k].M.e;
                case PN.eps : return grid[n][k].M.eps;
                case PN.m : return grid[n][k].M.m;
                case PN.p : return grid[n][k].M.p;
                case PN.psi : return grid[n][k].M.psi;
                case PN.r : return grid[n][k].M.r;
                case PN.ro : return grid[n][k].M.ro;
                case PN.z : return grid[n][k].M.z;

                //default : throw new Exception(); создать отдельные методы для случаев
            }
        }
    }
}

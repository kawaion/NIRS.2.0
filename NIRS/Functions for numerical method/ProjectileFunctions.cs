using MyDouble;
using NIRS.Cannon_Folder.Barrel_Folder;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Grid_Folder;
using NIRS.Helpers;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using NIRS.Projectile_Folder;
using System;

namespace NIRS.Functions_for_numerical_method
{
    public class ProjectileFunctions : IProjectileFunctions
    {
        private readonly IGrid g;
        private readonly IProjectile sn;
        private readonly IConstParameters constP;
        private readonly IBarrelSize bs;

        private readonly XGetter x;

        public ProjectileFunctions(IGrid grid, IMainData mainData)
        {
            g = grid;
            sn = mainData.Projectile;
            constP = mainData.ConstParameters;
            bs = mainData.BarrelSize; 

            XGetter x = new XGetter(mainData.ConstParameters);
        }
        public double Get(PN pn, LimitedDouble n, Pos pos)
        {
        }

        public double Get(PN pn, LimitedDouble n)
        {
            throw new NotImplementedException();
        }

        public double Get_a(LimitedDouble n)
        {
            var x_n = g[n].sn.x;
            var x_nPlus1 = g[n].sn.x;

            return g[n].sn.a * (bs.S(x_n) / bs.S(x_nPlus1)) *
                (
                    1 - constP.tau *
                    (
                        dvdx(n + 0.5)
                    )
                );
        }
        public double Get_e(LimitedDouble n)
        {
            return g[n].sn.e - constP.tau *
                (
                    g[n].sn.e * dvdx(n + 0.5)
                    + g[n].sn.p * 
                );
                   
        }
        public double Get_m(LimitedDouble n)
        {
            throw new NotImplementedException();
        }

        public double Get_p(LimitedDouble n)
        {
            throw new NotImplementedException();
        }

        public double Get_psi(LimitedDouble n)
        {
            throw new NotImplementedException();
        }

        public double Get_r(LimitedDouble n)
        {
            throw new NotImplementedException();
        }

        public double Get_ro(LimitedDouble n)
        {
            throw new NotImplementedException();
        }

        public double Get_v(LimitedDouble n)
        {
            throw new NotImplementedException();
        }

        public double Get_x(LimitedDouble n)
        {
            throw new NotImplementedException();
        }

        public double Get_z(LimitedDouble n)
        {
            throw new NotImplementedException();
        }

        private double dvdx(LimitedDouble n)
        {
            return (g[n].sn.v_sn - g[n].Last().v) /
                   (g[n].sn.x - x[g[n].LastIndex()]);
        }
    }
}

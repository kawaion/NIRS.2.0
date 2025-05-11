using MyDouble;
using NIRS.Cannon_Folder.Barrel_Folder;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Grid_Folder;
using NIRS.Helpers;
using NIRS.Interfaces;
using NIRS.Nabla_Functions.Projectile;
using NIRS.Parameter_names;
using NIRS.Projectile_Folder;
using System;

namespace NIRS.Functions_for_numerical_method
{
    public class ProjectileFunctions : IProjectileFunctions
    {
        private IGrid g;
        private readonly IConstParameters constP;
        private readonly IBarrelSize bs;
        private readonly IPowder powder;
        private readonly IProjectile projectile;

        private readonly IWaypointCalculator wc;
        private readonly IHFunctions hf;

        private readonly Differencial d;

        public ProjectileFunctions(IGrid grid, 
                                   IWaypointCalculator waypointCalculator,           
                                   IHFunctions hFunctions, 
                                   IMainData mainData)
        {
            g = grid;
            constP = mainData.ConstParameters;
            bs = mainData.Barrel.BarrelSize;
            powder = mainData.Powder;
            projectile = mainData.Projectile;

            wc = waypointCalculator;
            hf = hFunctions;

            d = new Differencial(grid, mainData.ConstParameters);
        }

        public double Get(PN pn, LimitedDouble n)
        {
            switch(pn)
            {
                case PN.a: return Get_a(n);
                case PN.e: return Get_e(n);
                case PN.m: return Get_m(n);
                case PN.p: return Get_p(n);
                case PN.psi: return Get_psi(n);
                case PN.r: return Get_r(n);
                case PN.ro: return Get_ro(n);
                case PN.v: return Get_vSn(n);
                case PN.x: return Get_x(n);
                case PN.z: return Get_z(n);
            }

            throw new Exception();
        }

        public double Get_a(LimitedDouble N)
        {
            var n = OffseterN.AppointAndOffset(N, + 1);

            var x_n = g.GetSn(PN.x, n);
            var x_nPlus1 = g.GetSn(PN.x, n + 1);

            return g.GetSn(PN.a, n) * (bs.S(x_n) / bs.S(x_nPlus1)) *
                (
                    1 - constP.tau * d.dvdx(n + 0.5)
                );
        }
        public double Get_e(LimitedDouble N)
        {
            var n = OffseterN.AppointAndOffset(N, + 1);

            return g.GetSn(PN.e, n) - constP.tau *
                (
                    g.GetSn(PN.e, n) * d.dvdx(n + 0.5)
                    + g.GetSn(PN.p, n) * 
                    (wc.sn.Nabla(PN.m,PN.S,PN.v).Cell(n + 0.5) + wc.sn.Nabla(PN.One_minus_m, PN.S, PN.w).Cell(n + 0.5))
                    - hf.sn.H4(n + 0.5)
                );
                   
        }
        public double Get_m(LimitedDouble N)
        {
            var n = OffseterN.AppointAndOffset(N, + 1);

            return 1 - g.GetSn(PN.a, n + 1) * powder.LAMDA0 * (1 - g.GetSn(PN.psi, n + 1));
        }

        public double Get_p(LimitedDouble N)
        {
            var n = OffseterN.AppointAndOffset(N, + 1);

            var x_nPlus1 = g.GetSn(PN.x, n + 1);

            var res = (constP.teta * g.GetSn(PN.e, n + 1)) /
                (
                    g.GetSn(PN.m, n + 1) * bs.S(x_nPlus1)
                    - constP.alpha * g.GetSn(PN.r, n + 1)
                );

            return res;
        }

        public double Get_psi(LimitedDouble N)
        {
            var n = OffseterN.AppointAndOffset(N, + 1);

            return g.GetSn(PN.psi, n)
                   + constP.tau * hf.sn.HPsi(n + 0.5);
        }

        public double Get_r(LimitedDouble N)
        {
            var n = OffseterN.AppointAndOffset(N, + 1);

            return g.GetSn(PN.r, n) - constP.tau *
                (
                   g.GetSn(PN.r, n) * d.dvdx(n + 0.5) - hf.sn.H3(n + 0.5)
                );
        }

        public double Get_ro(LimitedDouble N)
        {
            var n = OffseterN.AppointAndOffset(N, + 1);

            var x_nPlus1 = g.GetSn(PN.x, n + 1);

            return g.GetSn(PN.r, n + 1) /
                  (g.GetSn(PN.m, n + 1) * bs.S(x_nPlus1));
        }

        public double Get_vSn(LimitedDouble N)
        {
            var n = OffseterN.AppointAndOffset(N, + 0.5);

            var x_n = g.GetSn(PN.x, n);

            return g.GetSn(PN.vSn, n - 0.5) + (constP.tau / projectile.q)
                                    * (g.GetSn(PN.p, n) * bs.S(x_n));
                  
        }

        public double Get_x(LimitedDouble N)
        {
            if (N.IsHalfInt())
            {
                var n = OffseterN.AppointAndOffset(N, + 0.5);

                return g.GetSn(PN.x, n - 0.5) + constP.tau * (g.GetSn(PN.vSn, n - 0.5) + g.GetSn(PN.vSn, n + 0.5)) / 2;
            }
            else
            {
                var n = OffseterN.AppointAndOffset(N, + 1);

                return g.GetSn(PN.x, n) + constP.tau * g.GetSn(PN.vSn, n + 0.5);
            }
        }

        public double Get_z(LimitedDouble N)
        {
            var n = OffseterN.AppointAndOffset(N, + 1);

            return g.GetSn(PN.psi, n)
                   + constP.tau * hf.sn.H5(n + 0.5);
        }

        public void Update(IGrid grid)
        {
            g = grid;
            wc.Update(grid);
            hf.Update(grid);
        }
    }
}

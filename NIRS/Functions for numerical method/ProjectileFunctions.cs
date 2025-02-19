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
        private readonly IGrid g;
        private readonly IConstParameters constP;
        private readonly IBarrelSize bs;
        private readonly IPowder powder;
        private readonly IProjectile projectile;

        private readonly IWaypointCalculator wc;
        private readonly IHFunctions hf;

        private readonly XGetter x;

        public ProjectileFunctions(IGrid grid, 
                                   IWaypointCalculator waypointCalculator,           
                                   IHFunctions hFunctions, 
                                   IMainData mainData)
        {
            g = grid;
            constP = mainData.ConstParameters;
            bs = mainData.BarrelSize;
            powder = mainData.Powder;
            projectile = mainData.Projectile;

            wc = waypointCalculator;
            hf = hFunctions;

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
            n = OffseterN.Appoint(n).Offset(n + 1);

            var x_n = g[n].sn.x;
            var x_nPlus1 = g[n+1].sn.x;

            return g[n].sn.a * (bs.S(x_n) / bs.S(x_nPlus1)) *
                (
                    1 - constP.tau * dvdx(n + 0.5)
                );
        }
        public double Get_e(LimitedDouble n)
        {
            n = OffseterN.Appoint(n).Offset(n + 1);

            return g[n].sn.e - constP.tau *
                (
                    g[n].sn.e * dvdx(n + 0.5)
                    + g[n].sn.p * 
                    (wc.sn.Nabla(PN.m,PN.S,PN.v).Cell(n + 0.5) + wc.sn.Nabla(PN.One_minus_m, PN.S, PN.w).Cell(n + 0.5))
                    - hf.sn.H4(n + 0.5)
                );
                   
        }
        public double Get_m(LimitedDouble n)
        {
            n = OffseterN.Appoint(n).Offset(n + 1);

            return 1 - g[n + 1].sn.a * powder.LAMDA0 * (1 - g[n + 1].sn.psi);
        }

        public double Get_p(LimitedDouble n)
        {
            n = OffseterN.Appoint(n).Offset(n + 1);

            var x_nPlus1 = g[n+1].sn.x;

            return (constP.teta * g[n + 1].sn.e) /
                (
                    g[n + 1].sn.m * bs.S(x_nPlus1)
                    - constP.alpha * g[n + 1].sn.r
                );
        }

        public double Get_psi(LimitedDouble n)
        {
            n = OffseterN.Appoint(n).Offset(n + 1);

            return g[n].sn.psi
                   + constP.tau * hf.sn.HPsi(n + 0.5);
        }

        public double Get_r(LimitedDouble n)
        {
            n = OffseterN.Appoint(n).Offset(n + 1);

            return g[n].sn.r - constP.tau *
                (
                   g[n].sn.r * dvdx(n + 0.5) - hf.sn.H3(n + 0.5)
                );
        }

        public double Get_ro(LimitedDouble n)
        {
            n = OffseterN.Appoint(n).Offset(n + 1);

            var x_nPlus1 = g[n+1].sn.x;

            return g[n + 1].sn.r /
                  (g[n + 1].sn.m * bs.S(x_nPlus1));
        }

        public double Get_vSn(LimitedDouble n)
        {
            n = OffseterN.Appoint(n).Offset(n + 0.5);

            var x_nPlus1 = g[n + 1].sn.x;

            return g[n - 0.5].sn.vSn + (constP.tau / projectile.q)
                                    * (g[n].sn.p * bs.S(x_nPlus1));
                  
        }

        public double Get_x(LimitedDouble n)
        {
            if (n.IsHalfInt())
            {
                n = OffseterN.Appoint(n).Offset(n + 0.5);

                return g[n - 0.5].sn.x + constP.tau * (g[n - 0.5].sn.vSn * g[n + 0.5].sn.vSn) / 2;
            }
            else
            {
                n = OffseterN.Appoint(n).Offset(n + 1);

                return g[n].sn.x + constP.tau * g[n + 0.5].sn.vSn;
            }
        }

        public double Get_z(LimitedDouble n)
        {
            n = OffseterN.Appoint(n).Offset(n + 1);

            return g[n].sn.psi
                   + constP.tau * hf.sn.H5(n + 0.5);
        }

        private double dvdx(LimitedDouble n)
        {
            return (g[n].sn.vSn - g[n].Last().v) /
                   (g[n].sn.x - x[g[n].LastIndex()]);
        }
    }
}

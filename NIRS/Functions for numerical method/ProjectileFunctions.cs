﻿using MyDouble;
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

        public double Get(PN pn, double n)
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

        public double Get_a(double N)
        {
            var n = OffseterN.AppointAndOffset(N, + 1);

            var x_n = g.GetSn(PN.x, n);
            var x_nPlus1 = g.GetSn(PN.x, n + 1);

            return g.GetSn(PN.a, n) * (bs.S(x_n) / bs.S(x_nPlus1)) *
                (
                    1 - constP.tau * d.dvdx(n + 0.5)
                );
        }
        public double Get_e(double N)
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
        public double Get_m(double N)
        {
            var n = OffseterN.AppointAndOffset(N, + 1);

            return 1 - g.GetSn(PN.a, n + 1) * powder.LAMDA0 * (1 - g.GetSn(PN.psi, n + 1));
        }

        public double Get_p(double N)
        {
            var n = OffseterN.AppointAndOffset(N, + 1);

            var x_nPlus1 = g.GetSn(PN.x, n + 1);

            var res = (constP.teta * g.GetSn(PN.e, n + 1)) /
                (
                    g.GetSn(PN.m, n + 1) * bs.S(x_nPlus1)
                    - constP.alpha * g.GetSn(PN.r, n + 1)
                );
            if (double.IsNaN(res))
            {
                var tmp1 = bs.S(x_nPlus1);
                var tmp2 = g.GetSn(PN.r, n + 1);
                var tmp3 = g.GetSn(PN.e, n + 1);
            }

                return res;
        }

        public double Get_psi(double N)
        {
            var n = OffseterN.AppointAndOffset(N, + 1);

            double psi;

            var z = g.GetSn(PN.z, n + 1);

            if(z>=1)
                psi = g.GetSn(PN.psi, n)
                   + constP.tau * hf.sn.HPsi(n + 0.5);
            else
            {
                psi = powder.BurningPowdersSize.Psi(z);
            }
            if (double.IsNaN(psi))
            {
                int c = 0;
                var tmpn = n;
                var tmp1 = hf.sn.HPsi(n + 0.5);
                var tmp2 = g.GetSn(PN.psi, n);
            }

            psi = PowderValidation(psi);
            return psi;
        }
        private static double PowderValidation(double value) // метод скопирован
        {
            if (value > 1)
                value = 1;
            return value;
        }
        public double Get_r(double N)
        {
            var n = OffseterN.AppointAndOffset(N, + 1);

            var res = g.GetSn(PN.r, n) - constP.tau *
                (
                   g.GetSn(PN.r, n) * d.dvdx(n + 0.5) - hf.sn.H3(n + 0.5)
                );

            if (double.IsNaN(res))
            {
                var tmp1 = g.GetSn(PN.r, n);
                var tmp2 = d.dvdx(n + 0.5);
                var tmp3 = hf.sn.H3(n + 0.5);
            }
            return res;
        }

        public double Get_ro(double N)
        {
            var n = OffseterN.AppointAndOffset(N, + 1);

            var x_nPlus1 = g.GetSn(PN.x, n + 1);

            return g.GetSn(PN.r, n + 1) /
                  (g.GetSn(PN.m, n + 1) * bs.S(x_nPlus1));
        }

        public double Get_vSn(double N)
        {
            var n = OffseterN.AppointAndOffset(N, + 0.5);

            var x_n = g.GetSn(PN.x, n);
            var tmp = bs.S(x_n);

            var tmp2 = g.GetSn(PN.vSn, n - 0.5) + (constP.tau / projectile.q)
                                    * (g.GetSn(PN.p, n) * bs.S(x_n));

            return g.GetSn(PN.vSn, n - 0.5) + (constP.tau / projectile.q)
                                    * (g.GetSn(PN.p, n) * bs.S(x_n));
                  
        }

        public double Get_x(double N)
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

        public double Get_z(double N)
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

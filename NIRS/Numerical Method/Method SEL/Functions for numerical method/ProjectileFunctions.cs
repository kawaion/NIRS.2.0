using MyDouble;
using NIRS.BPMN_folder;
using NIRS.Cannon_Folder.Barrel_Folder;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Grid_Folder;
using NIRS.Helpers;
using NIRS.Interfaces;
using NIRS.Nabla_Functions.Projectile;
using NIRS.Parameter_names;
using NIRS.Projectile_Folder;
using System;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
        private readonly double tau;
        XGetter xGetter;

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
            tau = constP.tau;
            xGetter = new XGetter(constP);
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
                case PN.rho: return Get_ro(n);
                case PN.v: return Get_vSn(n);
                case PN.x: return Get_x(n);
                case PN.z: return Get_z(n);
            }

            throw new Exception();
        }

        public double Get_a(LimitedDouble N)
        {
            var n = OffseterN.AppointAndOffset(N, +1);

            double a_n = g.GetSn(PN.a, n);
            double dwdx_nP05 = d.dwdx(n + 0.5);
            double tau = this.tau;

            var res = MainFunctions.a_Sn_nP1(a_n, dwdx_nP05, tau);

            return res;
        }
        //
        public double Get_e(LimitedDouble N)
        {
            var n = OffseterN.AppointAndOffset(N, +1);
            var k = g.LastIndexK(PN.v, n);

            double e_n = g.GetSn(PN.e, n);
            double dvdx_nP05 = d.dvdx(n + 0.5);
            double p_n = g.GetSn(PN.p, n);
            double nabla_mSv_nP05 = 0;

            //var tmp1 = g[PN.m, n, k - 0.5];
            //var tmp2 = g[PN.m, n, k - 1.5];
            //var tmp4 = bs.S(xGetter[k-0.5]);
            //var tmp5 = bs.S(xGetter[k - 1.5]);
            var tmp6 = g[PN.v, n + 0.5, k-1];
            //var tmp7 = xGetter[k - 1];
            //var tmp8 = g.GetSn(PN.x, n + 0.5);

            double nabla_OneMinusmSw_nP05 = (bs.Skm * g.GetSn(PN.vSn, n + 0.5) -
                (g[PN.m, n, k - 0.5] * bs.S(xGetter[k - 0.5])+ g[PN.m, n, k - 1.5] * bs.S(xGetter[k - 1.5])) * g[PN.v, n + 0.5, k - 1] / 2 -
                ((1-g[PN.m, n, k - 0.5]) * bs.S(xGetter[k - 0.5]) + (1-g[PN.m, n, k - 1.5]) * bs.S(xGetter[k - 1.5])) *g[PN.w, n + 0.5, k - 1] / 2)
                /(g.GetSn(PN.x, n + 0.5)- xGetter[k - 1]);
            double H4_nP05 = hf.sn.H4(n + 0.5);
            double tau = this.tau;

            var res = MainFunctions.e_Sn_nP1(e_n, dvdx_nP05, p_n, nabla_mSv_nP05, nabla_OneMinusmSw_nP05, H4_nP05, tau);
            return res;
        }
        //

        //public double Get_e(LimitedDouble N)
        //{
        //    var n = OffseterN.AppointAndOffset(N, + 1);

        //    double e_n = g.GetSn(PN.e, n);
        //    double dvdx_nP05 = d.dvdx(n + 0.5);
        //    double p_n = g.GetSn(PN.p, n);
        //    double nabla_mSv_nP05 = wc.sn.Nabla(PN.m, PN.S, PN.v, n + 0.5);
        //    double nabla_OneMinusmSw_nP05 = wc.sn.Nabla(PN.One_minus_m, PN.S, PN.w, n + 0.5);
        //    double H4_nP05 = hf.sn.H4(n + 0.5);
        //    double tau = this.tau;

        //    var res = MainFunctions.e_Sn_nP1(e_n, dvdx_nP05, p_n, nabla_mSv_nP05, nabla_OneMinusmSw_nP05, H4_nP05, tau);
        //    return res;
        //}
        public double Get_m(LimitedDouble N)
        {
            var n = OffseterN.AppointAndOffset(N, + 1);

            double a_nP1 = g.GetSn(PN.a, n + 1);
            double psi_nP1 = g.GetSn(PN.psi, n + 1);
            double LAMBDA0 = powder.LAMBDA0;

            var res = MainFunctions.m_Sn_nP1(a_nP1, psi_nP1, LAMBDA0);
            return res;
        }

        public double Get_p(LimitedDouble N)
        {
            var n = OffseterN.AppointAndOffset(N, + 1);

            double x_nP1 = g.GetSn(PN.x, n + 1);

            double e_nP1 = g.GetSn(PN.e, n + 1);
            double m_nP1 = g.GetSn(PN.m, n + 1);
            double r_nP1 = g.GetSn(PN.r, n + 1);
            double S_nP1 = bs.S(x_nP1);
            double teta = constP.teta;
            double alpha = constP.alpha;

            var res = MainFunctions.p_Sn_nP1(e_nP1, m_nP1, r_nP1, S_nP1, teta, alpha);
            return res;
        }

        public double Get_psi(LimitedDouble N)
        {
            var n = OffseterN.AppointAndOffset(N, + 1);

            double psi;

            var z = g.GetSn(PN.z, n + 1);

            if (z >= 1)
            {
                double psi_n = g.GetSn(PN.psi, n);
                double HPsi_nP05 = hf.sn.HPsi(n + 0.5);
                double tau = this.tau;

                psi = MainFunctions.psi_Sn_nP1(psi_n, HPsi_nP05, tau);
            }
            else
            {
                psi = powder.BurningPowdersSize.Psi(z);
            }

            psi = Validation.Validation01(psi);
            return psi;
        }
        public double Get_r(LimitedDouble N)
        {
            var n = OffseterN.AppointAndOffset(N, + 1);

            double r_n = g.GetSn(PN.r, n);
            double dvdx_nP05 = d.dvdx(n + 0.5);
            double H3_nP05 = hf.sn.H3(n + 0.5);
            double tau = this.tau;

            var res = MainFunctions.r_Sn_nP1(r_n, dvdx_nP05, H3_nP05, tau);
            return res;
        }

        public double Get_ro(LimitedDouble N)
        {
            var n = OffseterN.AppointAndOffset(N, + 1);

            var x_nP1 = g.GetSn(PN.x, n + 1);

            double r_nP1 = g.GetSn(PN.r, n + 1);
            double m_nP1 = g.GetSn(PN.m, n + 1);
            double S_nP1 = bs.S(x_nP1);

            var res = MainFunctions.ro_Sn_nP1(r_nP1, m_nP1, S_nP1);
            return res;
        }

        public double Get_vSn(LimitedDouble N)
        {
            var n = OffseterN.AppointAndOffset(N, + 0.5);

            var x_n = g.GetSn(PN.x, n);

            double vSn_nM05 = g.GetSn(PN.vSn, n - 0.5);
            double p_n = g.GetSn(PN.p, n);
            double S_n = bs.S(x_n);
            double q = projectile.q;
            double tau = this.tau;

            var res = MainFunctions.vSn_nP05(vSn_nM05, p_n, S_n, q, tau);
            return res;

        }

        public double Get_x(LimitedDouble N)
        {
            if (N.IsHalfInt())
            {
                var n = OffseterN.AppointAndOffset(N, + 0.5);

                double x_nM05 = g.GetSn(PN.x, n - 0.5);
                double vSn_nM05 = g.GetSn(PN.vSn, n - 0.5);
                double vSn_nP05 = g.GetSn(PN.vSn, n + 0.5);
                double tau = this.tau;

                var res = MainFunctions.x_Sn_nP05(x_nM05, vSn_nM05, vSn_nP05, tau);
                return res;
            }
            else
            {
                var n = OffseterN.AppointAndOffset(N, + 1);

                double x_n = g.GetSn(PN.x, n);
                double vSn_nP05 = g.GetSn(PN.vSn, n + 0.5);
                double tau = this.tau;

                var res = MainFunctions.x_Sn_nP1(x_n, vSn_nP05, tau);
                return res;
            }
        }

        public double Get_z(LimitedDouble N)
        {
            var n = OffseterN.AppointAndOffset(N, + 1);
  
            double z_n = g.GetSn(PN.z, n);
            double H5_nP05 = hf.sn.H5(n + 0.5);
            double tau = this.tau;

            var res = MainFunctions.z_Sn_nP1(z_n, H5_nP05, tau);
            return res;
        }

        public void Update(IGrid grid)
        {
            g = grid;
            wc.Update(grid);
            hf.Update(grid);
        }
    }
}

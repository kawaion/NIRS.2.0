using MyDouble;
using System;
using NIRS.Grid_Folder;
using NIRS.Helpers;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Parameter_names;
using NIRS.Nabla_Functions;
using NIRS.Cannon_Folder.Barrel_Folder;
using NIRS.H_Functions;
using NIRS.Cannon_Folder.Powder_Folder;
using NIRS.Additional_calculated_values;
using NIRS.Interfaces;
using System.Diagnostics;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using NIRS.BPMN_folder;

namespace NIRS.Functions_for_numerical_method
{
    class FunctionsParametersOfTheNextLayerWithOffseter : IFunctionsParametersOfTheNextLayer
    {
        private IGrid g;
        private readonly IConstParameters constP;
        private IWaypointCalculator wc;
        private readonly IBarrelSize bs;
        private IHFunctions hf;
        private readonly IPowder powder;
        private double tau;

        private readonly XGetter x;

        public FunctionsParametersOfTheNextLayerWithOffseter(   IGrid grid, 
                                                    IWaypointCalculator waypointCalculator, 
                                                    IHFunctions hFunctions,
                                                    IMainData mainData)
        {
            g = grid;
            wc = waypointCalculator;
            hf = hFunctions;
            constP = mainData.ConstParameters;
            bs = mainData.Barrel.BarrelSize;
            powder = mainData.Powder;

            x = new XGetter(mainData.ConstParameters);
            tau = mainData.ConstParameters.tau;
        }

        public double Get(PN pn, LimitedDouble n, LimitedDouble k)
        {
            switch (pn)
            {
                case PN.dynamic_m: return Get_dynamic_m(n, k);
                case PN.v: return Get_v(n, k);
                case PN.M: return Get_M(n, k);
                case PN.w: return Get_w(n, k);
                case PN.r: return Get_r(n, k);
                case PN.e: return Get_e(n, k);
                case PN.psi: return Get_psi(n, k);
                case PN.z: return Get_z(n, k);
                case PN.a: return Get_a(n, k);
                case PN.m: return Get_m(n, k);
                case PN.p: return Get_p(n, k);
                case PN.rho: return Get_rho(n, k);
                default: throw new Exception("неинициализированный параметр");
            }
        }


        public double Get_dynamic_m(LimitedDouble n, LimitedDouble k)
        {
            if (n == 8.5)
            {
                int c = 0;
            }
            (n, k) = OffseterNK.AppointAndOffset(n, +0.5, k, 0);

            double x_kM05 = x[k - 0.5];
            double x_kP05 = x[k + 0.5];

            double dynamicm_nM05_k = g[PN.dynamic_m, n - 0.5, k];
            double nabla_dynamicm_v_nM05_k = wc.Nabla(PN.dynamic_m, PN.v, n - 0.5, k);
            double m_n_kM05 = g[PN.m, n, k - 0.5];
            double m_n_kP05 = g[PN.m, n, k + 0.5];
            double S_kM05 = bs.S(x_kM05);
            double S_kP05 = bs.S(x_kP05);
            double dpStrokeDivDx_n_k = wc.dPStrokeDivdx(n, k);
            double H1_n_k = hf.H1(n, k);
            double tau = this.tau;

            var res = MainFunctions.dynamic_m_nP05_k(dynamicm_nM05_k, nabla_dynamicm_v_nM05_k, m_n_kM05, m_n_kP05,
                                                     S_kM05, S_kP05, dpStrokeDivDx_n_k, H1_n_k, tau);
            return res;
        }
        public double Get_v(LimitedDouble n, LimitedDouble k)
        {
            if (n == 1.5)
            {
                int c = 0;
            }
            (n, k) = OffseterNK.AppointAndOffset(n ,0.5, k ,0);

            double dynamicm_nP05_k = g[PN.dynamic_m, n + 0.5, k];
            double r_n_kM05 = g[PN.r, n, k - 0.5];
            double r_n_kP05 = g[PN.r, n, k + 0.5];

            var res = MainFunctions.v_nP05_k(dynamicm_nP05_k, r_n_kM05, r_n_kP05);
            return res;
        }        
        public double Get_M(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.AppointAndOffset(n ,0.5, k ,0);

            double x_kM05 = x[k - 0.5];
            double x_kP05 = x[k + 0.5];

            double M_nM05_k = g[PN.M, n - 0.5, k];
            double nabla_Mv_nM05_k = wc.Nabla(PN.M, PN.w, n - 0.5, k);
            double oneMm_n_kM05 = g[PN.One_minus_m, n, k - 0.5];
            double oneMm_n_kP05 = g[PN.One_minus_m, n, k + 0.5];
            double S_kM05 = bs.S(x_kM05);
            double S_kP05 = bs.S(x_kP05);
            double dpStrokeDivDx_n_k = wc.dPStrokeDivdx(n, k);
            double H2_n_k = hf.H2(n, k);
            double tau = this.tau;

            var res = MainFunctions.M_nP05_k(M_nM05_k, nabla_Mv_nM05_k,
                                              oneMm_n_kM05, oneMm_n_kP05,
                                              S_kM05, S_kP05,
                                              dpStrokeDivDx_n_k, H2_n_k,
                                              tau);
            return res;
        }
        public double Get_w(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.AppointAndOffset(n, 0.5, k, 0);

            double x_kM05 = x[k - 0.5];
            double x_kP05 = x[k + 0.5];

            double M_nP05_k = g[PN.M, n + 0.5, k];
            double oneMm_n_kM05 = g[PN.One_minus_m, n, k - 0.5];
            double oneMm_n_kP05 = g[PN.One_minus_m, n, k + 0.5];
            double S_kM05 = bs.S(x_kM05);
            double S_kP05 = bs.S(x_kP05);
            double delta = constP.PowderDelta;

            var res = MainFunctions.w_nP05_k(M_nP05_k, oneMm_n_kM05, oneMm_n_kP05,
                                             S_kM05, S_kP05, delta);
            return res;
        }     


        public double Get_r(LimitedDouble n, LimitedDouble k)
        {
            if (n == 2 && k == 0.5)
            {
                int c = 0;
            }
            (n, k) = OffseterNK.AppointAndOffset(n, 1, k, -0.5);

            double r_n_kM05 = g[PN.r, n, k - 0.5];
            double nabla_rv_nP05_kM05 = wc.Nabla(PN.r, PN.v, n + 0.5, k - 0.5);
            double H3_nP05_kM05 = hf.H3(n + 0.5, k - 0.5);
            double tau = this.tau;

            var res = MainFunctions.r_nP1_kM05(r_n_kM05, nabla_rv_nP05_kM05, H3_nP05_kM05, tau);
            return res;
        }        
        public double Get_e(LimitedDouble N, LimitedDouble K)
        {            
            if (N == 2)
            {
                int c = 0;
            }
            (var n,var k) = OffseterNK.AppointAndOffset(N, 1, K, -0.5);

            double e_n_kM05 = g[PN.e, n, k - 0.5];
            double nabla_ev_nP05_kM05 = wc.Nabla(PN.e, PN.v, n + 0.5, k - 0.5);
            double p_n_kM05 = g[PN.p, n, k - 0.5];
            double q_nP05_kM05 = q(n + 0.5, k - 0.5);
            double nabla_mSv_nP05_kM05 = wc.Nabla(PN.m, PN.S, PN.v, n + 0.5, k - 0.5);
            double nabla_oneMmSw_nP05_kM05 = wc.Nabla(1, PN.S, PN.w, n + 0.5, k - 0.5) - wc.Nabla(PN.m, PN.S, PN.w, n + 0.5, k - 0.5);
            double H4_nP05_kM05 = hf.H4(n + 0.5, k - 0.5);
            double tau = this.tau;

            var res = MainFunctions.e_nP1_kM05(e_n_kM05, nabla_ev_nP05_kM05,
                               p_n_kM05, q_nP05_kM05,
                               nabla_mSv_nP05_kM05, nabla_oneMmSw_nP05_kM05,
                               H4_nP05_kM05,
                               tau);
            return res;
        }
        public double Get_psi(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.AppointAndOffset(n, 1, k, -0.5);

            double res;
            double z = g[PN.z, n + 1, k - 0.5];
            if (z >= 1)
            {
                double psi_n_kM05 = g[PN.psi, n, k - 0.5];
                double nabla_psiw_nP05_kM05 = wc.Nabla(PN.psi, PN.w, n + 0.5, k - 0.5);
                double nabla_w_nP05_kM05 = wc.Nabla(PN.w, n + 0.5, k - 0.5);
                double HPsi_nP05_kM05 = hf.HPsi(n + 0.5, k - 0.5);
                double tau = this.tau;

                res = MainFunctions.psi_nP1_kM05(psi_n_kM05, nabla_psiw_nP05_kM05, nabla_w_nP05_kM05, HPsi_nP05_kM05, tau);
            }
            else
                res = powder.BurningPowdersSize.Psi(z);

            res = Validation.Validation01(res);

            return res;
        }
        public double Get_z(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.AppointAndOffset(n, 1, k, -0.5);
            //if (n == 1 && k == 0)
            //{
            //    int c = 0;
            //}
            double z_n_kM05 = g[PN.z, n, k - 0.5];
            double nabla_zw_nP05_kM05 = wc.Nabla(PN.z, PN.w, n + 0.5, k - 0.5);
            double nabla_w_nP05_kM05 = wc.Nabla(PN.w, n + 0.5, k - 0.5);
            double H5_nP05_kM05 = hf.H5(n + 0.5, k - 0.5);
            double tau = this.tau;

            var res = MainFunctions.z_nP1_kM05(z_n_kM05, nabla_zw_nP05_kM05, nabla_w_nP05_kM05, H5_nP05_kM05, tau);
            //if (double.IsNaN(res))
            //{
            //    int c = 0;
            //}
            return res;
        }   
        public double Get_a(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.AppointAndOffset(n, 1, k, -0.5);

            double x_kM05 = x[k - 0.5];

            double a_n_kM05 = g[PN.a, n, k - 0.5];
            double nabla_aSw_nP05_kM05 = wc.Nabla(PN.a, PN.S, PN.w, n + 0.5, k - 0.5);
            double S_kM05 = bs.S(x_kM05);
            double tau = this.tau;

            var res = MainFunctions.a_nP1_kM05(a_n_kM05, nabla_aSw_nP05_kM05, S_kM05, tau);
            return res;
        }
        public double Get_p(LimitedDouble n, LimitedDouble k)
        {
            if (n == 9 && k == 0.5)
            {
                int c = 0;
            }
            (n, k) = OffseterNK.AppointAndOffset(n, 1, k, -0.5);

            double x_kM05 = x[k - 0.5];

            double e_nP1_kM05 = g[PN.e, n + 1, k - 0.5];
            double m_nP1_kM05 = g[PN.m, n + 1, k - 0.5];
            double S_kM05 = bs.S(x_kM05);
            double r_nP1_kM05 = g[PN.r, n + 1, k - 0.5];
            double teta = constP.teta;
            double alpha = constP.alpha;

            var res = MainFunctions.p_nP1_kM05(e_nP1_kM05, m_nP1_kM05, S_kM05, r_nP1_kM05, teta, alpha);
            return res;
        }
        public double Get_m(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.AppointAndOffset(n, 1, k, -0.5);

            double a_nP1_kM05 = g[PN.a, n + 1, k - 0.5];
            double psi_nP1_kM05 = g[PN.psi, n + 1, k - 0.5];
            double LAMBDA0 = powder.LAMBDA0;

            var res = MainFunctions.m_nP1_kM05(a_nP1_kM05, psi_nP1_kM05, LAMBDA0);
            res = Validation.Validation01(res);
            if (res < 0) throw new Exception();
            return res;
        }







        public double Get_rho(LimitedDouble n, LimitedDouble k)
        {
            double x_k = x[k];

            double r_n_k = g[PN.r, n, k];
            double m_n_k = g[PN.m, n, k];
            double S_k = bs.S(x_k);

            var res = MainFunctions.rho_n_k(r_n_k, m_n_k, S_k);
            return res;
        }





        private double q(LimitedDouble n, LimitedDouble k)
        {
            return PseudoViscosityMechanism.q(g, wc, constP, n, k);
        }

        public void Update(IGrid grid)
        {
            g = grid;
            wc.Update(grid);
            hf.Update(grid);
        }
    }
}

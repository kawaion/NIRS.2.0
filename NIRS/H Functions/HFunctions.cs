using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDouble;
using NIRS.Cannon_Folder.Barrel_Folder;
using NIRS.Cannon_Folder.Powder_Folder;
using NIRS.Grid_Folder;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Helpers;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using NIRS.BPMN_folder;

namespace NIRS.H_Functions
{
    class HFunctions : IHFunctions
    {
        private IGrid g;
        private readonly IBarrelSize bs;
        private readonly IPowder powder;
        private readonly IBurningPowdersSize bps;
        private readonly IConstParameters constP;

        private readonly XGetter x;

        public IHFunctionsProjectile sn { get; set; }

        public HFunctions(IGrid grid,
                          IMainData mainData)
        {
            g = grid;
            bs = mainData.Barrel.BarrelSize;
            powder = mainData.Powder;
            bps = mainData.Powder.BurningPowdersSize;
            constP = mainData.ConstParameters;

            x = new XGetter(mainData.ConstParameters);

            sn = new HFunctionsProjectile(grid, mainData);
        }

        public double H1(LimitedDouble n, LimitedDouble k)
        {
            double x_k = x[k];

            double S_k = bs.S(x_k);
            double tau_w_n_k = tauW(n, k);
            double G_n_k = G(n, k);
            double w_nM05_k = g[PN.w, n - 0.5, k];

            var res = MainFunctions.H1_n_k(S_k, tau_w_n_k, G_n_k, w_nM05_k);
            return res;
        }

        public double H2(LimitedDouble n, LimitedDouble k)
        {
            double x_k = x[k];

            double S_k = bs.S(x_k);
            double tau_w_n_k = tauW(n, k);
            double G_n_k = G(n, k);
            double w_nM05_k = g[PN.w, n - 0.5, k];

            var res = MainFunctions.H2_n_k(S_k, tau_w_n_k, G_n_k, w_nM05_k);
            return res;
        }

        public double H3(LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.AppointAndOffset(N, + 0.5, K, - 0.5);

            double x_kM05 = x[k - 0.5];

            double S_kM05 = bs.S(x_kM05);
            double G_n_k = G(n, k);

            var res = MainFunctions.H3_nP05_kM05(S_kM05, G_n_k);
            return res;
        }

        public double H4(LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.AppointAndOffset(N, + 0.5, K, - 0.5);

            double x_kM05 = x[k - 0.5];

            double S_kM05 = bs.S(x_kM05);
            double G_n_k = G(n, k);
            double v_nP05_k = g[PN.v, n + 0.5, k];
            double w_nP05_k = g[PN.w, n + 0.5, k];
            double tau_w_nP1_k = tauW(n + 1, k);
            double Q = constP.Q;

            var res = MainFunctions.H4_nP05_kM05(S_kM05, G_n_k, v_nP05_k, w_nP05_k, tau_w_nP1_k, Q);
            return res;
        }

        public double H5(LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.AppointAndOffset(N, + 0.5, K, - 0.5);

            double uk_p_n_kM05 = bps.Uk(g[PN.p, n, k - 0.5]);
            double e1 = constP.e1;

            var res = MainFunctions.H5_nP05_kM05(uk_p_n_kM05, e1);
            return res;
        }

        public double HPsi(LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.AppointAndOffset(N, + 0.5, K, - 0.5);

            double S0 = powder.S0;
            double LAMBDA0 = powder.LAMBDA0;
            double sigma_psi_n_kM05 = bps.Sigma(g[PN.z, n, k - 0.5], g[PN.psi, n, k - 0.5]);
            double uk_p_n_kM05 = bps.Uk(g[PN.p, n, k - 0.5]);

            var res = MainFunctions.HPsi_nP05_kM05(S0, LAMBDA0, sigma_psi_n_kM05, uk_p_n_kM05);
            return res;
        }


        private double tauW(LimitedDouble n, LimitedDouble k)
        {
            double rho_n_kM05 = g[PN.rho, n, k - 0.5];
            double v_nM05_k = g[PN.v, n - 0.5, k];
            double w_nM05_k = g[PN.w, n - 0.5, k];
            double a_n_kM05 = g[PN.a, n, k - 0.5];
            double sigma_z_n_kM05 = bps.Sigma(g[PN.z, n, k - 0.5], g[PN.psi, n, k - 0.5]);
            double lambda0 = constP.lamda0;
            double S0 = powder.S0;

            var res = MainFunctions.tau_w_n_k(rho_n_kM05, v_nM05_k, w_nM05_k, a_n_kM05, sigma_z_n_kM05, lambda0, S0);
            return res;
        }
        private double G(LimitedDouble n, LimitedDouble k)
        {
            double a_n_kM05 = g[PN.a, n, k - 0.5];
            double S0 = powder.S0;
            double sigma_z_n_kM0 = bps.Sigma(g[PN.z, n, k - 0.5], g[PN.psi, n, k - 0.5]);
            double delta = constP.PowderDelta;
            double uk_p_n_kM05 = bps.Uk(g[PN.p, n, k - 0.5]);

            var res = MainFunctions.G_n_k(a_n_kM05, S0, sigma_z_n_kM0, delta, uk_p_n_kM05);
            return res;
        }

        public void Update(IGrid grid)
        {
            g = grid;
        }
    }
}

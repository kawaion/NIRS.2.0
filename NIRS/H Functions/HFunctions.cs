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

        public double H1(double n, double k)
        {
            double S_k = bs.S(x[k]);
            double tau_w_n_k = tauW(n, k);
            double G_n_k = G(n, k);
            double w_nM05_k = g[PN.w, n - 0.5, k];

            var res = BPMN.H1_n_k(S_k, tau_w_n_k,
                                  G_n_k, w_nM05_k);
            return res;
        }

        public double H2(double n, double k)
        {
            return bs.S(x[k]) * tauW(n, k) - bs.S(x[k]) * G(n, k) * g[PN.w, n - 0.5, k];
        }

        public double H3(double N, double K)
        {
            (var n, var k) = OffseterNK.AppointAndOffset(N, + 0.5, K, - 0.5);
            return bs.S(x[k - 0.5]) * G(n, k);
        }

        public double H4(double N, double K)
        {
            (var n, var k) = OffseterNK.AppointAndOffset(N, + 0.5, K, - 0.5);
            double diff_v_w = g[PN.v, n + 0.5, k] - g[PN.w, n + 0.5, k];
            var res = bs.S(x[k - 0.5]) * G(n, k) * (constP.Q + Math.Pow(diff_v_w, 2) / 2) 
                    + bs.S(x[k - 0.5]) * tauW(n + 1, k) * diff_v_w;
            if (double.IsInfinity(res))
            {
                var tmp1 = diff_v_w;
                var tmp2 = bs.S(x[k - 0.5]);
                var tmp3 = G(n, k);
                var tmp4 = constP.Q + Math.Pow(diff_v_w, 2);
                var tmp5 = bs.S(x[k - 0.5]);
                var tmp6 = tauW(n + 1, k);
            }
            return res;
        }

        public double H5(double N, double K)
        {
            (var n, var k) = OffseterNK.AppointAndOffset(N, + 0.5, K, - 0.5);
            return bps.Uk(g[PN.p, n, k - 0.5]) / constP.e1;
        }

        public double HPsi(double N, double K)
        {
            (var n, var k) = OffseterNK.AppointAndOffset(N, + 0.5, K, - 0.5);
            return powder.S0 / powder.LAMDA0 * bps.Sigma(g[PN.z, n, k - 0.5], g[PN.psi, n, k - 0.5]) * bps.Uk(g[PN.p, n, k - 0.5]);
        }


        private double tauW(double n, double k)
        {
            double diff_v_w = g[PN.v, n - 0.5, k] - g[PN.w, n - 0.5, k];
            var res = constP.lamda0 * (g[PN.ro, n, k - 0.5] * diff_v_w * Math.Abs(diff_v_w)) / 2
                   * g[PN.a, n, k - 0.5] * (powder.S0 * bps.Sigma(g[PN.z, n, k - 0.5], g[PN.psi, n, k - 0.5])) / 4;
            if (double.IsInfinity(res))
            {
                var tmp1 = diff_v_w;
                var tmp2 = bps.Sigma(g[PN.z, n, k - 0.5], g[PN.psi, n, k - 0.5]);
                var tmp3 = constP.lamda0 * (g[PN.ro, n, k - 0.5] * diff_v_w * Math.Abs(diff_v_w));
                var tmp4 = g[PN.a, n, k - 0.5] * (powder.S0 * bps.Sigma(g[PN.z, n, k - 0.5], g[PN.psi, n, k - 0.5]));
            }
            return res;
        }
        private double G(double n, double k)
        {
            double a_n_kM05 = g[PN.a, n, k - 0.5];
            double S0 = powder.S0;
            double sigma_z_n_kM0 = bps.Sigma(g[PN.z, n, k - 0.5], g[PN.psi, n, k - 0.5]);
            double delta = constP.PowderDelta;
            double uk_p_n_kM05 = bps.Uk(g[PN.p, n, k - 0.5]);

            var res = BPMN.G_n_k(a_n_kM05, S0, sigma_z_n_kM0, delta, uk_p_n_kM05);
            return res;
        }

        public void Update(IGrid grid)
        {
            g = grid;
        }
    }
}

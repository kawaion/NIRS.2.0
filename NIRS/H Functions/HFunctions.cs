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

namespace NIRS.H_Functions
{
    class HFunctions : IHFunctions
    {
        private readonly IGrid g;
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
            var tmp1 = bs.S(x[k]);
            var tmp2 = tauW(n, k);
            var tmp3 = G(n, k);

            return -bs.S(x[k]) * tauW(n, k) + bs.S(x[k]) * G(n, k) * g[n - 0.5][k].w;
        }

        public double H2(LimitedDouble n, LimitedDouble k)
        {
            var tmp1 = bs.S(x[k]);
            var tmp2 = tauW(n, k);
            var tmp3 = bs.S(x[k]);
            var tmp4 = G(n, k);
            var tmp5 = g[n - 0.5][k].w;

            return bs.S(x[k]) * tauW(n, k) - bs.S(x[k]) * G(n, k) * g[n - 0.5][k].w;
        }

        public double H3(LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.Appoint(N, K).Offset(N + 0.5, K - 0.5);
            return bs.S(x[k - 0.5]) * G(n, k);
        }

        public double H4(LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.Appoint(N, K).Offset(N + 0.5, K - 0.5);
            double diff_v_w = g[n + 0.5][k].v - g[n + 0.5][k].w;
            return bs.S(x[k - 0.5]) * G(n, k) * (constP.Q + Math.Pow(diff_v_w, 2) / 2) 
                    + bs.S(x[k - 0.5]) * tauW(n + 1, k) * diff_v_w;
        }

        public double H5(LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.Appoint(N, K).Offset(N + 0.5, K - 0.5);
            return bps.Uk(g[n][k - 0.5].p) / constP.e1;
        }

        public double HPsi(LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.Appoint(N, K).Offset(N + 0.5, K - 0.5);
            return powder.S0 / powder.LAMDA0 * bps.Sigma(g[n][k - 0.5].z, g[n][k - 0.5].psi) * bps.Uk(g[n][k - 0.5].p);
        }


        private double tauW(LimitedDouble n, LimitedDouble k)
        {
            double diff_v_w = g[n - 0.5][k].v - g[n - 0.5][k].w;
            return constP.lamda0 * (g[n][k - 0.5].ro * diff_v_w * Math.Abs(diff_v_w)) / 2
                   * g[n][k - 0.5].a * (powder.S0 * bps.Sigma(g[n][k - 0.5].z, g[n][k - 0.5].psi)) / 4;
        }
        private double G(LimitedDouble n, LimitedDouble k)
        {
            var tmp1 = g[n][k - 0.5].a;
            var tmp2 = powder.S0;
            var tmp3 = bps.Sigma(g[n][k - 0.5].z, g[n][k - 0.5].psi);
            var tmp4 = constP.PowderDelta;
            var tmp5 = bps.Uk(g[n][k - 0.5].p);

            return g[n][k - 0.5].a * powder.S0 * bps.Sigma(g[n][k - 0.5].z, g[n][k - 0.5].psi) * constP.PowderDelta * bps.Uk(g[n][k - 0.5].p);
        }
    }
}

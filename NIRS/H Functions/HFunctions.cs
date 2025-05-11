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
            return -bs.S(x[k]) * tauW(n, k) + bs.S(x[k]) * G(n, k) * g[PN.w, n - 0.5, k];
        }

        public double H2(LimitedDouble n, LimitedDouble k)
        {
            return bs.S(x[k]) * tauW(n, k) - bs.S(x[k]) * G(n, k) * g[PN.w, n - 0.5, k];
        }

        public double H3(LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.AppointAndOffset(N, + 0.5, K, - 0.5);
            return bs.S(x[k - 0.5]) * G(n, k);
        }

        public double H4(LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.AppointAndOffset(N, + 0.5, K, - 0.5);
            double diff_v_w = g[PN.v, n + 0.5, k] - g[PN.w, n + 0.5, k];
            return bs.S(x[k - 0.5]) * G(n, k) * (constP.Q + Math.Pow(diff_v_w, 2) / 2) 
                    + bs.S(x[k - 0.5]) * tauW(n + 1, k) * diff_v_w;
        }

        public double H5(LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.AppointAndOffset(N, + 0.5, K, - 0.5);
            return bps.Uk(g[PN.p, n, k - 0.5]) / constP.e1;
        }

        public double HPsi(LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.AppointAndOffset(N, + 0.5, K, - 0.5);
            return powder.S0 / powder.LAMDA0 * bps.Sigma(g[PN.z, n, k - 0.5], g[PN.psi, n, k - 0.5]) * bps.Uk(g[PN.p, n, k - 0.5]);
        }


        private double tauW(LimitedDouble n, LimitedDouble k)
        {
            double diff_v_w = g[PN.v, n - 0.5, k] - g[PN.w, n - 0.5, k];
            return constP.lamda0 * (g[PN.ro, n, k - 0.5] * diff_v_w * Math.Abs(diff_v_w)) / 2
                   * g[PN.a, n, k - 0.5] * (powder.S0 * bps.Sigma(g[PN.z, n, k - 0.5], g[PN.psi, n, k - 0.5])) / 4;
        }
        private double G(LimitedDouble n, LimitedDouble k)
        {
            return g[PN.a, n, k - 0.5] * powder.S0 * bps.Sigma(g[PN.z, n, k - 0.5], g[PN.psi, n, k - 0.5]) * constP.PowderDelta * bps.Uk(g[PN.p, n, k - 0.5]);
        }

        public void Update(IGrid grid)
        {
            g = grid;
        }
    }
}

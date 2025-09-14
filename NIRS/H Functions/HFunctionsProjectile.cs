using MyDouble;
using NIRS.BPMN_folder;
using NIRS.Helpers;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.H_Functions
{
    class HFunctionsProjectile : IHFunctionsProjectile
    {
        private readonly IGrid g;
        private readonly IBarrelSize bs;
        private readonly IPowder powder;
        private readonly IBurningPowdersSize bps;
        private readonly IConstParameters constP;

        public HFunctionsProjectile(IGrid grid, IMainData mainData)
        {
            g = grid;
            bs = mainData.Barrel.BarrelSize;
            powder = mainData.Powder;
            bps = mainData.Powder.BurningPowdersSize;
            constP = mainData.ConstParameters;
        }

        public double H3(double N)
        {
            var n = OffseterN.AppointAndOffset(N, + 0.5);

            var x_n = g.GetSn(PN.x, n);

            double S_n = bs.S(x_n);
            double G_n = G(n);

            var res = MainFunctions.H3_Sn_nP05(S_n, G_n);
            return res;
        }

        public double H4(double N)
        {
            var n = OffseterN.AppointAndOffset(N, + 0.5);

            var x_n = g.GetSn(PN.x, n);

            double S_n = bs.S(x_n);
            double G_n = G(n);
            double Q = constP.Q;

            var res = MainFunctions.H4_Sn_nP05(S_n, G_n, Q);
            return res;
        }

        public double H5(double N)
        {
            var n = OffseterN.AppointAndOffset(N, + 0.5);

            double Uk_n = bps.Uk(g.GetSn(PN.p, n));
            double e1 = constP.e1;

            var res = MainFunctions.H5_Sn_nP05(Uk_n, e1);
            return res;
        }

        public double HPsi(double N)
        {
            var n = OffseterN.AppointAndOffset(N, + 0.5);

            double Sigma_n = bps.Sigma(g.GetSn(PN.z, n), g.GetSn(PN.psi, n));
            double Uk_n = bps.Uk(g.GetSn(PN.p, n));
            double S0 = powder.S0;
            double LAMBDA0 = powder.LAMBDA0;

            var res = MainFunctions.HPsi_Sn_nP05(Sigma_n, Uk_n, S0, LAMBDA0);
            return res;
        }

        private double G(double n)
        {
            double a_n = g.GetSn(PN.a, n);
            double Sigma_n = bps.Sigma(g.GetSn(PN.z, n), g.GetSn(PN.psi, n));
            double Uk_n = bps.Uk(g.GetSn(PN.p, n));
            double S0 = powder.S0;
            double Delta = constP.PowderDelta;

            var res = MainFunctions.G_Sn_n(a_n, Sigma_n, Uk_n, S0, Delta);
            return res;
        }
    }
}

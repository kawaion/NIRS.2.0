using MyDouble;
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

            return bs.S(x_n) * G(n);
        }

        public double H4(double N)
        {
            var n = OffseterN.AppointAndOffset(N, + 0.5);

            var x_n = g.GetSn(PN.x, n);

            return bs.S(x_n) * G(n) * constP.Q;
        }

        public double H5(double N)
        {
            var n = OffseterN.AppointAndOffset(N, + 0.5);

            return bps.Uk(g.GetSn(PN.p, n)) / constP.e1;
        }

        public double HPsi(double N)
        {
            var n = OffseterN.AppointAndOffset(N, + 0.5);

            return (powder.S0 / powder.LAMDA0)
                   * bps.Sigma(g.GetSn(PN.z, n), g.GetSn(PN.psi, n))
                   * bps.Uk(g.GetSn(PN.p, n));
        }

        private double G(double n)
        {
            return g.GetSn(PN.a, n) * powder.S0 * bps.Sigma(g.GetSn(PN.z, n), g.GetSn(PN.psi, n)) * constP.PowderDelta * bps.Uk(g.GetSn(PN.p, n));
        }
    }
}

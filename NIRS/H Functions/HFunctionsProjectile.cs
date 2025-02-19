using MyDouble;
using NIRS.Helpers;
using NIRS.Interfaces;
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
            bs = mainData.BarrelSize;
            powder = mainData.Powder;
            bps = mainData.BurningPowdersSize;
            constP = mainData.ConstParameters;
        }

        public double H3(LimitedDouble n)
        {
            n = OffseterN.Appoint(n).Offset(n + 0.5);

            var x_n = g[n].sn.x;

            return bs.S(x_n) * G(n);
        }

        public double H4(LimitedDouble n)
        {
            n = OffseterN.Appoint(n).Offset(n + 0.5);

            var x_n = g[n].sn.x;

            return bs.S(x_n) * G(n) * constP.Q;
        }

        public double H5(LimitedDouble n)
        {
            n = OffseterN.Appoint(n).Offset(n + 0.5);

            return bps.Uk(g[n].sn.p) / constP.e1;
        }

        public double HPsi(LimitedDouble n)
        {
            n = OffseterN.Appoint(n).Offset(n + 0.5);

            return (powder.S0 / powder.LAMDA0)
                   * bps.Sigma(g[n].sn.z, g[n].sn.psi)
                   * bps.Uk(g[n].sn.p);
        }

        private double G(LimitedDouble n)
        {
            return g[n].sn.a * powder.S0 * bps.Sigma(g[n].sn.z, g[n].sn.psi) * constP.delta * bps.Uk(g[n].sn.p);
        }
    }
}

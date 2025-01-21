using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDouble;
using NIRS.Cannon_Folder.Barrel_Folder;
using NIRS.Cannon_Folder.Powder_Folder;
using NIRS.Grid_Folder;
using NIRS.Grid_Folder.Mediator;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Helpers;

namespace NIRS.H_Functions
{
    class HFunctions : IHFunctions
    {
        private readonly IGrid g;
        private readonly IBarrelSize bs;
        private readonly IPowder powder;
        private readonly ICombustionFunctions cf;
        private readonly IConstParameters constP;

        public HFunctions(IGrid grid,
                          IBarrelSize barrelSize,
                          IPowder powderElement,
                          ICombustionFunctions combustionFunctions,
                          IConstParameters constParameters)
        {
            g = grid;
            bs = barrelSize;
            powder = powderElement;
            cf = combustionFunctions;
            constP = constParameters;
        }

        public double H1(LimitedDouble n, LimitedDouble k)
        {
            return - bs.SByIndex(k) * tauW(n, k) + bs.SByIndex(k) * G(n, k) * g.w(n - 0.5, k);
        }

        public double H2(LimitedDouble n, LimitedDouble k)
        {
            return bs.SByIndex(k) * tauW(n, k) - bs.SByIndex(k) * G(n, k) * g.w(n - 0.5, k);
        }

        public double H3(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 0.5, k - 0.5);
            return bs.SByIndex(k - 0.5) * G(n, k);
        }

        public double H4(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 0.5, k - 0.5);
            double diff_v_w = g.v(n + 0.5, k) - g.w(n + 0.5, k);
            return bs.SByIndex(k - 0.5) * G(n, k) * (constP.Q + Math.Pow(diff_v_w, 2) / 2) 
                    + bs.SByIndex(k - 0.5) * tauW(n + 1, k) * diff_v_w;
        }

        public double H5(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 0.5, k - 0.5);
            return cf.Uk(g.p(n, k - 0.5)) / constP.e1;
        }

        public double HPsi(LimitedDouble n, LimitedDouble k)
        {
            throw new NotImplementedException();
        }


        private double tauW(LimitedDouble n, LimitedDouble k)
        {
            double diff_v_w = g.v(n - 0.5, k) - g.w(n - 0.5, k);
            return constP.lamda0 * (g.ro(n, k - 0.5) * diff_v_w * Math.Abs(diff_v_w)) / 2
                   * g.a(n, k - 0.5) * (powder.S0 * cf.Sigma(g.z(n, k - 0.5), g.psi(n, k - 0.5))) / 4;
        }
        private double G(LimitedDouble n, LimitedDouble k)
        {
            return g.a(n,k - 0.5) * powder.S0 * cf.Sigma(g.z(n, k - 0.5), g.psi(n, k - 0.5)) * constP.delta * cf.Uk(g.p(n, k - 0.5));
        }
    }
}

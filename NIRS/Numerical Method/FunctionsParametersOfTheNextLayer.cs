using MyDouble;
using System;
using NIRS.Grid_Folder;
using NIRS.Helpers;
using NIRS.Grid_Folder.Mediator;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Parameter_names;
using NIRS.Nabla_Functions;
using NIRS.Cannon_Folder.Barrel_Folder;


namespace NIRS.Numerical_Method
{
    class FunctionsParametersOfTheNextLayer : IFunctionsParametersOfTheNextLayer
    {
        private readonly IGrid g;
        private readonly IConstParameters constP;
        private readonly INabla wc;
        private readonly IBarrelSize bs;

        public FunctionsParametersOfTheNextLayer(IGrid grid,IConstParameters constParameters,INabla wc,IBarrelSize bs)
        {
            g = grid;
            constP = constParameters;
            this.wc = wc;
            this.bs = bs;
        }
        public double Calc_a(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 1, k - 0.5);
            return g.a(n,k-0.5) - constP.tau *
                (
                    wc.Nabla(PN.a, PN.S, PN.w, n + 0.5, k - 0.5) / bs.SByIndex(k - 0.5)
                );
        }

        public double Calc_dynamic_m(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 0.5, k);
            return g.dynamic_m(n - 0.5, k) - constP.tau *
                (
                    wc.Nabla(PN.dynamic_m, PN.v, n - 0.5, k)
                   + (_grid[n][k - 0.5].m * _cannon.Barrel.S((k - 0.5) * Step.h) + _grid[n][k + 0.5].m * _cannon.Barrel.S((k + 0.5) * Step.h)) / 2 * wc.dDivdx("pStroke", n, k)
                   - mcn.H1(n, k)
                );
        }

        public double Calc_e(LimitedDouble n, LimitedDouble k)
        {
            throw new NotImplementedException();
        }

        public double Calc_M(LimitedDouble n, LimitedDouble k)
        {
            throw new NotImplementedException();
        }

        public double Calc_m(LimitedDouble n, LimitedDouble k)
        {
            throw new NotImplementedException();
        }

        public double Calc_p(LimitedDouble n, LimitedDouble k)
        {
            throw new NotImplementedException();
        }

        public double Calc_psi(LimitedDouble n, LimitedDouble k)
        {
            throw new NotImplementedException();
        }

        public double Calc_r(LimitedDouble n, LimitedDouble k)
        {
            throw new NotImplementedException();
        }

        public double Calc_ro(LimitedDouble n, LimitedDouble k)
        {
            throw new NotImplementedException();
        }

        public double Calc_v(LimitedDouble n, LimitedDouble k)
        {
            throw new NotImplementedException();
        }

        public double Calc_w(LimitedDouble n, LimitedDouble k)
        {
            throw new NotImplementedException();
        }

        public double Calc_z(LimitedDouble n, LimitedDouble k)
        {
            throw new NotImplementedException();
        }
    }
}

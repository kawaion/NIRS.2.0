using MyDouble;
using System;
using NIRS.Grid_Folder;


namespace NIRS.Numerical_Method
{
    class FunctionsParametersOfTheNextLayer : IFunctionsParametersOfTheNextLayer
    {
        Grid g;
        public FunctionsParametersOfTheNextLayer(Grid grid)
        {
            g = grid;
        }
        public double Calc_a(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffsetNK.Appoint(n, k).Offset(n + 1, k - 0.5);
            return g[n][k - 0.5].a - Step.tau *
                (
                    wc.Nabla("aS", "w", n + 0.5, k - 0.5) / _cannon.Barrel.S((k - 0.5) * Step.h)
                );
        }

        public double Calc_dynamic_m(LimitedDouble n, LimitedDouble k)
        {
            throw new NotImplementedException();
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

using MyDouble;
using System;
using NIRS.Grid_Folder;
using NIRS.Helpers;
using NIRS.Grid_Folder.Mediator;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Parameter_names;
using NIRS.Nabla_Functions;
using NIRS.Cannon_Folder.Barrel_Folder;
using NIRS.H_Functions;
using NIRS.Cannon_Folder.Powder_Folder;


namespace NIRS.Numerical_Method
{
    class FunctionsParametersOfTheNextLayer : IFunctionsParametersOfTheNextLayer
    {
        private readonly IGrid g;
        private readonly IConstParameters constP;
        private readonly INabla wc;
        private readonly IBarrelSize bs;
        private readonly IHFunctions hf;
        private readonly IPowder powder;

        public FunctionsParametersOfTheNextLayer(IGrid grid,IConstParameters constParameters,INabla wc,IBarrelSize bs,IHFunctions hf, IPowder powder)
        {
            g = grid;
            constP = constParameters;
            this.wc = wc;
            this.bs = bs;
            this.hf = hf;
            this.powder = powder;
        }
        public double Calc_dynamic_m(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 0.5, k);
            return g.dynamic_m(n - 0.5, k) - constP.tau *
                (
                    wc.Nabla(PN.dynamic_m, PN.v).Cell(n - 0.5, k)
                   + (g.dynamic_m(n, k - 0.5) * bs.SByIndex(k - 0.5) + g.dynamic_m(n, k + 0.5) * bs.SByIndex(k + 0.5)) / 2
                        * wc.dDivdx(PN.pStroke).Cell(n, k)
                   - hf.H1(n, k)
                );
        }
        public double Calc_v(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 0.5, k);
            return 2 * g.dynamic_m(n + 0.5, k) 
                    / (g.r(n, k - 0.5) + g.r(n, k + 0.5));
        }        
        public double Calc_M(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 0.5, k);
            return g.M(n - 0.5, k) - constP.tau *
                (
                    wc.Nabla(PN.M, PN.w).Cell(n - 0.5, k)
                   + ((1 - g.m(n, k - 0.5)) * bs.SByIndex(k - 0.5) + (1 - g.m(n, k + 0.5)) * bs.SByIndex(k + 0.5)) / 2 
                        * wc.dDivdx(PN.pStroke, n, k)
                   - hf.H2(n, k)
                );
        }         
        public double Calc_w(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 0.5, k);
            return 2 * g.M(n + 0.5, k)
                    / (constP.delta * (
                                      (1 - g.m(n, k - 0.5)) * bs.SByIndex(k - 0.5) 
                                    + (1 - g.m(n, k + 0.5)) * bs.SByIndex(k + 0.5) 
                                      )
                      );
        }     
        public double Calc_r(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 1, k - 0.5);
            return g.M(n, k - 0.5) - constP.tau *
                (
                    wc.Nabla(PN.r, PN.v).Cell(n + 0.5, k - 0.5)
                   - hf.H3(n + 0.5, k - 0.5)
                );
        }        
        public double Calc_e(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 1, k - 0.5);
            return g.e(n, k - 0.5) - constP.tau *
                (
                    wc.Nabla(PN.e, PN.v).Cell(n + 0.5, k - 0.5)
                   + (g.p(n + 0.5, k - 0.5) + g.q(n + 0.5, k - 0.5)) 
                        * (wc.Nabla(PN.m, PN.S, PN.v).Cell(n + 0.5, k - 0.5) + wc.Nabla(PN.One_minus_m, PN.S, PN.w).Cell(n + 0.5, k - 0.5))
                   - hf.H4(n + 0.5, k - 0.5)
                );
        }
        public double Calc_psi(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 1, k - 0.5);
            return g.psi(n, k - 0.5) - constP.tau *
                (
                    wc.Nabla(PN.psi, PN.w).Cell(n + 0.5, k - 0.5)
                   - g.psi(n, k - 0.5) * wc.Nabla(PN.w).Cell(n + 0.5, k - 0.5)
                   - hf.HPsi(n + 0.5, k - 0.5)
                );
        }
        public double Calc_z(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 1, k - 0.5);
            return g.z(n, k - 0.5) - constP.tau *
                (
                    wc.Nabla(PN.z, PN.w).Cell(n + 0.5, k - 0.5)
                   - g.z(n, k - 0.5) * wc.Nabla(PN.w).Cell(n + 0.5, k - 0.5)
                   - hf.H5(n + 0.5, k - 0.5)
                );
        }   
        public double Calc_a(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 1, k - 0.5);
            return g.a(n,k-0.5) - constP.tau *
                (
                    wc.Nabla(PN.a, PN.S, PN.w).Cell(n + 0.5, k - 0.5) 
                        / bs.SByIndex(k - 0.5)
                );
        }
        public double Calc_p(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 1, k - 0.5);
            return constP.teta * g.e(n + 1, k - 0.5)
                    / (g.m(n + 1, k - 0.5) * bs.SByIndex(k - 0.5) - constP.alpha * g.r(n + 1, k - 0.5));
        }
        public double Calc_m(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 1, k - 0.5);
            return 1 - g.a(n + 1, k - 0.5) * powder.LAMDA0 * (1 - g.psi(n + 1, k - 0.5));
        }







        public double Calc_ro(LimitedDouble n, LimitedDouble k)
        {
            throw new NotImplementedException();
        }






    }
}

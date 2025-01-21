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
        private readonly IWaypointCalculator wc;
        private readonly IBarrelSize bs;
        private readonly IHFunctions hf;
        private readonly IPowder powder;


        public FunctionsParametersOfTheNextLayer(   IGrid grid, 
                                                    IWaypointCalculator waypointCalculator, 
                                                    IHFunctions hFunctions,
                                                    IConstParameters constParameters,
                                                    IBarrelSize barrelSize, 
                                                    IPowder powderElement)
        {
            g = grid;
            wc = waypointCalculator;
            hf = hFunctions;
            constP = constParameters;
            bs = barrelSize;
            powder = powderElement;
        }

        public double Get(PN pN, LimitedDouble n, LimitedDouble k)
        {
            switch (pN)
            {
                case PN.dynamic_m: return Get_dynamic_m(n, k);
                case PN.v: return Get_v(n, k);
                case PN.M: return Get_M(n, k);
                case PN.w: return Get_w(n, k);
                case PN.r: return Get_r(n, k);
                case PN.e: return Get_e(n, k);
                case PN.psi: return Get_psi(n, k);
                case PN.z: return Get_z(n, k);
                case PN.a: return Get_a(n, k);
                case PN.m: return Get_m(n, k);
                case PN.p: return Get_p(n, k);
                case PN.ro: return Get_ro(n, k);
                default: throw new Exception("неинициализированный параметр");
            }
        }


        public double Get_dynamic_m(LimitedDouble n, LimitedDouble k)
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
        public double Get_v(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 0.5, k);
            return 2 * g.dynamic_m(n + 0.5, k) 
                    / (g.r(n, k - 0.5) + g.r(n, k + 0.5));
        }        
        public double Get_M(LimitedDouble n, LimitedDouble k)
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
        public double Get_w(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 0.5, k);
            return 2 * g.M(n + 0.5, k)
                    / (constP.delta * (
                                      (1 - g.m(n, k - 0.5)) * bs.SByIndex(k - 0.5) 
                                    + (1 - g.m(n, k + 0.5)) * bs.SByIndex(k + 0.5) 
                                      )
                      );
        }     
        public double Get_r(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 1, k - 0.5);
            return g.M(n, k - 0.5) - constP.tau *
                (
                    wc.Nabla(PN.r, PN.v).Cell(n + 0.5, k - 0.5)
                   - hf.H3(n + 0.5, k - 0.5)
                );
        }        
        public double Get_e(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 1, k - 0.5);
            return g.e(n, k - 0.5) - constP.tau *
                (
                    wc.Nabla(PN.e, PN.v).Cell(n + 0.5, k - 0.5)
                   + (g.p(n + 0.5, k - 0.5) + q(n + 0.5, k - 0.5)) 
                        * (wc.Nabla(PN.m, PN.S, PN.v).Cell(n + 0.5, k - 0.5) + wc.Nabla(PN.One_minus_m, PN.S, PN.w).Cell(n + 0.5, k - 0.5))
                   - hf.H4(n + 0.5, k - 0.5)
                );
        }
        public double Get_psi(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 1, k - 0.5);
            return g.psi(n, k - 0.5) - constP.tau *
                (
                    wc.Nabla(PN.psi, PN.w).Cell(n + 0.5, k - 0.5)
                   - g.psi(n, k - 0.5) * wc.Nabla(PN.w).Cell(n + 0.5, k - 0.5)
                   - hf.HPsi(n + 0.5, k - 0.5)
                );
        }
        public double Get_z(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 1, k - 0.5);
            return g.z(n, k - 0.5) - constP.tau *
                (
                    wc.Nabla(PN.z, PN.w).Cell(n + 0.5, k - 0.5)
                   - g.z(n, k - 0.5) * wc.Nabla(PN.w).Cell(n + 0.5, k - 0.5)
                   - hf.H5(n + 0.5, k - 0.5)
                );
        }   
        public double Get_a(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 1, k - 0.5);
            return g.a(n,k-0.5) - constP.tau *
                (
                    wc.Nabla(PN.a, PN.S, PN.w).Cell(n + 0.5, k - 0.5) 
                        / bs.SByIndex(k - 0.5)
                );
        }
        public double Get_p(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 1, k - 0.5);
            return constP.teta * g.e(n + 1, k - 0.5)
                    / (g.m(n + 1, k - 0.5) * bs.SByIndex(k - 0.5) - constP.alpha * g.r(n + 1, k - 0.5));
        }
        public double Get_m(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 1, k - 0.5);
            return 1 - g.a(n + 1, k - 0.5) * powder.LAMDA0 * (1 - g.psi(n + 1, k - 0.5));
        }







        public double Get_ro(LimitedDouble n, LimitedDouble k)
        {
            throw new NotImplementedException();
        }





        private double q(LimitedDouble n, LimitedDouble k)
        {
            return PseudoViscosityMechanism.q(g, wc, constP, n, k);
        }
    }
}

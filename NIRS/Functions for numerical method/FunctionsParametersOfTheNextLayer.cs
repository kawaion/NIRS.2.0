using MyDouble;
using System;
using NIRS.Grid_Folder;
using NIRS.Helpers;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Parameter_names;
using NIRS.Nabla_Functions;
using NIRS.Cannon_Folder.Barrel_Folder;
using NIRS.H_Functions;
using NIRS.Cannon_Folder.Powder_Folder;
using NIRS.Additional_calculated_values;
using NIRS.Interfaces;

namespace NIRS.Functions_for_numerical_method
{
    class FunctionsParametersOfTheNextLayer : IFunctionsParametersOfTheNextLayer
    {
        private readonly IGrid g;
        private readonly IConstParameters constP;
        private readonly IWaypointCalculator wc;
        private readonly IBarrelSize bs;
        private readonly IHFunctions hf;
        private readonly IPowder powder;

        private readonly XGetter x;


        public FunctionsParametersOfTheNextLayer(   IGrid grid, 
                                                    IWaypointCalculator waypointCalculator, 
                                                    IHFunctions hFunctions,
                                                    IMainData mainData)
        {
            g = grid;
            wc = waypointCalculator;
            hf = hFunctions;
            constP = mainData.ConstParameters;
            bs = mainData.Barrel.BarrelSize;
            powder = mainData.Powder;

            x = new XGetter(mainData.ConstParameters);
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


        public double Get_dynamic_m(LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.Appoint(N, K).Offset(N + 0.5, K);
            return g[n - 0.5][k].dynamic_m - constP.tau *
                (
                    wc.Nabla(PN.dynamic_m, PN.v).Cell(n - 0.5, k)
                   + (g[n][k - 0.5].m * bs.S(x[k - 0.5]) + g[n][k + 0.5].m * bs.S(x[k + 0.5])) / 2
                        * wc.dPStrokeDivdx().Cell(n, k)
                   - hf.H1(n, k)
                );
        }
        public double Get_v(LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.Appoint(N, K).Offset(N + 0.5, K);
            return 2 * g[n + 0.5][k].dynamic_m
                    / (g[n][k - 0.5].r + g[n][k + 0.5].r);
        }        
        public double Get_M(LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.Appoint(N, K).Offset(N + 0.5, K);

            var tmp1 = g[n - 0.5][k].M;
            var tmp2 = wc.Nabla(PN.M, PN.w).Cell(n - 0.5, k);
            var tmp3 = (1 - g[n][k - 0.5].m) * bs.S(x[k - 0.5]);
            var tmp4 = (1 - g[n][k + 0.5].m) * bs.S(x[k + 0.5]);
            var tmp5 = ((1 - g[n][k - 0.5].m) * bs.S(x[k - 0.5]) + (1 - g[n][k + 0.5].m) * bs.S(x[k + 0.5])) / 2
                        * wc.dPStrokeDivdx().Cell(n, k);
            var tmp6 = wc.dPStrokeDivdx().Cell(n, k);
            var tmp7 = hf.H2(n, k);

            return g[n - 0.5][k].M - constP.tau *
                (
                    wc.Nabla(PN.M, PN.w).Cell(n - 0.5, k)
                   + ((1 - g[n][k - 0.5].m) * bs.S(x[k - 0.5]) + (1 - g[n][k + 0.5].m) * bs.S(x[k + 0.5])) / 2 
                        * wc.dPStrokeDivdx().Cell(n, k)
                   - hf.H2(n, k)
                );
        }         
        public double Get_w(LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.Appoint(N, K).Offset(N + 0.5, K);

            var tmp1 = bs.S(x[k - 0.5]);

            return 2 * g[n + 0.5][k].M
                    / (constP.PowderDelta * (
                                      (1 - g[n][k - 0.5].m) * bs.S(x[k - 0.5]) 
                                    + (1 - g[n][k + 0.5].m) * bs.S(x[k + 0.5]) 
                                      )
                      );
        }     


        public double Get_r(LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.Appoint(N, K).Offset(N + 1, K - 0.5);
            return g[n][k - 0.5].r - constP.tau *
                (
                    wc.Nabla(PN.r, PN.v).Cell(n + 0.5, k - 0.5)
                   - hf.H3(n + 0.5, k - 0.5)
                ) ;
        }        
        public double Get_e(LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.Appoint(N, K).Offset(N + 1, K - 0.5);
            return g[n][k - 0.5].e - constP.tau *
                (
                    wc.Nabla(PN.e, PN.v).Cell(n + 0.5, k - 0.5)
                   //+ (g[n + 0.5][k - 0.5].p + q(n + 0.5, k - 0.5))
                   + (g[n][k - 0.5].p + q(n + 0.5, k - 0.5))
                        * (wc.Nabla(PN.m, PN.S, PN.v).Cell(n + 0.5, k - 0.5) + wc.Nabla(PN.One_minus_m, PN.S, PN.w).Cell(n + 0.5, k - 0.5))
                   - hf.H4(n + 0.5, k - 0.5)
                );
        }
        public double Get_psi(LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.Appoint(N, K).Offset(N + 1, K - 0.5);
            var psi = g[n][k - 0.5].psi - constP.tau *
                (
                    wc.Nabla(PN.psi, PN.w).Cell(n + 0.5, k - 0.5)
                   - g[n][k - 0.5].psi * wc.Nabla(PN.w).Cell(n + 0.5, k - 0.5)
                   - hf.HPsi(n + 0.5, k - 0.5)
                );
            psi = PowderValidation(psi);
            return psi;
        }
        private static double PowderValidation(double value) // метод скопирован
        {
            if (value > 1)
                value = 1;
            return value;
        }
        public double Get_z(LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.Appoint(N, K).Offset(N + 1, K - 0.5);

            var tmp1 = g[n][k - 0.5].z;
            var tmp3 = wc.Nabla(PN.z, PN.w).Cell(n + 0.5, k - 0.5);
            var tmp5 = wc.Nabla(PN.w).Cell(n + 0.5, k - 0.5);
            var tmp6 = hf.H5(n + 0.5, k - 0.5);
            var z = g[n][k - 0.5].z - constP.tau *
                (
                    wc.Nabla(PN.z, PN.w).Cell(n + 0.5, k - 0.5)
                   - g[n][k - 0.5].z * wc.Nabla(PN.w).Cell(n + 0.5, k - 0.5)
                   - hf.H5(n + 0.5, k - 0.5)
                );

            z = PowderValidation(z);
            return z;
        }   
        public double Get_a(LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.Appoint(N, K).Offset(N + 1, K - 0.5);
            return g[n][k - 0.5].a - constP.tau *
                (
                    wc.Nabla(PN.a, PN.S, PN.w).Cell(n + 0.5, k - 0.5) 
                        / bs.S(x[k - 0.5])
                );
        }
        public double Get_p(LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.Appoint(N, K).Offset(N + 1, K - 0.5);
            return constP.teta * g[n + 1][k - 0.5].e
                    / (g[n + 1][k - 0.5].m * bs.S(x[k - 0.5]) - constP.alpha * g[n + 1][k - 0.5].r);
        }
        public double Get_m(LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.Appoint(N, K).Offset(N + 1, K - 0.5);
            return 1 - g[n + 1][k - 0.5].a * powder.LAMDA0 * (1 - g[n + 1][k - 0.5].psi);
        }







        public double Get_ro(LimitedDouble n, LimitedDouble k)
        {
            return g[n][k].r / (g[n][k].m * bs.S(x[k]));
        }





        private double q(LimitedDouble n, LimitedDouble k)
        {
            return PseudoViscosityMechanism.q(g, wc, constP, n, k);
        }
    }
}

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
using System.Diagnostics;
using System.Data;

namespace NIRS.Functions_for_numerical_method
{
    class FunctionsParametersOfTheNextLayer : IFunctionsParametersOfTheNextLayer
    {
        private IGrid g;
        private readonly IConstParameters constP;
        private IWaypointCalculator wc;
        private readonly IBarrelSize bs;
        private IHFunctions hf;
        private readonly IPowder powder;
        private double tau;

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
            tau = mainData.ConstParameters.tau;
        }

        public double Get(PN pN, double n, double k)
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


        public double Get_dynamic_m(double n, double k)
        {
            //(var n, var k) = OffseterNK.Appoint(N, K).Offset(N + 0.5, K);
            (n, k) = OffseterNK.AppointAndOffset(n, +0.5, k, 0);
            Stopwatch stopwatch = new Stopwatch();

            return g[PN.dynamic_m, n - 0.5, k] - tau *
                (
                    wc.Nabla(PN.dynamic_m, PN.v, n - 0.5, k)
                   + (g[PN.m, n, k - 0.5] * bs.S(x[k - 0.5]) + g[PN.m, n, k + 0.5] * bs.S(x[k + 0.5])) / 2
                        * wc.dPStrokeDivdx(n, k)
                   - hf.H1(n, k)
                );
        }
        public double Get_v(double n, double k)
        {
            //(var n, var k) = OffseterNK.Appoint(N, K).Offset(N + 0.5, K);
            (n, k) = OffseterNK.AppointAndOffset(n ,0.5, k ,0);

            return 2 * g[PN.dynamic_m, n + 0.5, k]
                    / (g[PN.r, n, k - 0.5] + g[PN.r, n, k + 0.5]);
        }        
        public double Get_M(double n, double k)
        {
            (n, k) = OffseterNK.AppointAndOffset(n ,0.5, k ,0);

            return g[PN.M, n - 0.5, k] - tau *
                (
                    wc.Nabla(PN.M, PN.w, n - 0.5, k)
                   + ((1 - g[PN.m, n, k - 0.5]) * bs.S(x[k - 0.5]) + (1 - g[PN.m, n, k + 0.5]) * bs.S(x[k + 0.5])) / 2 
                        * wc.dPStrokeDivdx(n, k)
                   - hf.H2(n, k)
                );
        }         
        public double Get_w(double n, double k)
        {
            (n, k) = OffseterNK.AppointAndOffset(n, 0.5, k, 0);

            if (g[PN.m, n, k - 0.5] == 1 && g[PN.m, n, k + 0.5] == 1)
                return 0;

            return 2 * g[PN.M, n + 0.5, k]
                   / (constP.PowderDelta * (
                                     (1 - g[PN.m, n, k - 0.5]) * bs.S(x[k - 0.5])
                                   + (1 - g[PN.m, n, k + 0.5]) * bs.S(x[k + 0.5])
                                           )
                     );
        }     


        public double Get_r(double n, double k)
        {
            (n, k) = OffseterNK.AppointAndOffset(n, 1, k, -0.5);
            return g[PN.r, n, k - 0.5] - tau *
                (
                    wc.Nabla(PN.r, PN.v, n + 0.5, k - 0.5)
                   - hf.H3(n + 0.5, k - 0.5)
                ) ;
        }        
        public double Get_e(double n, double k)
        {
            (n, k) = OffseterNK.AppointAndOffset(n, 1, k, -0.5);

            return g[PN.e, n, k - 0.5] - tau *
                (
                    wc.Nabla(PN.e, PN.v, n + 0.5, k - 0.5)
                   + (g[PN.p, n, k - 0.5] + q(n + 0.5, k - 0.5))
                        * (wc.Nabla(PN.m, PN.S, PN.v, n + 0.5, k - 0.5) + wc.Nabla(PN.One_minus_m, PN.S, PN.w, n + 0.5, k - 0.5))
                   - hf.H4(n + 0.5, k - 0.5)
                );
        }
        public double Get_psi(double n, double k)
        {
            (n, k) = OffseterNK.AppointAndOffset(n, 1, k, -0.5);
            var psi = g[PN.psi, n, k - 0.5] - tau *
                (
                    wc.Nabla(PN.psi, PN.w, n + 0.5, k - 0.5)
                   - g[PN.psi, n, k - 0.5] * wc.Nabla(PN.w, n + 0.5, k - 0.5)
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
        public double Get_z(double n, double k)
        {
            (n, k) = OffseterNK.AppointAndOffset(n, 1, k, -0.5);

            var z = g[PN.z, n, k - 0.5] - tau *
                (
                    wc.Nabla(PN.z, PN.w, n + 0.5, k - 0.5)
                   - g[PN.z, n, k - 0.5] * wc.Nabla(PN.w, n + 0.5, k - 0.5)
                   - hf.H5(n + 0.5, k - 0.5)
                );

            z = PowderValidation(z);
            return z;
        }   
        public double Get_a(double n, double k)
        {
            (n, k) = OffseterNK.AppointAndOffset(n, 1, k, -0.5);
            return g[PN.a, n, k - 0.5] - tau *
                (
                    wc.Nabla(PN.a, PN.S, PN.w, n + 0.5, k - 0.5)
                        / bs.S(x[k - 0.5])
                );
        }
        public double Get_p(double n, double k)
        {
            (n, k) = OffseterNK.AppointAndOffset(n, 1, k, -0.5);
            return constP.teta * g[PN.e, n + 1, k - 0.5]
                    / (g[PN.m, n + 1, k - 0.5] * bs.S(x[k - 0.5]) - constP.alpha * g[PN.r, n + 1, k - 0.5]);
        }
        public double Get_m(double n, double k)
        {
            (n, k) = OffseterNK.AppointAndOffset(n, 1, k, -0.5);
            return 1 - g[PN.a, n + 1, k - 0.5] * powder.LAMDA0 * (1 - g[PN.psi, n + 1, k - 0.5]);
        }







        public double Get_ro(double n, double k)
        {
            return g[PN.r, n, k] / (g[PN.m, n, k] * bs.S(x[k]));
        }





        private double q(double n, double k)
        {
            return PseudoViscosityMechanism.q(g, wc, constP, n, k);
        }

        public void Update(IGrid grid)
        {
            g = grid;
            wc.Update(grid);
            hf.Update(grid);
        }
    }
}

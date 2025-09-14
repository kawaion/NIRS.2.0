using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Grid_Folder;
using NIRS.Nabla_Functions;
using NIRS.Parameter_names;
using MyDouble;
using System;
using NIRS.Interfaces;
using NIRS.Helpers;

namespace NIRS.Additional_calculated_values
{
    static class PseudoViscosityMechanism
    {
        public static double q(IGrid g, IWaypointCalculator wc, IConstParameters constP, double N, double K)
        {
            (var n, var k) = OffseterNK.AppointAndOffset(N, + 0.5, K, - 0.5);

            double NablaV = wc.Nabla(PN.v, n + 0.5, k - 0.5);
            if (NablaV < 0)
            {
                var res = Math.Pow(constP.mu0, 2) * Math.Pow(constP.h, 2) * g[PN.rho, n + 1, k - 0.5] * Math.Pow(NablaV, 2);
                if(double.IsInfinity(res))
                {
                    var tmp1 = Math.Pow(constP.mu0, 2);
                    var tmp2 = g[PN.rho, n + 1, k - 0.5];
                    var tmp3 = Math.Pow(NablaV, 2);
                }
                return res;
            }
            else
                return 0;
        }
    }
}

using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Grid_Folder;
using NIRS.Nabla_Functions;
using NIRS.Parameter_names;
using MyDouble;
using System;
using NIRS.Interfaces;

namespace NIRS.Additional_calculated_values
{
    static class PseudoViscosityMechanism
    {
        public static double q(IGrid g, IWaypointCalculator wc, IConstParameters constP, LimitedDouble n, LimitedDouble k)
        {
            double NablaV = wc.Nabla(PN.v, n, k);
            if (NablaV < 0)
            {
                return (double)
                    (Math.Pow(constP.mu0, 2) * Math.Pow(constP.h, 2) * g[n + 0.5][k].ro * Math.Pow(NablaV, 2));
            }
            else
                return 0;
        }
    }
}

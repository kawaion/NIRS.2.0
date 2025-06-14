using MyDouble;
using NIRS.Grid_Folder;
using NIRS.Helpers;
using NIRS.Nabla_Functions;
using NIRS.Data_Parameters.Input_Data_Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NIRS.Interfaces;
using NIRS.Parameter_names;

namespace NIRS.Additional_calculated_values
{
    public static class BlurryP
    {
        public static double PStroke(this IGrid g,IWaypointCalculator waypointCalculator,IConstParameters constP, double N, double K)
        {
            (var n, var k) = OffseterNK.AppointAndOffset(N, 0, K, + 0.5);

            var res = g[PN.p, n, k + 0.5] + PseudoViscosityMechanism.q(g, waypointCalculator, constP, n - 0.5, k + 0.5);
            if(double.IsInfinity(res) )
            {
                var tmp1 = g[PN.p, n, k + 0.5];
                var tmp2 = PseudoViscosityMechanism.q(g, waypointCalculator, constP, n - 0.5, k + 0.5);
            }
            return res;
        }
    }
}

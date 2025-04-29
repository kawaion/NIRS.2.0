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

namespace NIRS.Additional_calculated_values
{
    public static class BlurryP
    {
        public static double PStroke(this IGrid g,IWaypointCalculator waypointCalculator,IConstParameters constP, LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.Appoint(N, K).Offset(N, K + 0.5);

            return g[n][k + 0.5].p + PseudoViscosityMechanism.q(g, waypointCalculator, constP, n - 0.5, k + 0.5);
        }
    }
}

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
using NIRS.Grid_Folder.Mediator;

namespace NIRS.Additional_calculated_values
{
    public static class BlurryP
    {
        public static double PStroke(this IGrid g,IWaypointCalculator waypointCalculator,IConstParameters constP, LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n, k + 0.5);

            return g.p(n, k + 0.5) + PseudoViscosityMechanism.q(g, waypointCalculator, constP, n, k);
        }
    }
}

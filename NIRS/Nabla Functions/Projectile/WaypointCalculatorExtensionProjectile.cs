using NIRS.Nabla_Functions.Close;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Nabla_Functions.Projectile
{
    public static class WaypointCalculatorExtensionProjectile
    {
        public static NablaNodeProjectile Nabla(this IWaypointCalculatorProjectile waypointCalculator, PN param1, PN param2, PN param3)
        {
            List<PN> list = new List<PN>();
            list.Add(param1);
            list.Add(param2);
            list.Add(param3);
            return new NablaNodeProjectile(list, NablaFunctionType.Nabla3, waypointCalculator);
        }
    }
}

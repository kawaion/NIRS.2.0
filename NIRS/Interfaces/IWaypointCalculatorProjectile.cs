using MyDouble;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Nabla_Functions.Projectile
{
    public interface IWaypointCalculatorProjectile
    {
        double Nabla(PN param1, PN param2, PN param3, double n);
    }
}

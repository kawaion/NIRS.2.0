using MyDouble;
using NIRS.Parameter_names;
using NIRS.Projectile_Folder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Nabla_Functions.Projectile
{
    interface IWaypointCalculatorProjectile
    {
        double Nabla(PN param1, PN param2, PN param3, LimitedDouble n, Pos pos);
        double Nabla(PN param1, PN param2, LimitedDouble n, Pos pos);
        double Nabla(PN param, LimitedDouble n, Pos pos);
        double dPStrokeDivdx(LimitedDouble n, Pos pos);
    }
}

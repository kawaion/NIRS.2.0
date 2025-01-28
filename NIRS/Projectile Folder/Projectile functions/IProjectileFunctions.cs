using MyDouble;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Projectile_Folder.Projectile_functions
{
    interface IProjectileFunctions
    {
        double Culc_v(LimitedDouble n);
        double Culc_x(LimitedDouble n);
        double Culc_r(LimitedDouble n);
        double Culc_z(LimitedDouble n);
        double Culc_psi(LimitedDouble n);
        double Culc_a(LimitedDouble n);
        double Culc_m(LimitedDouble n);
        double Culc_ro(LimitedDouble n);
        double Culc_e(LimitedDouble n);
        double Culc_p(LimitedDouble n);
    }
}

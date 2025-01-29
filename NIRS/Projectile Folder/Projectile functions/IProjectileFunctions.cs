using MyDouble;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Projectile_Folder.Projectile_functions
{
    interface IProjectileFunctions
    {
        double Get(PN pn,LimitedDouble n, Pos pos);
        double Get_v(LimitedDouble n, Pos pos);
        double Get_x(LimitedDouble n, Pos pos);
        double Get_r(LimitedDouble n, Pos pos);
        double Get_z(LimitedDouble n, Pos pos);
        double Get_psi(LimitedDouble n, Pos pos);
        double Get_a(LimitedDouble n, Pos pos);
        double Get_m(LimitedDouble n, Pos pos);
        double Get_ro(LimitedDouble n, Pos pos);
        double Get_e(LimitedDouble n, Pos pos);
        double Get_p(LimitedDouble n, Pos pos);
    }
}

using MyDouble;
using NIRS.Cannon_Folder.Barrel_Folder;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Grid_Folder;
using NIRS.Grid_Folder.Mediator;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Projectile_Folder.Projectile_functions
{
    class ProjectileFunctions : IProjectileFunctions
    {
        private readonly IGrid g;
        private readonly IProjectile sn;
        private readonly IConstParameters constP;

        public ProjectileFunctions(IGrid grid, IProjectile projectile, IConstParameters constParameters)
        {
            g = grid;
            sn = projectile;
            constP = constParameters;
        }
        public double Get(PN pn, LimitedDouble n, Pos pos)
        {
        }

        public double Get_a(LimitedDouble n, Pos pos)
        {
            return g.a(n, Pos.sn) *
                (
                    1 - constP.tau *
                    (
                        (g.v_sn(n + 0.5, Pos.sn) - g.v_sn(n + 0.5, Pos.KplusOne)) /
                        (g.x(n + 0.5, Pos.sn) - g.x(n + 0.5, Pos.KplusOne))
                    )
                );
        }

        public double Get_e(LimitedDouble n, Pos pos)
        {
            return g.e(n, Pos.sn) - constP.tau *
                (
                    g.e(n, Pos.sn) * (g.v_sn(n + 0.5, Pos.sn) - g.v_sn(n + 0.5, Pos.KplusOne)) /
                                     (g.x(n + 0.5, Pos.sn) - g.x(n + 0.5, Pos.KplusOne))
                    + g.p(n, Pos.sn) * ()
                );
                   
        }

        public double Get_m(LimitedDouble n, Pos pos)
        {
            throw new NotImplementedException();
        }

        public double Get_p(LimitedDouble n, Pos pos)
        {
            throw new NotImplementedException();
        }

        public double Get_psi(LimitedDouble n, Pos pos)
        {
            throw new NotImplementedException();
        }

        public double Get_r(LimitedDouble n, Pos pos)
        {
            throw new NotImplementedException();
        }

        public double Get_ro(LimitedDouble n, Pos pos)
        {
            throw new NotImplementedException();
        }

        public double Get_v(LimitedDouble n, Pos pos)
        {
            throw new NotImplementedException();
        }

        public double Get_x(LimitedDouble n, Pos pos)
        {
            throw new NotImplementedException();
        }

        public double Get_z(LimitedDouble n, Pos pos)
        {
            throw new NotImplementedException();
        }
    }
}

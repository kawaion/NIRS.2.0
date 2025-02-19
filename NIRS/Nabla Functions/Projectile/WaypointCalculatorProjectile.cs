using MyDouble;
using NIRS.Cannon_Folder.Barrel_Folder;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Grid_Folder;
using NIRS.Helpers;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Nabla_Functions.Projectile
{
    class WaypointCalculatorProjectile : IWaypointCalculatorProjectile
    {
        private readonly IGrid g;
        private readonly IConstParameters constP;
        private readonly IBarrelSize bs;
        private readonly GetterValueByPN gByPN;

        private readonly XGetter x;

        public WaypointCalculatorProjectile(IGrid grid, IMainData mainData)
        {
            g = grid;
            constP = mainData.ConstParameters;
            bs = mainData.BarrelSize;
            gByPN = new GetterValueByPN(grid);

            x = new XGetter(mainData.ConstParameters);
        }
        public double Nabla(PN param1, PN param2, PN param3, LimitedDouble n)
        {
            n = OffseterN.Appoint(n).Offset(n + 0.5);

            return (AverageProjectile(param1, param2, param3, n + 0.5) - AverageK(param1, param2, param3, n + 0.5)) 
                   / constP.h;
        }
        private double AverageProjectile(PN mu, PN S, PN v, LimitedDouble n)
        {
            n = OffseterN.Appoint(n).Offset(n + 0.5);

            return gByPN.GetParamCellSn(v, n + 0.5) * gByPN.GetParamCellSn(mu, n) * bs.S(g[n].sn.x);
        }

        private double AverageK(PN mu, PN S, PN v, LimitedDouble n)
        {
            n = OffseterN.Appoint(n).Offset(n + 0.5);

            var K = g[n].LastIndex();

            double V = gByPN.GetParamCell(v, n + 0.5, K);
            if (V >= 0)
                return V * gByPN.GetParamCell(mu, n, K - 0.5) * bs.S(x[K-0.5]);
            else
                return V * gByPN.GetParamCellSn(mu, n) * bs.S(g[n].sn.x);
        }

    }
}

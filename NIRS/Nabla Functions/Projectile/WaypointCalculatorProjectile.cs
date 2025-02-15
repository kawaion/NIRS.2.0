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

        private readonly double XSn;

        public WaypointCalculatorProjectile(IGrid grid, IConstParameters constParameters, IBarrelSize barrelSize, double xSn)
        {
            g = grid;
            constP = constParameters;
            bs = barrelSize;
            gByPN = new GetterValueByPN(grid);

            XSn = xSn;
        }
        public double Nabla(PN param1, PN param2, PN param3, LimitedDouble n)
        {
            LimitedDouble uselessValue = new LimitedDouble(0);
            (n, _) = OffseterNK.Appoint(n, uselessValue).Offset(n + 0.5, uselessValue);

            return (AverageWithS(param1, param3, n + 0.5) - AverageWithS(param1, param3, n + 0.5)) / constP.h;
        }
        private double AverageWithS(PN mu, PN v, LimitedDouble n)
        {
            XGetter x = new XGetter(constP);
            // формула преобразуется в значение на n,k
            double V = gByPN.GetParamCell(v, n);
            if (V >= 0)
                return V * gByPN.GetParamCell(mu, n - 0.5) * bs.S(g[n].sn.P.x);
            else
                return V * gByPN.GetParamCell(mu, n - 0.5) * bs.S(x[k]);
        }

    }
}

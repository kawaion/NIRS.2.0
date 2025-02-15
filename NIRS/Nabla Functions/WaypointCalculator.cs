using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NIRS.Grid_Folder;
using MyDouble;
using NIRS.Parameter_names;
using NIRS.Parameter_Type;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Cannon_Folder.Barrel_Folder;
using NIRS.Helpers;
using NIRS.Grid_Folder.Mediator;
using NIRS.Additional_calculated_values;
using NIRS.Interfaces;

namespace NIRS.Nabla_Functions
{
    class WaypointCalculator : IWaypointCalculator
    {
        private readonly IGrid g;
        private readonly IConstParameters constP;
        private readonly IBarrelSize bs;

        private readonly GetterValueByPN gByPN;
        private readonly XGetter x;
        public WaypointCalculator(IGrid grid, IBarrelSize barrelSize, IMainData mainData)
        {
            g = grid;
            constP = mainData.ConstParameters;
            bs = barrelSize;

            gByPN = new GetterValueByPN(grid);
            x = new XGetter(mainData.ConstParameters);
        }


        public double Nabla(PN param1, PN param2, PN param3, LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 0.5, k - 0.5);

            return (AverageWithS(param1, param3, n + 0.5, k) - AverageWithS(param1, param3, n + 0.5, k - 1)) / constP.h;
        }
        private double AverageWithS(PN mu, PN v, LimitedDouble n, LimitedDouble k)
        {
            // формула преобразуется в значение на n,k
            double V = gByPN.GetParamCell(v, n, k);
            if (V >= 0)
                return V * gByPN.GetParamCell(mu, n - 0.5, k - 0.5) * bs.S(x[k - 0.5]);
            else
                return V * gByPN.GetParamCell(mu, n - 0.5, k + 0.5) * bs.S(x[k - 0.5]);
        }


        public double Nabla(PN param1, PN v, LimitedDouble n, LimitedDouble k)
        {
            if (param1.IsDynamic())
            {
                (n, k) = OffseterNK.Appoint(n, k).Offset(n - 0.5, k);

                return (DynamicAverage(param1, v, n - 0.5, k + 0.5) - DynamicAverage(param1, v, n - 0.5, k - 0.5)) / constP.h;
            }
                
            if (param1.IsMixture())
            {
                (n, k) = OffseterNK.Appoint(n, k).Offset(n + 0.5, k - 0.5);

                return (MixtureAverage(param1, v, n + 0.5, k ) - MixtureAverage(param1, v, n + 0.5, k - 1)) / constP.h;
            }
                
            throw new Exception($"неизвестные параметры {param1} и {v} на слое {n} {k}");
        }
        private double DynamicAverage(PN mu, PN v, LimitedDouble n, LimitedDouble k)
        {
            // формула преобразуется в значение на n,k
            double sum_v = gByPN.GetParamCell(v, n, k - 0.5) + gByPN.GetParamCell(v, n, k + 0.5);
            if (sum_v >= 0)
                return sum_v / 2 * gByPN.GetParamCell(mu, n, k - 0.5);
            else
                return sum_v / 2 * gByPN.GetParamCell(mu, n, k + 0.5);
        }
        private double MixtureAverage(PN fi, PN V, LimitedDouble n, LimitedDouble k)
        {
            // формула преобразуется в значение на n,k
            double v = gByPN.GetParamCell(V, n, k);
            if (v >= 0)
                return v * gByPN.GetParamCell(fi, n - 0.5, k - 0.5);
            else
                return v * gByPN.GetParamCell(fi, n - 0.5, k + 0.5);
        }


        public double Nabla(PN v, LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 0.5, k - 0.5);

            return (gByPN.GetParamCell(v, n + 0.5, k) - gByPN.GetParamCell(v, n + 0.5, k - 1)) / constP.h;
        }


        public double dPStrokeDivdx(LimitedDouble n, LimitedDouble k)
        {
            return (g.PStroke(this, constP, n, k + 0.5) - g.PStroke(this, constP, n, k - 0.5)) / constP.h;
        }
    }
}

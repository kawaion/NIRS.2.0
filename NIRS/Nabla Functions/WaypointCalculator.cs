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
using NIRS.Additional_calculated_values;
using NIRS.Interfaces;
using NIRS.Nabla_Functions.Projectile;

namespace NIRS.Nabla_Functions
{
    public class WaypointCalculator : IWaypointCalculator
    {
        private readonly IGrid g;
        private readonly IConstParameters constP;
        private readonly IBarrelSize bs;

        private readonly XGetter x;
        public WaypointCalculator(IGrid grid, IMainData mainData)
        {
            g = grid;
            constP = mainData.ConstParameters;
            bs = mainData.Barrel.BarrelSize;

            x = new XGetter(mainData.ConstParameters);

            sn = new WaypointCalculatorProjectile(g, mainData);
        }


        public double Nabla(PN param1, PN param2, PN param3, LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.Appoint(N, K).Offset(N + 0.5, K - 0.5);

            return (AverageWithS(param1, param3, n + 0.5, k) - AverageWithS(param1, param3, n + 0.5, k - 1)) / constP.h;
        }
        private double AverageWithS(PN mu, PN v, LimitedDouble n, LimitedDouble k)
        {
            // формула преобразуется в значение на n,k
            double V = g[n][k][v];

            if (V >= 0)
                return V * Get_m(n - 0.5, k - 0.5, mu) * bs.S(x[k - 0.5]);
            else
                return V * Get_m(n - 0.5, k + 0.5, mu) * bs.S(x[k + 0.5]);
        }
        private double Get_m(LimitedDouble n, LimitedDouble k, PN mu)
        {
            if (mu == PN.One_minus_m)
                return 1 - g[n][k][mu];
            else
                return g[n][k][mu];

        }


        public double Nabla(PN param1, PN v, LimitedDouble N, LimitedDouble K)
        {
            if (param1.IsDynamic())
            {
                (var n, var k) = OffseterNK.Appoint(N, K).Offset(N - 0.5, K);

                return (DynamicAverage(param1, v, n - 0.5, k + 0.5, NablaType.plus) - DynamicAverage(param1, v, n - 0.5, k - 0.5, NablaType.minus)) / constP.h;
            }

            if (param1.IsMixture())
            {
                (var n, var k) = OffseterNK.Appoint(N, K).Offset(N + 0.5, K - 0.5);

                return (MixtureAverage(param1, v, n + 0.5, k, NablaType.plus) - MixtureAverage(param1, v, n + 0.5, k - 1, NablaType.minus)) / constP.h;
            }

            throw new Exception($"неизвестные параметры {param1} и {v} на слое {N} {K}");
        }
        private double DynamicAverage(PN mu, PN v, LimitedDouble N, LimitedDouble K, NablaType type)
        {
            if (type == NablaType.plus)
            {
                (var n, var k) = OffseterNK.Appoint(N, K).Offset(N - 0.5, K + 0.5);

                double sum_v = g[n - 0.5][k][v] + g[n - 0.5][k + 1][v];
                if (sum_v >= 0)
                    return sum_v / 2 * g[n - 0.5][k][mu];
                else
                    return sum_v / 2 * g[n - 0.5][k + 1][mu];
            }
            if (type == NablaType.minus)
            {
                (var n, var k) = OffseterNK.Appoint(N, K).Offset(N - 0.5, K - 0.5);

                double sum_v = g[n - 0.5][k - 1][v] + g[n - 0.5][k][v];
                if (sum_v >= 0)
                    return sum_v / 2 * g[n - 0.5][k - 1][mu];
                else
                    return sum_v / 2 * g[n - 0.5][k][mu];
            }
            throw new Exception();
        }
        private double MixtureAverage(PN fi, PN V, LimitedDouble N, LimitedDouble K, NablaType type)
        {
            if (type == NablaType.plus)
            {
                (var n, var k) = OffseterNK.Appoint(N, K).Offset(N + 0.5, K);

                double v = g[n + 0.5][k][V];
                if (v >= 0)
                    return v * g[n][k - 0.5][fi];
                else
                    return v * g[n][k + 0.5][fi];
            }
            if (type == NablaType.minus)
            {
                (var n, var k) = OffseterNK.Appoint(N, K).Offset(N + 0.5, K - 1);

                double v = g[n + 0.5][k - 1][V];
                if (v >= 0)
                    return v * g[n][k - 1.5][fi];
                else
                    return v * g[n][k - 0.5][fi];
            }
            throw new Exception();
        }


        public double Nabla(PN v, LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.Appoint(N, K).Offset(N + 0.5, K - 0.5);

            return (g[n + 0.5][k][v] - g[n + 0.5][k - 1][v]) / constP.h;
        }


        public double dPStrokeDivdx(LimitedDouble n, LimitedDouble k)
        {
            var tmp1 = g.PStroke(this, constP, n, k + 0.5);
            var tmp2 = g.PStroke(this, constP, n, k - 0.5);

            var tmpres = (g.PStroke(this, constP, n, k + 0.5) - g.PStroke(this, constP, n, k - 0.5)) / constP.h;
            //return (g.PStroke(this, constP, n, k + 0.5) - g.PStroke(this, constP, n, k - 0.5)) / constP.h;
            if (double.IsInfinity(tmpres))
            {
                int c = 0;
            }
            return tmpres;
        }

        enum NablaType
        {
            plus,
            minus
        }
        public IWaypointCalculatorProjectile sn { get; set; }
    }

}

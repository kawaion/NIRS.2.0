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
using System.Diagnostics;

namespace NIRS.Nabla_Functions
{
    public class WaypointCalculator : IWaypointCalculator
    {
        private IGrid g;
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


        public double Nabla(PN param1, PN param2, PN param3, double N, double K)
        {
            (var n, var k) = OffseterNK.AppointAndOffset(N, + 0.5, K, - 0.5);

            return (AverageWithS(param1, param3, n + 0.5, k) - AverageWithS(param1, param3, n + 0.5, k - 1)) / constP.h;
        }
        private double AverageWithS(PN mu, PN v, double n, double k)
        {
            // формула преобразуется в значение на n,k
            double V = g[v, n, k];

            if (V >= 0)
                return V * Get_m(n - 0.5, k - 0.5, mu) * bs.S(x[k - 0.5]);
            else
                return V * Get_m(n - 0.5, k + 0.5, mu) * bs.S(x[k + 0.5]);
        }
        private double Get_m(double n, double k, PN mu)
        {
             return g[mu, n, k];
        }


        public double Nabla(PN param1, PN v, double N, double K)
        {
            if (param1.IsDynamic())
            {
                (var n, var k) = OffseterNK.AppointAndOffset(N, -0.5, K, 0);

                return (DynamicAverage(param1, v, n - 0.5, k + 0.5, NablaType.plus) - DynamicAverage(param1, v, n - 0.5, k - 0.5, NablaType.minus)) / constP.h;
            }

            if (param1.IsMixture())
            {
                (var n, var k) = OffseterNK.AppointAndOffset(N, + 0.5, K, - 0.5);

                return (MixtureAverage(param1, v, n + 0.5, k , NablaType.plus) - MixtureAverage(param1, v, n + 0.5, k - 1, NablaType.minus)) / constP.h;
            }

            throw new Exception($"неизвестные параметры {param1} и {v} на слое {N} {K}");
        }
        private double DynamicAverage(PN mu, PN v, double N, double K, NablaType type)
        {
            if (type == NablaType.plus)
            {
                (var n, var k) = OffseterNK.AppointAndOffset(N, - 0.5, K, + 0.5);

                double sum_v = g[v, n - 0.5, k] + g[v, n - 0.5, k + 1];
                if (sum_v >= 0)
                    return sum_v / 2 * g[mu, n - 0.5, k];
                else
                    return sum_v / 2 * g[mu, n - 0.5, k + 1];
            }
            if (type == NablaType.minus)
            {
                (var n, var k) = OffseterNK.AppointAndOffset(N, - 0.5, K, - 0.5);

                double sum_v = g[v, n - 0.5, k - 1] + g[v, n - 0.5, k];
                if (sum_v >= 0)
                    return sum_v / 2 * g[mu, n - 0.5, k - 1];
                else
                    return sum_v / 2 * g[mu, n - 0.5, k];
            }
            throw new Exception();
        }
        private double MixtureAverage(PN fi, PN V, double N, double K, NablaType type)
        {
            if (type == NablaType.plus)
            {
                (var n, var k) = OffseterNK.AppointAndOffset(N, + 0.5, K, 0);

                double v = g[V, n + 0.5, k];

                if (v >= 0)
                {
                    if (v == 0) return 0;
                    return v * g[fi, n, k - 0.5];
                }

                else
                    return v * g[fi, n, k + 0.5];

            }
            if (type == NablaType.minus)
            {
                (var n, var k) = OffseterNK.AppointAndOffset(N, +0.5, K, -1);

                double v = g[V, n + 0.5, k - 1];
                if (v >= 0)
                {
                    if (v == 0) return 0;
                    return v * g[fi, n, k - 1.5];
                }
                else
                    return v * g[fi, n, k - 0.5];
            }
            throw new Exception();
        }


        public double Nabla(PN v, double N, double K)
        {
            (var n, var k) = OffseterNK.AppointAndOffset(N, + 0.5, K, - 0.5);

            return (g[v, n + 0.5, k] - g[v, n + 0.5, k - 1]) / constP.h;
        }


        public double dPStrokeDivdx(double n, double k)
        {
            var res = (g.PStroke(this, constP, n, k + 0.5) - g.PStroke(this, constP, n, k - 0.5)) / constP.h;
            if (double.IsNaN(res))
            {
                var tmp1 = g.PStroke(this, constP, n, k + 0.5);
                var tmp2 = g.PStroke(this, constP, n, k - 0.5);
            }
            return res;
        }

        public void Update(IGrid grid)
        {
            g = grid;
        }

        enum NablaType
        {
            plus,
            minus
        }
        public IWaypointCalculatorProjectile sn { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NIRS.Grid_Folder;
using MyDouble;
using NIRS.Parameter_names;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Cannon_Folder.Barrel_Folder;
using NIRS.Helpers;
using NIRS.Grid_Folder.Mediator;

namespace NIRS.Nabla_Functions
{
    class WaypointCalculator : IWaypointCalculator
    {
        private readonly IGrid g;
        private readonly IConstParameters constP;
        private readonly IBarrelSize bs;

        public WaypointCalculator(IGrid grid, IConstParameters constParameters, IBarrelSize barrelSize)
        {
            g = grid;
            constP = constParameters;
            bs = barrelSize;
        }
        public double Nabla(PN param1, PN v, LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 0.5, k);
            if (n.Type == DoubleType.HalfInt && k.Type == DoubleType.Int)
            {
                (n, k) = OffseterNK.Appoint(n, k).Offset(n - 0.5, k);
                return (DynamicAverage(param1, v, n - 0.5, k + 0.5) - DynamicAverage(param1, v, n - 0.5, k - 0.5)) / constP.h;
            }
                
            if (n.Type == DoubleType.HalfInt && k.Type == DoubleType.HalfInt)
            {
                (n, k) = OffseterNK.Appoint(n, k).Offset(n + 0.5, k - 0.5);
                return (MixtureAverage(param1, v, n + 0.5, k ) - MixtureAverage(param1, v, n + 0.5, k - 1)) / constP.h;
            }
                

            throw new Exception($"неизвестные параметры {param1} и {v} на слое {n} {k}");
        }
        private double DynamicAverage(PN mu, PN v, LimitedDouble n, LimitedDouble k)
        {
            double sum_v = GetParamCell(v, n, k - 0.5) + GetParamCell(v, n, k + 0.5);
            if (sum_v >= 0)
                return sum_v / 2 * GetParamCell(mu, n, k - 0.5);
            else
                return sum_v / 2 * GetParamCell(mu, n, k + 0.5);
        }
        private double MixtureAverage(PN fi, PN V, LimitedDouble n, LimitedDouble k)
        {
            double v = GetParamCell(V, n, k);
            if (v >= 0)
                return GetParamCell(fi, n - 0.5, k - 0.5) * v;
            else
                return GetParamCell(fi, n - 0.5, k + 0.5) * v;
        }
        public double Nabla(PN v, LimitedDouble n, LimitedDouble k)
        {
            if (n.Type == DoubleType.HalfInt && k.Type == DoubleType.HalfInt)
                return (GetParamCell(v, n, k + 0.5) - GetParamCell(v, n, k - 0.5)) / constP.h;

            throw new Exception($"неизвестный параметр {v} на слое {n} {k}");
        }
        public double dDivdx(PN param, LimitedDouble n, LimitedDouble k)
        {
            if (n.Type == DoubleType.Int && k.Type == DoubleType.Int)
                return (GetParamCell(param, n, k + 0.5) - GetParamCell(param, n, k - 0.5)) / constP.h;

            throw new Exception($"неизвестный параметр {param} на слое {n} {k}");
        }
        private double GetParamCell(PN param, LimitedDouble n, LimitedDouble k)
        {
            if (_grid[n][k] is DynamicCharacteristicsFlowCell dynamicCell)
                return dynamicCell.GetValueByString(param);
            if (_grid[n][k] is MixtureStateParametersCell MixtureCell)
                return MixtureCell.GetValueByString(param);
            if (param == PN.S)
                return bs.SByIndex(k);
            if (param == PN.pStroke)
                return GetParamCell("p", n, k) + PseudoViscosityMechanism.q(g, this, constP, n, k);
            if (param == PN.One_minus_m)
                return 1 - g.m(n,k);
            throw new Exception($"неизвестное значение {param}");
        }
    }
}

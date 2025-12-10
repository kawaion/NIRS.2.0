using MyDouble;
using NIRS.Helpers;
using NIRS.Interfaces;
using NIRS.Numerical_Method;
using NIRS.Parameter_names;
using NIRS.Parameter_Type;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.For_chart
{
    class ResultExtractor
    {
        IGrid _grid;
        public ResultExtractor(IGrid grid)
        {
            _grid = grid;
        }
        public DataForChart GetPKn()
        {
            List<double> massive = new List<double>();
            LimitedDouble lastN = _grid.LastIndexN(PN.p);
            LimitedDouble firstN = FirstLimitedDouble.GetFirstN(PN.p);

            for (LimitedDouble n = firstN; n <= lastN; n++)
                massive.Add(_grid[PN.p, n, new LimitedDouble(0.5)] * 1e-6);
            string title = "давление, МПа";
            string lineName = "Pkn";

            return new DataForChart(massive, title, lineName);
        }
        public DataForChart GetPSn()
        {
            List<double> massive = new List<double>();
            LimitedDouble lastN = _grid.LastIndexNSn(PN.p);
            LimitedDouble firstN = FirstLimitedDouble.GetFirstN(PN.p);
            

            for (LimitedDouble n = firstN; n <= lastN; n++)
                massive.Add(_grid.GetSn(PN.p, n) * 1e-6);
            string title = "давление, МПа";
            string lineName = "Psn";
            return new DataForChart(massive, title, lineName);
        }
        public DataForChart GetP(LimitedDouble n)
        {
            List<double> massive = new List<double>();
            LimitedDouble lastK = _grid.LastIndexK(PN.p, n);
            LimitedDouble firstK = FirstLimitedDouble.GetFirstK(PN.p);

            for (LimitedDouble k = firstK; k <= lastK; k++)
                massive.Add(_grid[PN.p, n, k] * 1e-6);
            string title = "давление, МПа";
            string lineName = "P";
            return new DataForChart(massive, title, lineName);
        }
        public DataForChart GetVSn()
        {
            List<double> massive = new List<double>();
            LimitedDouble lastN = _grid.LastIndexNSn(PN.vSn);
            LimitedDouble firstN = FirstLimitedDouble.GetFirstN(PN.v);

            for (LimitedDouble n = firstN; n <= lastN; n++)
                massive.Add(_grid.GetSn(PN.vSn, n));
            string title = "скорость снаряда, м/с";
            string lineName = "Vsn";
            return new DataForChart(massive, title, lineName);
        }
        public DataForChart GetT(PN pn, IMainData mainData)
        {
            var tau = mainData.ConstParameters.tau;
            List<double> massive = new List<double>();
            LimitedDouble lastN = _grid.LastIndexN(pn);
            LimitedDouble firstN = FirstLimitedDouble.GetFirstN(pn);

            for (LimitedDouble n = firstN; n <= lastN; n++)
                massive.Add(n.GetDouble() * tau * 1e3);
            string title = "время, t";
            string lineName = "t";
            return new DataForChart(massive, title, lineName);
        }
        public DataForChart GetX(PN pn, LimitedDouble n, IMainData mainData)
        {
            var h = mainData.ConstParameters.h;
            List<double> massive = new List<double>();
            LimitedDouble lastK = _grid.LastIndexK(pn,n);
            LimitedDouble firstK = FirstLimitedDouble.GetFirstK(pn);

            for (LimitedDouble K = firstK; K <= lastK; K++)
                massive.Add(K.GetDouble() * h);
            string title = "длина, м";
            string lineName = "x";
            return new DataForChart(massive, title, lineName);
        }
        public DataForChart GetRo(LimitedDouble n)
        {
            List<double> massive = new List<double>();
            LimitedDouble lastK = _grid.LastIndexK(PN.rho, n);
            LimitedDouble firstK = FirstLimitedDouble.GetFirstK(PN.rho);

            for (LimitedDouble k = firstK; k <= lastK; k++)
                massive.Add(_grid[PN.rho, n, k]);
            string title = "плотность, кг/м³";
            string lineName = "ro";
            return new DataForChart(massive, title, lineName);
        }
        public DataForChart GetTemperature(LimitedDouble n, IMainData mainData)
        {
            var bs = mainData.Barrel.BarrelSize;
            var h = mainData.ConstParameters.h;
            var cv = mainData.ConstParameters.cv;
            var xGetter = new XGetter(mainData.ConstParameters);
            List<double> massive = new List<double>();
            LimitedDouble lastK = _grid.LastIndexK(PN.rho, n);
            LimitedDouble firstK = FirstLimitedDouble.GetFirstK(PN.rho);

            for (LimitedDouble k = firstK; k <= lastK; k++)
            {
                var eps = _grid[PN.e,n,k] / (_grid[PN.rho, n, k] * _grid[PN.m, n, k] * bs.S(xGetter[k]));
                massive.Add(eps/cv);
            }
            string title = "температура, K";
            string lineName = "T";
            return new DataForChart(massive, title, lineName);
        }
        public DataForChart GetV(LimitedDouble n)
        {
            List<double> massive = new List<double>();
            LimitedDouble lastK = _grid.LastIndexK(PN.v, n);
            LimitedDouble firstK = FirstLimitedDouble.GetFirstK(PN.v);

            for (LimitedDouble k = firstK; k <= lastK; k++)
                massive.Add(_grid[PN.v, n, k]);
            string title = "скорость газов, м/с";
            string lineName = "V";
            return new DataForChart(massive, title, lineName);
        }
        public DataForChart GetW(LimitedDouble n)
        {
            List<double> massive = new List<double>();
            LimitedDouble lastK = _grid.LastIndexK(PN.w, n);
            LimitedDouble firstK = FirstLimitedDouble.GetFirstK(PN.w);

            for (LimitedDouble k = firstK; k <= lastK; k++)
                massive.Add(_grid[PN.w, n, k]);
            string title = "скорость твердой фазы, м/с";
            string lineName = "W";
            return new DataForChart(massive, title, lineName);
        }
        public DataForChart GetA(LimitedDouble n, IMainData mainData)
        {
            var constP = mainData.ConstParameters;
            var powder = mainData.Powder;
            var bs = mainData.Barrel.BarrelSize;
            var h = mainData.ConstParameters.h;
            var cv = mainData.ConstParameters.cv;
            List<double> massive = new List<double>();
            LimitedDouble lastK = _grid.LastIndexK(PN.a, n);
            LimitedDouble firstK = FirstLimitedDouble.GetFirstK(PN.a);
            double a_n = powder.Omega / (powder.LAMBDA0 * powder.Delta * bs.Wkm);

            for (LimitedDouble k = firstK; k <= lastK; k++)
                massive.Add(_grid[PN.a, n, k] / a_n);
            string title = "относительная счетная концентрация, %";
            string lineName = "a";
            return new DataForChart(massive, title, lineName);
        }
        public DataForChart GetPsi(LimitedDouble n, IMainData mainData)
        {
            List<double> massive = new List<double>();
            LimitedDouble lastK = _grid.LastIndexK(PN.psi, n);
            LimitedDouble firstK = FirstLimitedDouble.GetFirstK(PN.psi);

            for (LimitedDouble k = firstK; k <= lastK; k++)
                massive.Add(_grid[PN.psi, n, k]);
            string title = "относительная доля сгоревшего пороха, %";
            string lineName = "psi";
            return new DataForChart(massive, title, lineName);
        }
        public DataForChart GetEpure(IMainData mainData)
        {
            List<double> massive = new List<double>();
            LimitedDouble lastN = _grid.LastIndexN(PN.p);
            LimitedDouble firstN = FirstLimitedDouble.GetFirstN(PN.p);

            LimitedDouble endOfChamberK = _grid.LastIndexK(PN.p, lastN);
            LimitedDouble bottomK = FirstLimitedDouble.GetFirstK(PN.p);
            LimitedDouble minNForCurrentK = firstN;

            for(LimitedDouble k = bottomK; k <= endOfChamberK; k++)
            {
                while (k > _grid.LastIndexK(PN.p, minNForCurrentK))
                    minNForCurrentK++;
                double maxP = double.MinValue;
                for(LimitedDouble n = minNForCurrentK; n <= lastN; n++)
                {
                    double currentP = _grid[PN.p, n, k];
                    if (maxP < currentP)
                        maxP = currentP;
                }
                massive.Add(maxP / 1e6);
            }
            string title = "эпюра максимальных давлений, МПа";
            string lineName = "Epur";
            return new DataForChart(massive, title, lineName);
        }
    }
    enum PT
    {
        dynamic,
        mixture
    }
}

using NIRS.Helpers;
using NIRS.Interfaces;
using NIRS.Numerical_Method;
using NIRS.Parameter_names;
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
            double lastN = _grid.LastIndexN(PN.p);
            double firstN = GetFirstNode(lastN);

            for (double n = firstN; n <= lastN; n++)
                massive.Add(_grid[PN.p, n, 0.5] * 1e-6);
            string title = "давление, МПа";
            string lineName = "Pkn";

            return new DataForChart(massive, title, lineName);
        }
        public DataForChart GetPSn()
        {
            List<double> massive = new List<double>();
            double lastN = _grid.LastIndexNSn(PN.p);
            double firstN = GetFirstNode(lastN);
            

            for (double n = firstN; n <= lastN; n++)
                massive.Add(_grid.GetSn(PN.p, n) * 1e-6);
            string title = "давление, МПа";
            string lineName = "Psn";
            return new DataForChart(massive, title, lineName);
        }
        public DataForChart GetP(double n)
        {
            List<double> massive = new List<double>();
            double lastK = _grid.LastIndexK(PN.p, n);
            double firstK = GetFirstNode(lastK);

            for (double k = firstK; k <= lastK; k++)
                massive.Add(_grid[PN.p, n, k] * 1e-6);
            string title = "давление, МПа";
            string lineName = "P";
            return new DataForChart(massive, title, lineName);
        }
        public DataForChart GetVSn()
        {
            List<double> massive = new List<double>();
            double lastN = _grid.LastIndexNSn(PN.vSn);
            double firstN = GetFirstNode(lastN);

            for (double n = firstN; n <= lastN; n++)
                massive.Add(_grid.GetSn(PN.vSn, n));
            string title = "скорость снаряда, м/с";
            string lineName = "Vsn";
            return new DataForChart(massive, title, lineName);
        }
        public DataForChart GetT(PN pn, IMainData mainData)
        {
            var tau = mainData.ConstParameters.tau;
            List<double> massive = new List<double>();
            double lastN = _grid.LastIndexN(pn);
            double firstN = GetFirstNode(lastN);

            for (double n = firstN; n <= lastN; n++)
                massive.Add(n * tau * 1e3);
            string title = "время, t";
            string lineName = "t";
            return new DataForChart(massive, title, lineName);
        }
        public DataForChart GetX(PN pn, double n, IMainData mainData)
        {
            var h = mainData.ConstParameters.h;
            List<double> massive = new List<double>();
            double lastK = _grid.LastIndexK(pn,n);
            double firstK = GetFirstNode(lastK);

            for (double K = firstK; K <= lastK; K++)
                massive.Add(K * h);
            string title = "длина, м";
            string lineName = "x";
            return new DataForChart(massive, title, lineName);
        }
        public DataForChart GetRo(double n)
        {
            List<double> massive = new List<double>();
            double lastK = _grid.LastIndexK(PN.ro, n);
            double firstK = GetFirstNode(lastK);

            for (double k = firstK; k <= lastK; k++)
                massive.Add(_grid[PN.ro, n, k]);
            string title = "плотность, кг/м³";
            string lineName = "ro";
            return new DataForChart(massive, title, lineName);
        }
        public DataForChart GetTemperature(double n, IMainData mainData)
        {
            var bs = mainData.Barrel.BarrelSize;
            var h = mainData.ConstParameters.h;
            var cv = mainData.ConstParameters.cv;
            List<double> massive = new List<double>();
            double lastK = _grid.LastIndexK(PN.ro, n);
            double firstK = GetFirstNode(lastK);

            for (double k = firstK; k <= lastK; k++)
            {
                var eps = _grid[PN.e,n,k] / (_grid[PN.ro, n, k] * _grid[PN.m, n, k] * bs.S(k * h));
                massive.Add(eps/cv);
            }
            string title = "температура, K";
            string lineName = "T";
            return new DataForChart(massive, title, lineName);
        }
        public DataForChart GetV(double n)
        {
            List<double> massive = new List<double>();
            double lastK = _grid.LastIndexK(PN.v, n);
            double firstK = GetFirstNode(lastK);

            for (double k = firstK; k <= lastK; k++)
                massive.Add(_grid[PN.v, n, k]);
            string title = "скорость газов, м/с";
            string lineName = "V";
            return new DataForChart(massive, title, lineName);
        }
        public DataForChart GetW(double n)
        {
            List<double> massive = new List<double>();
            double lastK = _grid.LastIndexK(PN.w, n);
            double firstK = GetFirstNode(lastK);

            for (double k = firstK; k <= lastK; k++)
                massive.Add(_grid[PN.w, n, k]);
            string title = "скорость твердой фазы, м/с";
            string lineName = "W";
            return new DataForChart(massive, title, lineName);
        }
        public DataForChart GetA(double n, IMainData mainData)
        {
            var constP = mainData.ConstParameters;
            var powder = mainData.Powder;
            var bs = mainData.Barrel.BarrelSize;
            var h = mainData.ConstParameters.h;
            var cv = mainData.ConstParameters.cv;
            List<double> massive = new List<double>();
            double lastK = _grid.LastIndexK(PN.a, n);
            double firstK = GetFirstNode(lastK);
            double a_n = powder.Omega / (powder.LAMDA0 * powder.Delta * bs.Wkm);

            for (double k = firstK; k <= lastK; k++)
                massive.Add(_grid[PN.a, n, k] / a_n);
            string title = "относительная счетная концентрация, %";
            string lineName = "a";
            return new DataForChart(massive, title, lineName);
        }
        public DataForChart GetPsi(double n, IMainData mainData)
        {
            List<double> massive = new List<double>();
            double lastK = _grid.LastIndexK(PN.psi, n);
            double firstK = GetFirstNode(lastK);

            for (double k = firstK; k <= lastK; k++)
                massive.Add(_grid[PN.psi, n, k]);
            string title = "относительная доля сгоревшего пороха, %";
            string lineName = "psi";
            return new DataForChart(massive, title, lineName);
        }
        public DataForChart GetEpure(IMainData mainData)
        {
            List<double> massive = new List<double>();
            double lastN = _grid.LastIndexN(PN.p);
            double firstN = GetFirstNode(lastN);

            double endOfChamberK = _grid.LastIndexK(PN.p, lastN);
            double bottomK = GetFirstNode(endOfChamberK);
            double minNForCurrentK = firstN;

            for(double k = bottomK; k <= endOfChamberK; k++)
            {
                while (k > _grid.LastIndexK(PN.p, minNForCurrentK))
                    minNForCurrentK++;
                double maxP = double.MinValue;
                for(double n = minNForCurrentK; n <= lastN; n++)
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
        private static double GetFirstNode(double lastNode)
        {
            if (lastNode.IsInt())
                return 0;
            else if (lastNode.IsHalfInt())
                return 0.5;
            else
                throw new Exception();
        }
    }
    enum PT
    {
        dynamic,
        mixture
    }
}

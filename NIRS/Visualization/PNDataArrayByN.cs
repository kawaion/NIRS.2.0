
using MyDouble;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using NIRS.Parameter_Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Visualization
{
    public class PNDataArrayByN
    {
        public PN pn { get; private set; }
        public double maxX { get; private set; }
        public List<double> Xs { get; private set; }
        public List<List<double>> Array { get; private set; }
        public PNDataArrayByN(PN pn, IGrid grid, IMainData mainData)
        {
            this.pn = pn;

            Array = PNExtractor.Extract(pn, grid);

            Xs = GetX(pn, grid, mainData);
            maxX = Xs.Last();

            tau = mainData.ConstParameters.tau;
            firstN = FirstLimitedDouble.GetFirstN(pn).GetDouble();
        }

        private List<double> GetX(PN pn, IGrid grid, IMainData mainData)
        {
            List<double> Xs = new List<double>();

            var lastN = grid.LastIndexN(pn);

            var firstK = FirstLimitedDouble.GetFirstK(pn);
            var lastK = grid.LastIndexK(pn, lastN);

            var h = mainData.ConstParameters.h;

            for (LimitedDouble k = firstK; k <= lastK; k++)
            {
                Xs.Add(h * k.GetDouble());
            }
            return Xs;
        }


        private double tau;
        public double firstN { get; private set; }
        public double time(double n) => n * tau;
    }
}

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
    static class PNExtractor
    {
        public static List<List<double>> Extract(PN pn, IGrid grid)
        {
            List<List<double>> data = new List<List<double>>();

            var firstN = FirstLimitedDouble.GetFirstN(pn);
            var lastN = grid.LastIndexN(pn);

            var firstK = FirstLimitedDouble.GetFirstK(pn);

            for (LimitedDouble n = firstN; n<=lastN; n++)
            {
                List<double> subData = new List<double>();
                var lastK = grid.LastIndexK(pn, n);
                for (LimitedDouble k = firstK; k <= lastK; k++)
                {
                    subData.Add(grid[pn, n, k]);
                }
                data.Add(subData);
            }

            return data;
        }
    }
}

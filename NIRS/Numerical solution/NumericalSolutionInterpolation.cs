using MyDouble;
using NIRS.Helpers;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClosedXML.Excel.XLPredefinedFormat;

namespace NIRS.Numerical_solution
{
    internal class NumericalSolutionInterpolation : INumericalSolutionInterpolation
    {
        private readonly IParameterInterpolationFunctions _functions;
        private readonly XGetter x;

        public NumericalSolutionInterpolation(IParameterInterpolationFunctions parameterInterpolationFunctions,IMainData mainData)
        {
            _functions = parameterInterpolationFunctions;
            x = new XGetter(mainData.ConstParameters);
        }
        //public IGrid GetAtUnavailableNodes(IGrid g, LimitedDouble n)
        //{
        //    var xSn = g.GetSn(PN.x, n);
        //    if (n.IsInt())
        //    {

        //        foreach (var pn in VectorPN.mixture)
        //        {
        //            while (isExistNonCalculatedNodes(g, n, pn, xSn))
        //            {
        //                var kLast = g.LastIndexK(pn, n);
        //                if (pn == PN.e && n == 1078 && (kLast + 1) == 79.5)
        //                {
        //                    int c = 0;
        //                }

        //                g[pn, n, kLast + 1] = _functions.InterpolateMixture(pn, n, xSn, kLast);
        //            }
        //        }
        //    }

        //    if (n.IsHalfInt())
        //    {
        //        foreach (var pn in VectorPN.dynamic)
        //        {
        //            while (isExistNonCalculatedNodes(g, n, pn, xSn))
        //            {
        //                var kLast = g.LastIndexK(pn, n);
        //                g[pn, n, kLast + 1] = _functions.InterpolateDynamic(pn, n, kLast, xSn);
        //            }
        //        }
        //    }
        //    return g;
        //}
        static LimitedDouble  lastnodeN;
        public IGrid GetAtUnavailableNodes(IGrid g, LimitedDouble n)
        {

            var xSn = g.GetSn(PN.x, n);
            if (n.IsInt())
            {
                
                foreach (var pn in VectorPN.mixture)
                {
                    //
                    int number = 0;
                    //
                    while (isExistNonCalculatedNodes(g, n, pn, xSn))
                    {
                        //
                        number++;
                        //
                        var kLast = g.LastIndexK(pn, n);
                        if (pn == PN.e && n == 1078 && (kLast + 1) == 79.5)
                        {
                            int c = 0;
                        }
                        
                        //g[pn, n, kLast + 1] = 
                        _functions.InterpolateMixture(pn, n, xSn, kLast, number);
                    }
                }
            }

            if (n.IsHalfInt())
            {
                foreach (var pn in VectorPN.dynamic)
                {

                    //
                    int number = 0;
                    //
                    while (isExistNonCalculatedNodes(g, n, pn, xSn))
                    {
                        //
                        number++;
                        //
                        var kLast = g.LastIndexK(pn, n);
                        //g[pn, n, kLast + 1] =
                        _functions.InterpolateDynamic(pn, n, kLast, xSn, number);
                    }
                }
            }

            return g;

        }
        private bool isExistNonCalculatedNodes(IGrid g, LimitedDouble n, PN pn, double xSn)
        {
            var kLast = g.LastIndexK(pn, n);
            var xEmptyNode = x[kLast+1];
            return xSn >= xEmptyNode;
        }
    }
}

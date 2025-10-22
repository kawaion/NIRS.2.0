﻿using MyDouble;
using NIRS.Helpers;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public IGrid Get(IGrid g, LimitedDouble n)
        {
            var xSn = g.GetSn(PN.x, n);
            if (n.IsInt())
            {
                
                foreach (var pn in VectorPN.mixture)
                {
                    while (isExistNonCalculatedNodes(g, n, pn, xSn))
                    {
                        var kLast = g.LastIndexK(pn, n);
                        g[pn, n, kLast + 1] = _functions.InterpolateMixture(pn, n, xSn, kLast);
                    }
                }
            }

            if (n.IsHalfInt())
            {
                foreach (var pn in VectorPN.dynamic)
                {
                    while (isExistNonCalculatedNodes(g, n, pn, xSn))
                    {
                        var kLast = g.LastIndexK(pn, n);
                        g[pn, n, kLast + 1] = _functions.InterpolateDynamic(pn, n, kLast, xSn);
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

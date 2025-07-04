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
        public IGrid Get(IGrid g, double n)
        {
            List<PN> parameters;
            if (n.IsInt())
            {
                parameters = new List<PN> { PN.r, PN.z, PN.psi, PN.a, PN.m, PN.ro, PN.e, PN.p };

                foreach (var pn in parameters)
                {
                    while (isExistNonCalculatedNodes(g, n, pn))
                    {
                        var kLast = g.LastIndexK(pn, n);
                        g[pn, n, kLast + 1] = _functions.InterpolateMixture(pn, n, kLast + 1);
                    }
                }
            }

            if (n.IsHalfInt())
            {
                parameters = new List<PN>() { PN.v, PN.w, PN.dynamic_m, PN.M };

                foreach (var pn in parameters)
                {
                    while (isExistNonCalculatedNodes(g, n, pn))
                    {
                        var kLast = g.LastIndexK(pn, n);
                        g[pn, n, kLast + 1] = _functions.InterpolateDynamic(pn, n, kLast + 1);
                    }
                }
            }

            return g;

        }
        private bool isExistNonCalculatedNodes(IGrid g, double n, PN pn)
        {
            var kLast = g.LastIndexK(pn, n);
            var xEmptyNode = x[kLast+1];
            return g.GetSn(PN.x, n) >= xEmptyNode;
        }
    }
}

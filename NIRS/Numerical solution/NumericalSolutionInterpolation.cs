using MyDouble;
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
            List<PN> parameters;
            if(n.Type == DoubleType.Int)
            {
                parameters = new List<PN>();
                parameters.Add(PN.r);
                parameters.Add(PN.z);
                parameters.Add(PN.a);
                parameters.Add(PN.m);
                parameters.Add(PN.ro);
                parameters.Add(PN.e);
                parameters.Add(PN.p);

            foreach(var pn in parameters)
            {
                while (isExistNonCalculatedNodes(g, n, pn))
                {
                    var kLast = g[n].LastIndex(pn);
                    g[n][kLast + 1][pn] = _functions.InterpolateMixture(pn, n, kLast + 1);
                }
            }
            }


            parameters = new List<PN>();
            parameters.Add(PN.v);
            parameters.Add(PN.w);
            parameters.Add(PN.dynamic_m);
            parameters.Add(PN.M);

            foreach (var pn in parameters)
            {
                while (isExistNonCalculatedNodes(g, n, pn))
                {
                    var kLast = g[n].LastIndex(pn);
                    g[n][kLast + 1][pn] = _functions.InterpolateMixture(pn, n, kLast + 1);
                }
            }
        }

        private bool isExistNonCalculatedNodes(IGrid g, LimitedDouble n, PN pn)
        {
            var kLast = g[n].LastIndex(pn);
            return g[n].sn.x - x[kLast] >= 1;
        }
    }
}

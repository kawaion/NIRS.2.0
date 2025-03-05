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
        public NumericalSolutionInterpolation(IParameterInterpolationFunctions parameterInterpolationFunctions)
        {
            _functions = parameterInterpolationFunctions;
        }
        public IGrid Get(IGrid g, LimitedDouble n)
        {
            InterpolateStep step = new InterpolateStep(g);
            var xSnFloor = LimitedDouble.Floor(g[n].sn.x);
            List<PN> parameters = new List<PN>();
            parameters.Add(PN.r);
            parameters.Add(PN.z);
            parameters.Add(PN.a);
            parameters.Add(PN.m);
            parameters.Add(PN.ro);
            parameters.Add(PN.e);
            parameters.Add(PN.p);

            foreach(var pn in parameters)
            {
                int dotCount=0;
                do
                {
                    dotCount = step.Get(n, xSnFloor, pn);
                    var kLast=g[n].LastIndex(pn);
                    g[n][kLast + 1][pn] = _functions.InterpolateMixture(pn,)
                }while(dotCount>0);
                

            }

            
        }
    }
}

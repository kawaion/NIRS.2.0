using MyDouble;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Helpers
{
    internal class Differencial
    {
        private readonly IGrid g;
        private readonly XGetter x;

        public Differencial(IGrid grid, IConstParameters constParameters)
        {
            g = grid;
            x = new XGetter(constParameters);
        }
        public double dvdx(LimitedDouble n)
        {
            return (g[n].sn.vSn - g[n].Last().v) /
                   (g[n].sn.x - x[g[n].LastIndex()]);
        }
        public double dwdx(LimitedDouble n)
        {
            return (g[n].sn.vSn - g[n].Last().w) /
                   (g[n].sn.x - x[g[n].LastIndex()]);
        }



    }
}

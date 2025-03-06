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
            var kLast = g[n].LastIndex(PN.v);
            return (g[n].sn.vSn - g[n][kLast].v) /
                   (g[n].sn.x - x[kLast]);
        }
        public double dwdx(LimitedDouble n)
        {
            var kLast = g[n].LastIndex(PN.v);
            return (g[n].sn.vSn - g[n][kLast].w) /
                   (g[n].sn.x - x[kLast]);
        }

        public double dPNdx(LimitedDouble n, PN pn)
        {
            var kLast = g[n].LastIndex(pn);
            return (g[n].sn[pn] - g[n][kLast][pn]) /
                   (g[n].sn.x - x[kLast]);
        }



    }
}

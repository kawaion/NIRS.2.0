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
        public double dvdx(double n)
        {
            var kLast = g.LastIndexK(PN.v, n);
            return (g.GetSn(PN.vSn, n) - g[PN.v, n, kLast]) /
                   (g.GetSn(PN.x, n) - x[kLast]);
        }
        public double dwdx(double n)
        {
            var kLast = g.LastIndexK(PN.w, n);
            return (g.GetSn(PN.vSn, n) - g[PN.w, n, kLast]) /
                   (g.GetSn(PN.x, n) - x[kLast]);
        }

        public double dPNdx(double n, PN pn)
        {
            var kLast = g.LastIndexK(pn, n);
            return (g.GetSn(pn, n) - g[pn, n, kLast]) /
                   (g.GetSn(PN.x, n) - x[kLast]);
        }



    }
}

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
        //public double dvdx(LimitedDouble n)
        //{
        //    var kLast = g.LastIndexK(PN.v, n);                

        //    var tmp1 = g.GetSn(PN.vSn, n);
        //    var tmp2 = g[PN.v, n, kLast];
        //    var tmp3 = g.GetSn(PN.x, n);
        //    var tmp4 = x[kLast];

        //    var res = (g.GetSn(PN.vSn, n) - g[PN.v, n, kLast]) /
        //           (g.GetSn(PN.x, n) - x[kLast]);

        //    return res;
        //}

        //public double dwdx(LimitedDouble n)
        //{
        //    var kLast = g.LastIndexK(PN.w, n);
        //    return (g.GetSn(PN.vSn, n) - g[PN.w, n, kLast - 1]) /
        //           (g.GetSn(PN.x, n) - x[kLast - 1]);
        //}

        //
        public double dvdx(LimitedDouble n)
        {
            var kLast = g.LastIndexK(PN.v, n);

            var tmp1 = g.GetSn(PN.vSn, n);
            var tmp2 = g[PN.v, n, kLast - 1];
            var tmp3 = g.GetSn(PN.x, n);
            var tmp4 = x[kLast - 1];

            var res = (g.GetSn(PN.vSn, n) - g[PN.v, n, kLast - 1]) /
                   (g.GetSn(PN.x, n) - x[kLast - 1]);

            return res;
        }
        
        public double dwdx(LimitedDouble n)
        {
            var kLast = g.LastIndexK(PN.w, n);
            return (g.GetSn(PN.vSn, n) - g[PN.w, n, kLast - 1]) /
                   (g.GetSn(PN.x, n) - x[kLast - 1]);
        }
        //
        public double dPNdx(LimitedDouble n, PN pn)
        {
            var kLast = g.LastIndexK(pn, n);
            return (g.GetSn(pn, n) - g[pn, n, kLast]) /
                   (g.GetSn(PN.x, n) - x[kLast]);
        }



    }
}

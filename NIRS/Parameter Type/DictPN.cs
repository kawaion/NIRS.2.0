using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Parameter_Type
{
    static class DictPN
    {
        private static Dictionary<string, PN> dict = new Dictionary<string, PN>()
        {
            { "dynamic_m",PN.dynamic_m },
            { "M",PN.M },
            { "v",PN.v },
            { "w",PN.w },


            { "r",PN.r },
            { "e",PN.e },
            { "eps",PN.eps },
            { "psi",PN.psi },
            { "z",PN.z },
            { "a",PN.a },
            { "m",PN.m },
            { "p",PN.p },
            { "rho",PN.rho },
        };

        public static PN Get(string pn)
        {
            return dict[pn];
        }
    }
}

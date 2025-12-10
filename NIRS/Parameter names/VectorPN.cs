using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Parameter_names
{
    class VectorPN
    {
        public static List<PN> mixture = new List<PN> { PN.r, PN.z, PN.a, PN.psi, PN.m, PN.rho, PN.e, PN.p};
        public static List<PN> dynamic = new List<PN> { PN.v, PN.w, PN.dynamic_m, PN.M};
    };
}


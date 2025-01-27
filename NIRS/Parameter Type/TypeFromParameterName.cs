﻿using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Parameter_Type
{
    public static class TypeFromParameterName
    {
        public static PT GetTypeFromParameterName(this PN pn)
        {
            switch (pn)
            {
                case PN.dynamic_m:
                case PN.M:
                case PN.v:
                case PN.w:
                    return PT.Dynamic;
                case PN.r:
                case PN.e:
                case PN.eps:
                case PN.psi:
                case PN.z:
                case PN.a:
                case PN.m:
                case PN.p:
                case PN.ro:
                    return PT.Mixture;
                default: throw new Exception("неизвестный параметр");
            }
        }

    }
}
    


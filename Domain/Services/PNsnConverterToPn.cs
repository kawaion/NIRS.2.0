using Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Services;

internal static class PNsnConverterToPn
{
    public static PN Convert(PNsn pn)
    {
        switch (pn)
        {
            case PNsn.dynamic_m: return PN.dynamic_m;
            case PNsn.M: return PN.M;
            case PNsn.r: return PN.r;
            case PNsn.e: return PN.e;
            case PNsn.eps: return PN.eps;
            case PNsn.psi: return PN.psi;
            case PNsn.z: return PN.z;
            case PNsn.a: return PN.a;
            case PNsn.m: return PN.m; 
            case PNsn.p: return PN.p;
            case PNsn.rho: return PN.rho;
            default: throw new Exception("неизвестный параметр");
        }
    }
}

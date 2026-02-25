using Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Services;

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
            case PN.rho:
                return PT.Mixture;
            default: throw new Exception("неизвестный параметр");
        }
    }
    public static PT GetTypeFromParameterName(this PNsn pn)
    {
        switch (pn)
        {
            case PNsn.dynamic_m:
            case PNsn.M:
            case PNsn.vSn:
                return PT.Dynamic;
            case PNsn.r:
            case PNsn.e:
            case PNsn.eps:
            case PNsn.psi:
            case PNsn.z:
            case PNsn.a:
            case PNsn.m:
            case PNsn.p:
            case PNsn.rho:
                return PT.Mixture;
            default: throw new Exception("неизвестный параметр");
        }
    }

}

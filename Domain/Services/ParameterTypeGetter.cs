using Core.Domain.Enums;
using Core.Domain.Limited_Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Services;

static class ParameterTypeGetter
{
    public static bool IsDynamic(LimitedDouble n, LimitedDouble k)
    {
        return n.IsHalf && k.IsInt;
    }
    public static bool IsMixture(LimitedDouble n, LimitedDouble k)
    {
        return n.IsInt && k.IsHalf;
    }

    public static bool IsDynamic(LimitedDouble n)
    {
        return n.IsHalf;
    }
    public static bool IsMixture(LimitedDouble n)
    {
        return n.IsInt;
    }

    public static bool IsDynamic(this PN pn)
    {
        return pn.GetTypeFromParameterName() == PT.Dynamic;
    }
    public static bool IsMixture(this PN pn)
    {
        return pn.GetTypeFromParameterName() == PT.Mixture;
    }

    public static bool IsDynamic(this PNsn pn)
    {
        return pn.GetTypeFromParameterName() == PT.Dynamic;
    }
    public static bool IsMixture(this PNsn pn)
    {
        return pn.GetTypeFromParameterName() == PT.Mixture;
    }
}

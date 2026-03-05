using Core.Domain.Common;
using Core.Domain.Enums;
using Core.Domain.Limited_Double;
using Core.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Grid.ValueObjects;

internal class GridMapper : ValueObject
{
    private GridMapper()
    {
    }
    public static GridMapper Create()
    {
        var instance = new GridMapper();

        return instance;
    }
    
    public (int, int, int) MappingAllToInt(PN pn, LimitedDouble n, LimitedDouble k)
    {
        return (
                  (int)pn,
                  ConvertToNIndex(n),
                  ConvertToKIndex(k)
               );
    }
    public int MappingNToInt(LimitedDouble n)
    {
        return ConvertToNIndex(n);
    }
    public int MappingKToInt(LimitedDouble k)
    {
        return ConvertToKIndex(k);
    }
    public int MappingForUntypeNToInt(LimitedDouble n)
    {
        return ConvertToNIndexForUntype(n);
    }
    public int MappingPNToInt(PN pn)
    {
        return (int)pn;
    }
    public int MappingPNToInt(PNsn pn)
    {
        return (int)pn;
    }
    private int ConvertToNIndex(LimitedDouble n)
    {
        return n.GetInt();
    }

    private int ConvertToKIndex(LimitedDouble k)
    {
        return k.GetInt();
    }

    private int ConvertToNIndexForUntype(LimitedDouble n)
    {
        var res = n.GetInt() * 2;

        if (ParameterTypeGetter.IsDynamic(n))
            res += 1;

        return res;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return true;
    }
}

using Core.Domain.Common;
using Core.Domain.Enums;
using Core.Domain.Limited_Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Grid.ValueObjects;

internal class GridMapper : ValueObject
{
    private LimitedDouble _maximumnNegativeN;
    private LimitedDouble _maximumnNegativeK;

    private GridMapper(double maximumnNegativeN, double maximumnNegativeK)
    {
        _maximumnNegativeN = maximumnNegativeN;
        _maximumnNegativeK = maximumnNegativeK;
    }
    public static GridMapper Create(double maximumnNegativeN, double maximumnNegativeK)
    {
        var instance = new GridMapper(maximumnNegativeN, maximumnNegativeK);

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
    public int MappingPNToInt(PN pn)
    {
        return (int)pn;
    }
    private int ConvertToNIndex(LimitedDouble n)
    {
        return (n - _maximumnNegativeN).GetInt();
    }

    private int ConvertToKIndex(LimitedDouble k)
    {
        return (k - _maximumnNegativeK).GetInt();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return _maximumnNegativeN;
        yield return _maximumnNegativeK;
    }
}

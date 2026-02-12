using Core.Domain.Common;
using Core.Domain.Enums;
using Core.Domain.Limited_Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Grid.ValueObjects;

internal class LastIndexer : ValueObject
{
    static readonly int DynamicParamIndex = (int)PN.dynamic_m;
    static readonly int MixtureParamIndex = (int)PN.r;

    private readonly GridMapper _gridMapper;

    private LastKArray lastK;
    private LastNArray lastN;

    public LastIndexer(GridMapper gridMapper, int countParams)
    {
        _gridMapper = gridMapper;

        lastK = new LastKArray(countParams, new LimitedDouble(-1));
        lastN = new LastNArray(countParams, new LimitedDouble(-1));
    }
    public LimitedDouble LastIndexK(PN pn, LimitedDouble n)
    {
        var paramIndex = (int)pn;
        var nIndex = _gridMapper.MappingNToInt(n);
        return lastK[paramIndex, nIndex];
    }
    public LimitedDouble LastIndexK(LimitedDouble n)
    {
        var nIndex = _gridMapper.MappingNToInt(n);
        if (n.IsHalf)
            return lastK[DynamicParamIndex, nIndex];
        else
            return lastK[MixtureParamIndex, nIndex];
    }

    public LimitedDouble LastIndexN(PN pn)
    {
        var paramIndex = _gridMapper.MappingPNToInt(pn);
        return lastN[paramIndex];
    }
    public LimitedDouble LastIndexN()
    {
        var nDynamic = lastN[DynamicParamIndex];
        var nMixture = lastN[MixtureParamIndex];
        if (nDynamic > nMixture)
            return nDynamic;
        else
            return nMixture;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}

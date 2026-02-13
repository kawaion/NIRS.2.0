using Core.Domain.Common;
using Core.Domain.Enums;
using Core.Domain.Limited_Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Grid.ValueObjects;

internal class LastIndexer : ValueObject
{
    static readonly int DynamicParamIndex = (int)PN.dynamic_m;
    static readonly int MixtureParamIndex = (int)PN.r;

    private readonly GridMapper _gridMapper;
    private readonly int _countParams;

    private LastKArray lastK;
    private LastNArray lastN;

    private LastIndexer(GridMapper gridMapper, int countParams)
    {
        _gridMapper = gridMapper;
        _countParams = countParams;

        lastK = LastKArray.Create(countParams, new LimitedDouble(-1));
        lastN = LastNArray.Create(countParams, new LimitedDouble(-1));
    }
    public static LastIndexer Create(GridMapper gridMapper, int countParams)
    {
        Validate(countParams);

        var instance = new LastIndexer(gridMapper, countParams);

        return instance;
    }
    private static void Validate(int countParams)
    {
        if (countParams <= 0)
            throw new Exception("количство параметров должно быть больше 0");
    }
    public void TryIncreaseLastIlndex(LimitedDouble n, LimitedDouble k, int paramIndex, int nIndex)
    {
        if (lastN[paramIndex] < n)
            lastN[paramIndex] = n;

        if (lastK[paramIndex,nIndex] < k)
            lastK[paramIndex,nIndex] = k;
    }

    public LimitedDouble LastIndexK(PN pn, LimitedDouble n)
    {
        var paramIndex = _gridMapper.MappingPNToInt(pn);
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
        yield return _gridMapper;
        yield return _countParams;
        yield return lastK;
        yield return lastN;
    }
}

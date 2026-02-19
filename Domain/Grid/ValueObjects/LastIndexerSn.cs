using Core.Domain.Common;
using Core.Domain.Enums;
using Core.Domain.Limited_Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Grid.ValueObjects;

internal class LastIndexerSn : ValueObject
{
    static readonly int DynamicParamIndex = (int)PN.dynamic_m;
    static readonly int MixtureParamIndex = (int)PN.r;

    private readonly GridMapper _gridMapper;
    private readonly int _countParams;

    private LastNArray lastN;

    private LastIndexerSn(GridMapper gridMapper, int countParams)
    {
        _gridMapper = gridMapper;
        _countParams = countParams;

        lastN = LastNArray.Create(countParams, new LimitedDouble(-1));
    }
    public static LastIndexerSn Create(GridMapper gridMapper, int countParams)
    {
        Validate(countParams);

        var instance = new LastIndexerSn(gridMapper, countParams);

        return instance;
    }
    private static void Validate(int countParams)
    {
        if (countParams <= 0)
            throw new Exception("количство параметров должно быть больше 0");
    }
    public void TryIncreaseLastIlndex(LimitedDouble n, int paramIndex, int nIndex)
    {
        if (lastN[paramIndex] < n)
            lastN[paramIndex] = n;
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
        yield return lastN;
    }
}

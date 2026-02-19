using Core.Domain.Common;
using Core.Domain.Enums;
using Core.Domain.Grid.Entities;
using Core.Domain.Grid.Exceptions;
using Core.Domain.Grid.Interfaces;
using Core.Domain.Grid.ValueObjects;
using Core.Domain.Limited_Double;
using Core.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Grid.Aggregates;

internal class TimeSpaceGridSn : Entity
{
    static readonly int COUNT_PARAMS = Enum.GetValues(typeof(PNsn)).Length;

    private const double MAXIMUM_NEGATIVE_N = -1;
    private const double MAXIMUM_NEGATIVE_K = -1;

    private Dynamic2DArray _data;

    private GridMapper _gridMapper;

    private LastIndexerSn _lastIndexerSn;
    public TimeSpaceGridSn(GridMapper gridMapper)
    {
        _data = Dynamic2DArray.CreateWithExponentialExpansion(COUNT_PARAMS);
        _gridMapper = gridMapper;
        _lastIndexerSn = LastIndexerSn.Create(_gridMapper, COUNT_PARAMS);
    }
    public double this[PN pn, LimitedDouble n]
    {
        get
        {
            Validation(pn, n);

            if (pn == PN.One_minus_m)
                return 1.02 - this[PN.m, n];

            var paramIndex = _gridMapper.MappingPNToInt(pn);
            var nIndex = _gridMapper.MappingNToInt(n);

            try
            {
                return _data[paramIndex, nIndex];
            }
            catch (UninitializedElementException ex)
            {
                throw new Exception($"Неинициализованное значение по адресу: {pn}, {n}");
            }

        }
    }
    public void Set(PN pn, LimitedDouble n, double value)
    {
        Validation(pn, n);

        if (pn == PN.One_minus_m)
            Set(pn, n, 1.02 - this[PN.m, n]);

        var paramIndex = _gridMapper.MappingPNToInt(pn);
        var nIndex = _gridMapper.MappingNToInt(n);

        _lastIndexerSn.TryIncreaseLastIlndex(n, paramIndex, nIndex);

        _data[paramIndex, nIndex] = value;
    }
    public ISetter At(PN pn, LimitedDouble n, LimitedDouble k)
    {
        return new SetterSn(this, pn, n);
    }


    public LimitedDouble LastIndexN(PN pn)
    {
        return _lastIndexerSn.LastIndexN(pn);
    }
    public LimitedDouble LastIndexN()
    {
        return _lastIndexerSn.LastIndexN();
    }


    private void Validation(PN pn, LimitedDouble n)
    {
        if (ParameterTypeGetter.IsMixture(n) && ParameterTypeGetter.IsMixture(pn))
            return;
        if (ParameterTypeGetter.IsDynamic(n) && ParameterTypeGetter.IsDynamic(pn))
            return;
        throw new Exception("значение не является ни динамическим параметром, ни параметром состояния смеси");
    }
}

internal class SetterSn : ISetter
{
    TimeSpaceGridSn _grid;
    PN _pn;
    LimitedDouble _n;
    internal SetterSn(TimeSpaceGridSn grid, PN pn, LimitedDouble n)
    {
        _grid = grid;
        _pn = pn;
        _n = n;
    }
    public void Set(double value)
    {
        _grid.Set(_pn, _n, value);
    }
}
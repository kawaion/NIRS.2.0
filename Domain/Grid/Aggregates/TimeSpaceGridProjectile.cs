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

internal class TimeSpaceGridProjectile : Entity, IGridProjectile
{
    static readonly int COUNT_PARAMS = Enum.GetValues(typeof(PNsn)).Length;

    private const double MAXIMUM_NEGATIVE_N = -1;
    private const double MAXIMUM_NEGATIVE_K = -1;

    private Dynamic2DArray _data;
    private Dynamic2DArray _dataX;

    private GridMapper _gridMapper;

    private LastIndexerSn _lastIndexerSn;
    public TimeSpaceGridProjectile(GridMapper gridMapper)
    {
        _data = Dynamic2DArray.CreateWithExponentialExpansion(COUNT_PARAMS-1);
        _dataX = Dynamic2DArray.CreateWithExponentialExpansion(1);
        _gridMapper = gridMapper;
        _lastIndexerSn = LastIndexerSn.Create(_gridMapper, COUNT_PARAMS);
    }
    public double this[PNsn pn, LimitedDouble n]
    {
        get
        {
            if (pn == PNsn.x)
                return GetX(n);

            Validation(pn, n);

            if (pn == PNsn.One_minus_m)
                return 1.02 - this[PNsn.m, n];

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
    private double GetX(LimitedDouble n)
    {
        var nIndex = _gridMapper.MappingForUntypeNToInt(n);

        try
        {
            return _dataX[0, nIndex];
        }
        catch (UninitializedElementException ex)
        {
            throw new Exception($"Неинициализованное значение _x по адресу: {n}");
        }
    }

    public void Set(PNsn pn, LimitedDouble n, double value)
    {
        if (pn == PNsn.x)
            SetX(n, value);

        Validation(pn, n);

        if (pn == PNsn.One_minus_m)
            Set(pn, n, 1.02 - this[PNsn.m, n]);

        var paramIndex = _gridMapper.MappingPNToInt(pn);
        var nIndex = _gridMapper.MappingNToInt(n);

        _lastIndexerSn.TryIncreaseLastIlndex(n, paramIndex, nIndex);

        _data[paramIndex, nIndex] = value;
    }
    private void SetX(LimitedDouble n, double value)
    {
        var paramIndex = _gridMapper.MappingPNToInt(PNsn.x);
        var nIndex = _gridMapper.MappingForUntypeNToInt(n);

        _lastIndexerSn.TryIncreaseLastIlndex(n, paramIndex, nIndex);

        _dataX[0, nIndex] = value;
    }
    public ISetter At(PNsn pn, LimitedDouble n, LimitedDouble k)
    {
        return new SetterProjectile(this, pn, n);
    }


    public LimitedDouble LastIndexN(PN pn)
    {
        return _lastIndexerSn.LastIndexN(pn);
    }
    public LimitedDouble LastIndexN()
    {
        return _lastIndexerSn.LastIndexN();
    }


    private void Validation(PNsn pn, LimitedDouble n)
    {
        if (ParameterTypeGetter.IsMixture(n) && ParameterTypeGetter.IsMixture(pn))
            return;
        if (ParameterTypeGetter.IsDynamic(n) && ParameterTypeGetter.IsDynamic(pn))
            return;
        throw new Exception("значение не является ни динамическим параметром, ни параметром состояния смеси");
    }
}

internal class SetterProjectile : ISetter
{
    TimeSpaceGridProjectile _grid;
    PNsn _pn;
    LimitedDouble _n;
    internal SetterProjectile(TimeSpaceGridProjectile grid, PNsn pn, LimitedDouble n)
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
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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Grid.Aggregates;

internal class TimeSpaceGrid : Entity
{
    static readonly int COUNT_PARAMS = Enum.GetValues(typeof(PN)).Length;    
    
    private const double MAXIMUM_NEGATIVE_N = -1;
    private const double MAXIMUM_NEGATIVE_K = -1;

    

    private Dynamic3DArray _data;

    private GridMapper _gridMapper;

    private LastIndexer _lastIndexer;
    public TimeSpaceGrid()
    {
        _data = Dynamic3DArray.CreateWithExponentialExpansion(COUNT_PARAMS);
        _gridMapper = GridMapper.Create(MAXIMUM_NEGATIVE_N, MAXIMUM_NEGATIVE_K);
        _lastIndexer = LastIndexer.Create(_gridMapper, COUNT_PARAMS);
        sn = new TimeSpaceGridProjectile(_gridMapper);
    }
    public double this[PN pn, LimitedDouble n, LimitedDouble k]
    {
        get
        {
            Validation(pn, n, k);

            if (pn == PN.One_minus_m)
                return 1.02 - this[PN.m, n, k];

            (var paramIndex, var nIndex, var kIndex) = _gridMapper.MappingAllToInt(pn, n, k);

            try
            {
                return _data[paramIndex, nIndex, kIndex];
            }
            catch (UninitializedElementException ex) 
            { 
                throw new Exception($"Неинициализованное значение по адресу: {pn}, {n}, {k}");
            }
            
        }
    }        
    public void Set(PN pn, LimitedDouble n, LimitedDouble k, double value)
    {
        Validation(pn, n, k);

        if (pn == PN.One_minus_m)
            Set(pn, n, k,   1.02 - this[PN.m, n, k]);

        (var paramIndex, var nIndex, var kIndex) = _gridMapper.MappingAllToInt(pn, n, k);

        _lastIndexer.TryIncreaseLastIlndex(n, k, paramIndex, nIndex);

        _data[paramIndex, nIndex, kIndex] = value;
    }
    public ISetter At(PN pn, LimitedDouble n, LimitedDouble k)
    {
        return new Setter(this, pn, n, k);
    }

    public LimitedDouble LastIndexK(PN pn, LimitedDouble n)
    {
        return _lastIndexer.LastIndexK(pn, n);
    }
    public LimitedDouble LastIndexK(LimitedDouble n)
    {
        return _lastIndexer.LastIndexK(n);
    }

    public LimitedDouble LastIndexN(PN pn)
    {
        return _lastIndexer.LastIndexN(pn);
    }
    public LimitedDouble LastIndexN()
    {
        return _lastIndexer.LastIndexN();
    }


    public TimeSpaceGridProjectile sn {  get; set; }


    private void Validation(PN pn, LimitedDouble n, LimitedDouble k)
    {
        if (ParameterTypeGetter.IsMixture(n, k) && ParameterTypeGetter.IsMixture(pn))
            return;
        if (ParameterTypeGetter.IsDynamic(n, k) && ParameterTypeGetter.IsDynamic(pn))
            return;
        throw new Exception("значение не является ни динамическим параметром, ни параметром состояния смеси");
    }
}    

internal class Setter : ISetter
{
    private TimeSpaceGrid _grid;
    private PN _pn;
    private LimitedDouble _n;
    private LimitedDouble _k;
    internal Setter(TimeSpaceGrid grid, PN pn, LimitedDouble n, LimitedDouble k)
    {
        _grid = grid;
        _pn = pn;
        _n = n;
        _k = k;
    }
    public void Set(double value) 
    { 
        _grid.Set(_pn, _n, _k, value);
    }
} 

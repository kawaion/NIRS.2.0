using Core.Domain.Common;
using Core.Domain.Grid.Exceptions;
using Core.Domain.Grid.Interfaces;
using Core.Domain.Grid.Services;
using Core.Domain.Grid.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Grid.Entities;

public sealed class Dynamic2DArray : Entity, ICloneable
{
    private double[][] _data;

    private readonly int _fixedX;
    private int _currentY;
    private readonly IArrayExpansionStrategy _expansionStrategy;
    private bool _disposed;

    private const double UNINITIALIZED_VALUE = double.NaN;
    private const int INITIAL_DIMENSION_SIZE = 32;

    private Dynamic2DArray(int fixedX, int initialY = INITIAL_DIMENSION_SIZE,
        IArrayExpansionStrategy expansionStrategy = null)
    {
        _fixedX = fixedX;
        _currentY = initialY;
        _expansionStrategy = expansionStrategy ?? ExponentialExpansionStrategy.Create();

        InitializeData();
    }

    private void InitializeData()
    {
        _data = new double[_fixedX][];

        for (int x = 0; x < _fixedX; x++)
        {
            _data[x] = new double[_currentY];
            _data[x] = ArrayInitializerWithNumber.Initialize(_data[x], UNINITIALIZED_VALUE);
        }
    }

    public static Dynamic2DArray CreateWithExponentialExpansion(int fixedX, int initialY = INITIAL_DIMENSION_SIZE,
        double growthFactor = 2.0)
    {
        return new Dynamic2DArray(fixedX, initialY,
            ExponentialExpansionStrategy.Create(growthFactor));
    }

    public static Dynamic2DArray CreateWithFixedIncrement(int fixedX, int initialY = INITIAL_DIMENSION_SIZE,
        int increment = 32)
    {
        return new Dynamic2DArray(fixedX, initialY,
            FixedIncrementExpansionStrategy.Create(increment));
    }

    public double this[int x, int y]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            double value = _data[x][y];

            if (double.IsNaN(value))
                ThrowNotInitialized(x, y);

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            EnsureCapacity(y);

            _data[x][y] = value;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnsureCapacity(int y)
    {
        if (y >= _currentY)
        {
            ExpandArray(y);
        }
    }

    private void ExpandArray(int requiredY)
    {
        int newY = _expansionStrategy.CalculateNewDimension(_currentY, requiredY);
        ResizeData(newY);
    }

    private void ResizeData(int newY)
    {
        for (int x = 0; x < _fixedX; x++)
        {
            var newRow = new double[newY];
            newRow = ArrayInitializerWithNumber.Initialize(newRow, UNINITIALIZED_VALUE);

            int copyY = Math.Min(_currentY, newY);
            Array.Copy(_data[x], 0, newRow, 0, copyY);

            _data[x] = newRow;
        }

        _currentY = newY;
    }

    public bool IsInitialized(int x, int y)
    {
        if (x < 0 || x >= _fixedX || y < 0 || y >= _currentY)
            return false;

        return !double.IsNaN(_data[x][y]);
    }

    public void Clear()
    {
        for (int x = 0; x < _fixedX; x++)
        {
            Array.Fill(_data[x], UNINITIALIZED_VALUE);
        }
    }

    public (int FixedX, int CurrentY) GetDimensions()
    {
        return (_fixedX, _currentY);
    }

    public object Clone()
    {
        var (fixedX, currentY) = GetDimensions();
        var clone = new Dynamic2DArray(fixedX, currentY);

        for (int x = 0; x < fixedX; x++)
        {
            Array.Copy(_data[x], clone._data[x], currentY);
        }

        return clone;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void ThrowNotInitialized(int x, int y)
    {
        throw new UninitializedElementException($"Неинициализованное значение по адресу: {x}, {y}");
    }
}

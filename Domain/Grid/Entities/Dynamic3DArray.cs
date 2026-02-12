using Core.Domain.Common;
using Core.Domain.Entities;
using Core.Domain.Grid.Exceptions;
using Core.Domain.Grid.Interfaces;
using Core.Domain.Grid.Services;
using Core.Domain.Grid.ValueObjects;
using Core.Domain.Physical.ValueObjects.Main;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Grid.Entities;

public sealed class Dynamic3DArray : Entity, ICloneable
{
    private double[][,] _data;

    private readonly int _fixedX;
    private int _currentY;
    private int _currentZ;
    private readonly IArrayExpansionStrategy _expansionStrategy;
    private bool _disposed;

    private const double UNINITIALIZED_VALUE = double.NaN;
    private const int INITIAL_DIMENSION_SIZE = 32;

    private Dynamic3DArray(int fixedX, int initialY = INITIAL_DIMENSION_SIZE,
        int initialZ = INITIAL_DIMENSION_SIZE, IArrayExpansionStrategy expansionStrategy = null)
    {
        _fixedX = fixedX;
        _currentY = initialY;
        _currentZ = initialZ;
        _expansionStrategy = expansionStrategy ?? new ExponentialExpansionStrategy();

        InitializeData();
    }
    private void InitializeData()
    {
        _data = new double[_fixedX][,];

        for (int x = 0; x < _fixedX; x++)
        {
            _data[x] = new double[_currentY, _currentZ];
            _data[x] = Array2DInitializerWithNumber.Initialize(_data[x], UNINITIALIZED_VALUE);
        }
    }

    public static Dynamic3DArray CreateWithExponentialExpansion(int fixedX, int initialY = INITIAL_DIMENSION_SIZE,
        int initialZ = INITIAL_DIMENSION_SIZE, double growthFactor = 2.0)
    {
        return new Dynamic3DArray(fixedX, initialY, initialZ,
            new ExponentialExpansionStrategy(growthFactor));
    }

    public static Dynamic3DArray CreateWithFixedIncrement(int fixedX, int initialY = INITIAL_DIMENSION_SIZE,
        int initialZ = INITIAL_DIMENSION_SIZE, int increment = 32)
    {
        return new Dynamic3DArray(fixedX, initialY, initialZ,
            new FixedIncrementExpansionStrategy(increment));
    }



    public double this[int x, int y, int z]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            double value = _data[x][y, z];

            if (double.IsNaN(value))
                ThrowNotInitialized(x, y, z);

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            EnsureCapacity(y, z);

            _data[x][y, z] = value;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnsureCapacity(int y, int z)
    {
        if (y >= _currentY || z >= _currentZ)
        {
            ExpandArray(y, z);
        }
    }

    private void ExpandArray(int requiredY, int requiredZ)
    {
        var (newY, newZ) = _expansionStrategy.CalculateNewDimensions(
            _currentY, _currentZ, requiredY, requiredZ);

        ResizeData(newY, newZ);
    }

    private void ResizeData(int newY, int newZ)
    {
        for (int x = 0; x < _fixedX; x++)
        {
            var newSlice = new double[newY, newZ];

            newSlice = Array2DInitializerWithNumber.Initialize(newSlice, UNINITIALIZED_VALUE);

            int copyY = Math.Min(_currentY, newY);
            int copyZ = Math.Min(_currentZ, newZ);

            for (int y = 0; y < copyY; y++)
            {
                Array.Copy(_data[x], y * _currentZ, newSlice, y * newZ, copyZ);
            }

            _data[x] = newSlice;
        }

        _currentY = newY;
        _currentZ = newZ;
    }


    public bool IsInitialized(int x, int y, int z)
    {
        if (x < 0 || x >= _fixedX || y < 0 || y >= _currentY || z < 0 || z >= _currentZ)
            return false;

        return !double.IsNaN(_data[x][y, z]);
    }

    public void Clear()
    {
        for (int x = 0; x < _fixedX; x++)
        {
            _data[x] = Array2DInitializerWithNumber.Initialize(_data[x], UNINITIALIZED_VALUE);
        }
    }

    public (int FixedX, int CurrentY, int CurrentZ) GetDimensions()
    {
        return (_fixedX, _currentY, _currentZ);
    }    

    public object Clone()
    {
        var (fixedX, currentY, currentZ) = GetDimensions();
        var clone = new Dynamic3DArray(fixedX, currentY, currentZ);

        for (int x = 0; x < fixedX; x++)
        {
            for (int y = 0; y < currentY; y++)
            {
                for (int z = 0; z < currentZ; z++)
                {
                    if (IsInitialized(x, y, z))
                    {
                        clone[x, y, z] = this[x, y, z];
                    }
                }
            }
        }

        return clone;
    }



    [MethodImpl(MethodImplOptions.NoInlining)]
    private void ThrowNotInitialized(int x, int y, int z)
    {
        throw new UninitializedElementException($"Неинициализованное значение по адресу: {x}, {y}, {z}");
    }
}

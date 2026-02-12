using Core.Domain.Common;
using Core.Domain.Limited_Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Grid.ValueObjects;

internal class LastKArray : ValueObject
{
    private LimitedDouble[][] _data;
    private readonly LimitedDouble _defaultValue;
    private readonly int _constParam;
    private int _sizeN;

    private static readonly int[] _sizes = { 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768, 65536 };

    public LastKArray(int constParam, LimitedDouble defaultValue)
    {
        _constParam = constParam;
        _defaultValue = defaultValue;
        _sizeN = 128;

        _data = new LimitedDouble[constParam][];

        for (int i = 0; i < constParam; i++)
        {
            var row = new LimitedDouble[128];
            if (defaultValue != null)
            {
                Array.Fill(row, defaultValue);
            }
            _data[i] = row;
        }
    }

    public LimitedDouble this[int p, int n]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _data[p][n];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            if (n >= _sizeN)
            {
                Grow(n);
            }
            _data[p][n] = value;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Grow(int n)
    {
        int newSize = _sizes[0];
        for (int i = 0; i < _sizes.Length; i++)
        {
            if (_sizes[i] > n)
            {
                newSize = _sizes[i];
                break;
            }
        }

        for (int i = 0; i < _constParam; i++)
        {
            Array.Resize(ref _data[i], newSize);

            if (_defaultValue != null && _sizeN < newSize)
            {
                Array.Fill(_data[i], _defaultValue, _sizeN, newSize - _sizeN);
            }
        }

        _sizeN = newSize;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        foreach (var itemPN in _data)
        {
            foreach (var item in itemPN)
            {
                yield return item;
            }
        }
    }
}

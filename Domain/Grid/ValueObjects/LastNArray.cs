using Core.Domain.Common;
using Core.Domain.Limited_Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Grid.ValueObjects;

internal class LastNArray : ValueObject
{
    private readonly LimitedDouble[] _data;
    public int SizeP { get; }

    public LastNArray(int constParam, LimitedDouble defaultValue)
    {
        SizeP = constParam;
        _data = new LimitedDouble[constParam];

        if (defaultValue != null)
        {
            Array.Fill(_data, defaultValue);
        }
    }

    public LimitedDouble this[int p]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _data[p];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _data[p] = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        foreach (var item in _data)
        {
            yield return item;
        }
    }
}

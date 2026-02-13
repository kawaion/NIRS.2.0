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

    private LastNArray(int countParams, LimitedDouble defaultValue)
    {
        SizeP = countParams;
        _data = new LimitedDouble[countParams];

        Array.Fill(_data, defaultValue);
    }
    public static LastNArray Create(int countParams, LimitedDouble defaultValue)
    {
        Validate(countParams);

        var instance = new LastNArray(countParams, defaultValue);

        return instance;
    }
    private static void Validate(int countParams)
    {
        if (countParams <= 0)
            throw new Exception("количство параметров должно быть больше 0");
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

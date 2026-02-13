using Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Limited_Double;

internal sealed class LimitedDouble : ValueObject
{
    private readonly int _scaled;

    public LimitedDouble(double value) => _scaled = (int)Math.Round(value * 2);
    public LimitedDouble(int value) => _scaled = value * 2;
    private LimitedDouble(int scaled, bool _) => _scaled = scaled; 

    public bool IsHalf => (_scaled & 1) == 1;
    public bool IsInt => (_scaled & 1) == 0;

    public int GetInt() => _scaled >> 1; 
    public double GetDouble() => _scaled * 0.5d;

    public LimitedDouble Copy()
    {
        return new(_scaled, false);
    }
    private static LimitedDouble FromScaled(int scaled) => new(scaled, false);


    public static LimitedDouble operator +(LimitedDouble a, LimitedDouble b)
        => FromScaled(a._scaled + b._scaled);

    public static LimitedDouble operator -(LimitedDouble a, LimitedDouble b)
        => FromScaled(a._scaled - b._scaled);

    public static bool operator >(LimitedDouble a, LimitedDouble b)
        => a._scaled > b._scaled;
    public static bool operator <(LimitedDouble a, LimitedDouble b)
        => a._scaled < b._scaled;

    public static bool operator ==(LimitedDouble a, LimitedDouble b)
        => a._scaled == b._scaled;
    public static bool operator !=(LimitedDouble a, LimitedDouble b)
        => a._scaled != b._scaled;

    public static implicit operator LimitedDouble(double value) => new(value);
    public static implicit operator LimitedDouble(int value) => new(value);

    public static explicit operator double(LimitedDouble value) => value.GetDouble();
    public static explicit operator int(LimitedDouble value) => value.GetInt();    
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return _scaled;
    }
    public override string ToString() => GetDouble().ToString("F1");
}

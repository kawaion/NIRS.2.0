using Core.Domain.Common;
using Core.Domain.interfaces;
using Core.Domain.Points.ValueObjects;

namespace Core.Domain.Physical.ValueObjects;

internal class СannonRadiusInterpolator : ValueObject
{
    private IInterpolator _interpolator;
    private СannonRadiusInterpolator(IInterpolator interpolator) 
    {
        _interpolator = interpolator;
    }
    public static СannonRadiusInterpolator Create(IInterpolator interpolator)
    {
        var instance = new СannonRadiusInterpolator(interpolator);
        return instance;
    }
    public double GetRadius(double x)
    {
        return _interpolator.Interpolate(x);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return _interpolator;
    }
}

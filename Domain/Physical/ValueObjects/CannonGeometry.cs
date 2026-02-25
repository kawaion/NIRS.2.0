using Core.Domain.Common;
using Core.Domain.interfaces;
using Core.Domain.Points.ValueObjects;
using System.Net;

namespace Core.Domain.Physical.ValueObjects;

internal class CannonGeometry : ValueObject
{
    private СannonRadiusInterpolator _cannonRadiusInterpolator;
    private CannonVolumeInterpolator _cannonVolumeInterpolator;

    private CannonGeometry(CannonContour cannonContour, IInterpolator interpolator)
    {
        _cannonRadiusInterpolator = СannonRadiusInterpolator.Create(interpolator);
        _cannonVolumeInterpolator = CannonVolumeInterpolator.Create(cannonContour, interpolator);
    }
    public static CannonGeometry Create(CannonContour bendPoints, IInterpolator interpolator)
    {
        var instance = new CannonGeometry(bendPoints, interpolator);
        return instance;
    }

    public double GetRadius(double x)
    {
        //if (_x < 0) return 0;

        return _cannonRadiusInterpolator.GetRadius(x);
    }
    public double GetDiameter(double x) 
    { 
        var r = GetRadius(x);
        return 2 * r; 
    }
    public double GetArea(double x)
    {
        var r = GetRadius(x);
        return Math.PI * Math.Pow(r, 2); 
    }
    public double GetVolume(double x)
    {
        return _cannonVolumeInterpolator.GetOccupiedVolume(x);
    }
    public double GetVolume(double x1, double x2) => Math.Abs(GetVolume(x1) - GetVolume(x2));
      

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return _cannonRadiusInterpolator;
    }
}

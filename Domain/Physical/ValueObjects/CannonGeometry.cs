using Core.Domain.Common;
using Core.Domain.Points.ValueObjects;

namespace Core.Domain.Physical.ValueObjects;

internal class CannonGeometry : ValueObject
{
    private СannonRadius _cannonRadius;
    private CannonVolume _cannonVolume;

    private CannonGeometry(OrderedList<BendPoint> bendPoints)
    {
        _cannonRadius = СannonRadius.Create(bendPoints);
        _cannonVolume = CannonVolume.Create(bendPoints);
    }
    public static CannonGeometry Create(OrderedList<BendPoint> bendPoints)
    {
        var instance = new CannonGeometry(bendPoints);
        return instance;
    }

    public double GetRadius(double x)
    {
        //if (_x < 0) return 0;

        return _cannonRadius.GetRadius(x);
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
        return _cannonVolume.GetOccupiedVolume(x);
    }
    public double GetVolume(double x1, double x2) => Math.Abs(GetVolume(x1) - GetVolume(x2));
      

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return _cannonRadius;
    }
}

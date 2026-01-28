using Core.Domain.Common;
using Core.Domain.Points.ValueObjects;

namespace Core.Domain.Physical.ValueObjects;

internal class СannonRadius : ValueObject
{
    private TableFunction _tableFunction;
    private СannonRadius(OrderedList<BendPoint> bendPoints) 
    {
        var points = bendPoints.ConvertToPoint2D();
        _tableFunction = TableFunction.Create(points);
    }
    public static СannonRadius Create(OrderedList<BendPoint> bendPoints)
    {
        var instance = new СannonRadius(bendPoints);
        return instance;
    }
    public double GetRadius(double x)
    {
        return _tableFunction.Interpolate(x);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return _tableFunction;
    }
}

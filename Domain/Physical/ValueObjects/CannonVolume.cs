using Core.Domain.Common;
using Core.Domain.Physical.Services;
using Core.Domain.Physical.Servises;
using Core.Domain.Points.ValueObjects;

namespace Core.Domain.Physical.ValueObjects;

internal class CannonVolume : ValueObject
{
    private IReadOnlyList<PointOfOccupiedVolumes> _volumes;
    private OrderedList<Point2D> _points;
    private TableFunction _tableFunction;
    private CannonVolume(OrderedList<BendPoint> bendPoints)
    {        
        _points = bendPoints.ConvertToPoint2D();
        _tableFunction = TableFunction.Create(_points);
        _volumes = OccupiedVolumeCalculatorUpToBendPoint.Calculate(bendPoints);
    }
    public static CannonVolume Create(OrderedList<BendPoint> bendPoints)
    {
        var instance = new CannonVolume(bendPoints);
        return instance;
    }
    public double GetOccupiedVolume(double x)
    {
        (var left, var right) = BinarySearchForAdjacentPoints.Search(_points, x);

        var RadiusX = _tableFunction.Interpolate(x);
        var bendPointX = BendPoint.Create(x, RadiusX);

        var volumeAtPoint = GetVolumeAtPoint(left);
        var segmentVolume = SegmentVolumeCalculator.CalculateSegmentVolume((BendPoint)left, bendPointX);
        
        return volumeAtPoint + segmentVolume;
    }

    private double GetVolumeAtPoint(Point2D left)
    {
        var pointOfOccupiedVolumes = _volumes.FirstOrDefault(v => (Point2D)v.BendPoint == left);
        return pointOfOccupiedVolumes.Volume;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}

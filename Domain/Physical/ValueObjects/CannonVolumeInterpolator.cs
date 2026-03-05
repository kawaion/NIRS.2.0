using Core.Domain.Common;
using Core.Domain.interfaces;
using Core.Domain.Physical.Services;
using Core.Domain.Physical.Servises;
using Core.Domain.Points.ValueObjects;
using System.Drawing;

namespace Core.Domain.Physical.ValueObjects;

internal class CannonVolumeInterpolator : ValueObject
{
    private OrderedList<Point2D> _cannonContour;
    private IInterpolator _interpolator;
    private readonly Dictionary<Point2D, double> _volumeCache;
    private CannonVolumeInterpolator(OrderedList<Point2D> cannonContour, IInterpolator interpolator)
    {        
        _interpolator = interpolator;
        _cannonContour = cannonContour;

        var volumes = OccupiedVolumeCalculatorUpToBendPoint.Calculate(cannonContour);
        _volumeCache = volumes.ToDictionary(v => v.BendPoint, v => v.Volume);
    }
    public static CannonVolumeInterpolator Create(OrderedList<Point2D> cannonContour, IInterpolator interpolator)
    {
        var instance = new CannonVolumeInterpolator(cannonContour, interpolator);
        return instance;
    }
    public double GetOccupiedVolume(double x)
    {
        (var left, var right) = BinarySearchForAdjacentBendPoints.Search(_cannonContour, x);

        var RadiusX = _interpolator.Interpolate(x);
        var bendPointX = Point2D.Create(x, RadiusX);

        var volumeAtPoint = GetVolumeAtPoint(left);
        var segmentVolume = SegmentVolumeCalculator.CalculateSegmentVolume(left, bendPointX);
        
        return volumeAtPoint + segmentVolume;
    }

    private double GetVolumeAtPoint(Point2D point)
    {
        return _volumeCache.TryGetValue(point, out var volume) ? volume : 0;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}

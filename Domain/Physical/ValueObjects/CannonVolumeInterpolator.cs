using Core.Domain.Common;
using Core.Domain.interfaces;
using Core.Domain.Physical.Services;
using Core.Domain.Physical.Servises;
using Core.Domain.Points.ValueObjects;

namespace Core.Domain.Physical.ValueObjects;

internal class CannonVolumeInterpolator : ValueObject
{
    private IReadOnlyList<PointOfOccupiedVolumes> _volumes;
    private CannonContour cannonContour;
    private IInterpolator _interpolator;
    private CannonVolumeInterpolator(CannonContour cannonContour, IInterpolator interpolator)
    {        
        _interpolator = interpolator;
        _volumes = OccupiedVolumeCalculatorUpToBendPoint.Calculate(cannonContour);
    }
    public static CannonVolumeInterpolator Create(CannonContour cannonContour, IInterpolator interpolator)
    {
        var instance = new CannonVolumeInterpolator(cannonContour);
        return instance;
    }
    public double GetOccupiedVolume(double x)
    {
        (var left, var right) = BinarySearchForAdjacentPoints.Search(_points, x);

        var RadiusX = _interpolator.Interpolate(x);
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

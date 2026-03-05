using Core.Domain.Common;
using Core.Domain.Physical.Servises;
using Core.Domain.Physical.ValueObjects;
using Core.Domain.Points.ValueObjects;

namespace Core.Domain.Physical.Services;

internal class OccupiedVolumeCalculatorUpToBendPoint
{
    public static IReadOnlyList<PointOfOccupiedVolumes> Calculate(OrderedList<Point2D> cannonContour)
    {
        List<PointOfOccupiedVolumes> volumes = new List<PointOfOccupiedVolumes>();

        double accumulatedVolume;
        accumulatedVolume = 0;
        volumes = AddVolumeForZeroPoint(cannonContour, volumes, accumulatedVolume);

        for (int i = 1; i < cannonContour.Count; i++)
        {
            var previousPoint = cannonContour[i - 1];
            var currentPoint = cannonContour[i];

            var volumeSegment = CalculateSegmentVolume(previousPoint, currentPoint);
            accumulatedVolume += volumeSegment;
            var pointOfOccupiedVolumes = CreateVolumePoint(cannonContour[i], accumulatedVolume);
            volumes.Add(pointOfOccupiedVolumes);
        }

        return volumes;
    }

    private static List<PointOfOccupiedVolumes> AddVolumeForZeroPoint(OrderedList<Point2D> cannonContour, List<PointOfOccupiedVolumes> volumes, double accumulatedVolume)
    {
        var pointOfOccupiedVolumes = CreateVolumePoint(cannonContour.First(), accumulatedVolume);
        volumes.Add(pointOfOccupiedVolumes);
        return volumes;
    }

    private static PointOfOccupiedVolumes CreateVolumePoint(Point2D point, double volume)
    {
        return PointOfOccupiedVolumes.Create(point, volume);
    }
    private static double CalculateSegmentVolume(Point2D start, Point2D end)
    {
        return SegmentVolumeCalculator.CalculateSegmentVolume(start, end);
    }
}

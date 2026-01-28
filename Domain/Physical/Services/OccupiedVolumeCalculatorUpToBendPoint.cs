using Core.Domain.Common;
using Core.Domain.Physical.Servises;
using Core.Domain.Points.ValueObjects;

namespace Core.Domain.Physical.Services;

internal class OccupiedVolumeCalculatorUpToBendPoint
{
    public static IReadOnlyList<PointOfOccupiedVolumes> Calculate(OrderedList<BendPoint> bendPoints)
    {
        List<PointOfOccupiedVolumes> volumes = new List<PointOfOccupiedVolumes>();

        double accumulatedVolume;
        accumulatedVolume = 0;
        volumes = AddVolumeForZeroPoint(bendPoints, volumes, accumulatedVolume);

        for (int i = 1; i < bendPoints.Count; i++)
        {
            var previousPoint = bendPoints[i - 1];
            var currentPoint = bendPoints[i];

            var volumeSegment = CalculateSegmentVolume(previousPoint, currentPoint);
            accumulatedVolume += volumeSegment;
            var pointOfOccupiedVolumes = CreateVolumePoint(bendPoints[i], accumulatedVolume);
            volumes.Add(pointOfOccupiedVolumes);
        }

        return volumes;
    }

    private static List<PointOfOccupiedVolumes> AddVolumeForZeroPoint(OrderedList<BendPoint> bendPoints, List<PointOfOccupiedVolumes> volumes, double accumulatedVolume)
    {
        var pointOfOccupiedVolumes = CreateVolumePoint(bendPoints.First(), accumulatedVolume);
        volumes.Add(pointOfOccupiedVolumes);
        return volumes;
    }

    private static PointOfOccupiedVolumes CreateVolumePoint(BendPoint point, double volume)
    {
        return PointOfOccupiedVolumes.Create(point, volume);
    }
    private static double CalculateSegmentVolume(BendPoint start, BendPoint end)
    {
        return SegmentVolumeCalculator.CalculateSegmentVolume(start, end);
    }
}

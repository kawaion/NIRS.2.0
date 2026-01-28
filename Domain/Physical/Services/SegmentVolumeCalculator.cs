using Core.Domain.Points.ValueObjects;

namespace Core.Domain.Physical.Servises;

internal static class SegmentVolumeCalculator
{
    public static double CalculateSegmentVolume(BendPoint start, BendPoint end)
    {
        (var h, var radiusStart, var radiusEnd) = GetBarrelSegmentParameters(start, end);
        double volumeSegment = CalculateTruncatedConeVolume(h, radiusStart, radiusEnd);
        return volumeSegment;
    }
    private static double CalculateTruncatedConeVolume(double h, double r1, double r2)
    {
        return 1.0 / 3 * Math.PI * h * (Math.Pow(r1, 2) + Math.Pow(r2, 2) + r1 * r2);
    }
    private static (double h, double r1, double r2) GetBarrelSegmentParameters(BendPoint p1, BendPoint p2)
    {
        double h = Math.Abs(p2.DistanceFromBottom - p1.DistanceFromBottom);
        double r1 = p1.Radius;
        double r2 = p2.Radius;

        return (h, r1, r2);
    }
}

using Core.Domain.Common;
using Core.Domain.Physical.ValueObjects;
using Core.Domain.Points.ValueObjects;

namespace Core.Domain.Physical.Services;

internal static class BinarySearchForAdjacentPoints
{
    public static (BendPoint, BendPoint) Search(IReadOnlyList<BendPoint> points, double x)
    {
        ValidateX(points, x); 

        int left = 0;
        int right = points.Count - 1;

        while (right - left > 1)
        {
        int mid = (left + right) / 2;
        if (points[mid].DistanceFromBottom<x)
            left = mid;
        else
            right = mid;
        }

        return (points[left], points[right]);
    }
    private static void ValidateX(IReadOnlyList<BendPoint> points, double x)
    {
        if (points[0].DistanceFromBottom > x || points[^1].DistanceFromBottom < x)
            throw new ArgumentOutOfRangeException(
                nameof(x),
                $"X={x} is out of range [{points[0].DistanceFromBottom}, {points[^1].DistanceFromBottom}]");
    }
}

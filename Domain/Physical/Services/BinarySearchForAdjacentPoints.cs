using Core.Domain.Common;
using Core.Domain.Points.ValueObjects;

namespace Core.Domain.Physical.Services;

internal static class BinarySearchForAdjacentPoints
{
    public static (Point2D,Point2D) Search(OrderedList<Point2D> points, double x)
    {
        ValidateX(points, x); 

        int left = 0;
        int right = points.Count - 1;

        while (right - left > 1)
        {
        int mid = (left + right) / 2;
        if (points[mid].X<x)
            left = mid;
        else
            right = mid;
        }

        return (points[left], points[right]);
    }
    private static void ValidateX(OrderedList<Point2D> points, double x)
    {
        if(points[0].X > x || points[^1].X < x)
        throw new ArgumentOutOfRangeException(
            nameof(x),
            $"X={x} is out of range [{points[0].X}, {points[^1].X}]");
    }
}

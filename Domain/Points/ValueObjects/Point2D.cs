using Core.Domain.Common;

namespace Core.Domain.Points.ValueObjects;

internal class Point2D : ValueObject
{
    public double X { get; }
    public double Y { get; }

    public Point2D(double x, double y)
    {
        X = x;
        Y = y;
    }
    public static Point2D operator +(Point2D a, Point2D b) =>
        new Point2D(a.X + b.X, a.Y + b.Y);

    public static Point2D operator -(Point2D a, Point2D b) =>
        new Point2D(a.X - b.X, a.Y - b.Y);

    public double DistanceTo(Point2D other)
    {
        double dx = X - other.X;
        double dy = Y - other.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    public override string ToString() => $"({X}, {Y})";
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return X;
        yield return Y;
    }
}

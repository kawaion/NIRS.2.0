using Core.Domain.Common;
using Core.Domain.Points.ValueObjects;
using FluentValidation;

namespace Core.Domain.Physical.ValueObjects;

internal sealed class BendPoint : ValueObject
{
    private readonly Point2D _point;
    public double DistanceFromBottom { get; }
    public double Radius { get; }

    private BendPoint(double distanceFromBottom, double radius)
    {
        _point = Point2D.Create(distanceFromBottom, radius);
        DistanceFromBottom = distanceFromBottom;
        Radius = radius;
    }
    private BendPoint(Point2D point)
    {
        _point = point;
        DistanceFromBottom = point.X;
        Radius = point.Y;
    }
    public static BendPoint Create(double distanceFromBottom, double radius)
    {
        var instance = new BendPoint(distanceFromBottom, radius);
        _validator.ValidateAndThrow(instance);
        return instance;
    }

    public static BendPoint Create(Point2D point)
    {
        var instance = new BendPoint(point);
        _validator.ValidateAndThrow(instance);
        return instance;
    }

    public static implicit operator Point2D(BendPoint bendPoint) => bendPoint._point;

    public static explicit operator BendPoint(Point2D point) => Create(point);
        
    public class Validator : AbstractValidator<BendPoint>
    {
        public Validator()
        {
            RuleFor(x => x.DistanceFromBottom)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Distance must be non-negative");

            RuleFor(x => x.Radius)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Radius must be non-negative");
        }
    }
    private static readonly Validator _validator = new Validator();
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return DistanceFromBottom;
        yield return Radius;
    }
}


internal static class ConverterToPoint2D
{
    public static IReadOnlyList<Point2D> ConvertToPoint2D(this IReadOnlyList<BendPoint> bendPoints)
    {
        return bendPoints.Select(bp => (Point2D)bp).ToList();
    }
    public static List<Point2D> ConvertToPoint2D(this List<BendPoint> bendPoints)
    {
        return bendPoints.Select(bp => (Point2D)bp).ToList();
    }
    public static OrderedList<Point2D> ConvertToPoint2D(this OrderedList<BendPoint> bendPoints)
    {
        var points = bendPoints.Select(bp => (Point2D)bp).ToList();
        return OrderedList<Point2D>.Create(points, (p) => p.X);
    }
}

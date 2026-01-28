using Core.Domain.Common;
using Core.Domain.Physical.Services;
using FluentValidation;

namespace Core.Domain.Points.ValueObjects;

internal class TableFunction : ValueObject
{
    private OrderedList<Point2D> _points;

    private double minX;
    private double maxX;
    private TableFunction(OrderedList<Point2D> points)
    {
        _points = points;

        minX = _points.First().X;
        maxX = _points.Last().X;
    }
    public static TableFunction Create(OrderedList<Point2D> points)
    {
        var instance = new TableFunction(points);
        _validator.ValidateAndThrow(instance);
        return instance;
    }



    public double Interpolate(double x)
    {
        ValidateXValue(x);

        var (left, right) = BinarySearchForAdjacentPoints.Search(_points, x);
        EquationOfLine equationOfLine = EquationOfLine.Create(left, right);
        return equationOfLine.GetY(x);
    }    
    private void ValidateXValue(double x)
    {
        if (double.IsNaN(x))
            throw new ArgumentException("X cannot be NaN", nameof(x));

        if (double.IsInfinity(x))
            throw new ArgumentException("X cannot be Infinity", nameof(x));

        if (x < minX)
            throw new ArgumentOutOfRangeException(
                nameof(x), x,
                $"X ({x}) cannot be less than minimum X ({minX})");

        if (x > maxX)
            throw new ArgumentOutOfRangeException(
                nameof(x), x,
                $"X ({x}) cannot be greater than maximum X ({maxX})");
    }

    public class Validator : AbstractValidator<TableFunction>
    {
        public Validator()
        {
            RuleFor(x => x._points)
                .Must(points => points.Count >= 2)
                .WithMessage("At least two points are required for interpolation");
        }
    }
    private static readonly Validator _validator = new Validator();   
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        foreach (var point in _points)
            yield return point;
    }
}

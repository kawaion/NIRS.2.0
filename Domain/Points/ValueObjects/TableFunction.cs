using Core.Domain.Common;
using Core.Domain.interfaces;
using Core.Domain.Physical.Services;
using FluentValidation;

namespace Core.Domain.Points.ValueObjects;

internal class TableFunction : ValueObject, IInterpolator
{
    private OrderedList<Point2D> _points;

    private Dictionary<(Point2D, Point2D), EquationOfLine> equations;

    private double minX;
    private double maxX;
    private TableFunction(OrderedList<Point2D> points)
    {
        _points = points;

        minX = _points.First().X;
        maxX = _points.Last().X;

        equations = InitializeEquations(points);
    }
    public static TableFunction Create(OrderedList<Point2D> points)
    {
        var instance = new TableFunction(points);
        _validator.ValidateAndThrow(instance);
        return instance;
    }

    private Dictionary<(Point2D, Point2D), EquationOfLine> InitializeEquations(OrderedList<Point2D> points)
    {
        var equations = new Dictionary<(Point2D, Point2D), EquationOfLine>();
        for (int i=0; i<points.Count-1; i++)
        {
            var left = points[i];
            var right = points[i+1];
            EquationOfLine equationOfLine = EquationOfLine.Create(left, right);

            equations.Add((left,right), equationOfLine);
        }
        return equations;
    }

    public double Interpolate(double x)
    {
        ValidateXValue(x);

        var (left, right) = BinarySearchForAdjacentPoints.Search(_points, x);
        return equations[(left, right)].Interpolate(x);
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

    private class Validator : AbstractValidator<TableFunction>
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

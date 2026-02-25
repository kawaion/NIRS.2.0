using Core.Domain.Common;
using Core.Domain.interfaces;
using FluentValidation;
using System.Transactions;

namespace Core.Domain.Points.ValueObjects;

internal class EquationOfLine : ValueObject, IInterpolator
{
    private readonly Point2D _p1;
    private readonly Point2D _p2;

    //private double dx;
    //private double dy;

    private EquationOfLine(Point2D p1, Point2D p2)
    {
        _p1 = p1;
        _p2 = p2;

        //dx = _p2.X - _p1.X;
        //dy = _p2.Y - _p1.Y;
    }

    public static EquationOfLine Create(Point2D p1, Point2D p2)
    {
        var instance = new EquationOfLine(p1, p2);
        _validator.ValidateAndThrow(instance);
        return instance;
    }
    public double Interpolate(double x) => (x - _p1.X) * (_p2.Y - _p1.Y) / (_p2.X - _p1.X) + _p1.Y;

    public class Validator : AbstractValidator<EquationOfLine>
    {
        public Validator()
        {
            RuleFor(x => x._p1).NotNull();
            RuleFor(x => x._p2).NotNull();

            RuleFor(x => x)
                .Must(line => Math.Abs(line._p2.X - line._p1.X) > 0.000001)
                .WithMessage("Points cannot have the same X coordinate");
        }
    }
    private static readonly Validator _validator = new Validator();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return _p1;
        yield return _p2;
    }
}

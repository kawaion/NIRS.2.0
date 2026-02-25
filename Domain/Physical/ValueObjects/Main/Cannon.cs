using Core.Domain.Common;
using Core.Domain.interfaces;
using Core.Domain.Physical.Interfaces;
using FluentValidation;

namespace Core.Domain.Physical.ValueObjects.Main;

internal sealed class Cannon : ValueObject, ICannon
{
    private readonly CannonGeometry _cannonGeometry;
    private readonly CannonContour _cannonContour;
    public double ChamberLength { get; }
    public double Length { get; }

    public double Skn { get; }
    public double Wkm { get; }

    private Cannon(CannonContour cannonContour, double ChamberLength, IInterpolator interpolator)
    {
        _cannonContour = cannonContour;
        _cannonGeometry = CannonGeometry.Create(cannonContour, interpolator);
        this.ChamberLength = ChamberLength;        
        Length = _cannonContour.Last().DistanceFromBottom;
        Skn = R(ChamberLength);
        Wkm = W(ChamberLength);
    }
    public static Cannon Create(CannonContour cannonContour, double chamberLength, IInterpolator interpolator)
    {
        ValidateInputParameters(cannonContour, chamberLength, interpolator);

        var instance = new Cannon(cannonContour, chamberLength, interpolator);

        _validator.ValidateAndThrow(instance);

        return instance;
    }

    public double R(double x) => _cannonGeometry.GetRadius(x);    
    public double S(double x) => _cannonGeometry.GetArea(x);
    public double W(double x) => _cannonGeometry.GetVolume(x);




    public class Validator : AbstractValidator<Cannon>
    {
        public Validator()
        {
            RuleFor(x => x.ChamberLength)
                .GreaterThan(0)
                .WithMessage("Конец каморы должен быть положительным");

            RuleFor(x => x.ChamberLength)
                .LessThanOrEqualTo(x => x.Length)
                .WithMessage("Конец каморы должен быть в пределах длины ствола");
        }
    }
    private static readonly Validator _validator = new Validator();
    private static void ValidateInputParameters(CannonContour cannonContour, double endChamber, IInterpolator interpolator)
    {
        string ex = "";
        if (endChamber <= 0)
            ex+= $"Конец каморы ({endChamber}) должен быть положительным" +'\n';
        for (var i = 0; i < cannonContour.Count; i++)
        {
            var point = cannonContour[i];
            if (Math.Abs(point.Radius - interpolator.Interpolate(point.DistanceFromBottom)) < 0.000001)
            {
                ex += "интерполятор не соответсвует точкам контура";
                break;
            }
        }

        if(ex!="")
            throw new Exception(ex);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return _cannonContour;
        yield return ChamberLength;
    }
}

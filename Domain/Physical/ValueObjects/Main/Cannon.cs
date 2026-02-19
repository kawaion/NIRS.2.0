using Core.Domain.Common;
using Core.Domain.Physical.Interfaces;
using Core.Domain.Points.ValueObjects;
using FluentValidation;

namespace Core.Domain.Physical.ValueObjects.Main;

internal sealed class Cannon : ValueObject, ICannon
{
    private readonly CannonGeometry _cannonGeometry;
    private readonly OrderedList<BendPoint> _bendPoints;
    public double ChamberLength { get; }
    public double Length { get; }

    public double Skn { get; }
    public double Wkm { get; }

    private Cannon(OrderedList<BendPoint> bendPoints, double ChamberLength)
    {
        _bendPoints = bendPoints;
        _cannonGeometry = CannonGeometry.Create(_bendPoints);
        this.ChamberLength = ChamberLength;        
        Length = _bendPoints.Last().DistanceFromBottom;
        Skn = R(ChamberLength);
        Wkm = W(ChamberLength);
    }
    public static Cannon Create(List<BendPoint> bendingPoints, double chamberLength)
    {
        ValidateInputParameters(bendingPoints, chamberLength);

        var orderedList = OrderedList<BendPoint>.Create(bendingPoints, p=>p.DistanceFromBottom);
        var instance = new Cannon(orderedList, chamberLength);

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
    private static void ValidateInputParameters(List<BendPoint> bendingPoints, double endChamber)
    {
        if (bendingPoints == null)
            throw new ArgumentNullException(nameof(bendingPoints));

        if (!bendingPoints.Any())
            throw new ArgumentException("Должна быть хотя бы одна точка изгиба", nameof(bendingPoints));

        if (endChamber <= 0)
            throw new ArgumentException("Конец каморы должен быть положительным", nameof(endChamber));
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        foreach (var point in _bendPoints)
        {
            yield return point;
        }
        yield return ChamberLength;
    }
}

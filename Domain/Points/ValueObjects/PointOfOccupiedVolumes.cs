using Core.Domain.Common;
using FluentValidation;

namespace Core.Domain.Points.ValueObjects;

internal class PointOfOccupiedVolumes : ValueObject
{
    public BendPoint BendPoint {  get; }
    public double Volume {  get; }

    private PointOfOccupiedVolumes(BendPoint bendPoint, double volume)
    {
        BendPoint = bendPoint;
        Volume = volume;
    }
    public static PointOfOccupiedVolumes Create(BendPoint bendPoint, double volume)
    { 
        var instance = new PointOfOccupiedVolumes(bendPoint, volume);
        _validator.ValidateAndThrow(instance);
        return instance;
    }
    private class Validator : AbstractValidator<PointOfOccupiedVolumes>
    {
        public Validator()
        {
            RuleFor(x => x.Volume)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Volume must be non-negative");
        }
    }
    private static readonly Validator _validator = new Validator();
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return BendPoint;
        yield return Volume;
    }
}

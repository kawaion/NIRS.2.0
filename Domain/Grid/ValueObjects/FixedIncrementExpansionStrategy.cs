using Core.Domain.Common;
using Core.Domain.Grid.Interfaces;
using Core.Domain.Points.ValueObjects;
using FluentValidation;

namespace Core.Domain.Grid.ValueObjects;

internal class FixedIncrementExpansionStrategy : ValueObject, IArrayExpansionStrategy
{
    private readonly int _increment;

    private FixedIncrementExpansionStrategy(int increment)
    {
        if (increment <= 0)
            throw new ArgumentException("Приращение должно быть положительным", nameof(increment));

        _increment = increment;
    }
    public static FixedIncrementExpansionStrategy Create(int increment = 32)
    {
        var instance = new FixedIncrementExpansionStrategy(increment);
        _validator.ValidateAndThrow(instance);
        return instance;
    }
    public (int newY, int newZ) CalculateNewDimensions(int currentY, int currentZ, int requiredY, int requiredZ)
    {
        int newY = currentY;
        int newZ = currentZ;

        while (requiredY >= newY)
            newY += _increment;

        while (requiredZ >= newZ)
            newZ += _increment;

        return (newY, newZ);
    }

    public class Validator : AbstractValidator<FixedIncrementExpansionStrategy>
    {
        public Validator()
        {
            RuleFor(x => x._increment)
                .GreaterThan(0)
                .WithMessage("Increment must be positive");
        }
    }
    private static readonly Validator _validator = new Validator();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return _increment;
    }
}

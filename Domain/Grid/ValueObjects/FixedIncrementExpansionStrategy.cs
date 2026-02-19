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
    public int CalculateNewDimension(int currentSize, int requiredSize)
    {
        int newSize = currentSize;

        while (requiredSize >= newSize)
        {
            newSize += _increment;
        }

        return newSize;
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

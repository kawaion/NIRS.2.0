using Core.Domain.Common;
using Core.Domain.Grid.Interfaces;
using FluentValidation;

namespace Core.Domain.Grid.ValueObjects;

internal class ExponentialExpansionStrategy : ValueObject, IArrayExpansionStrategy
{
    private readonly double _growthFactor;

    private ExponentialExpansionStrategy(double growthFactor, int minGrowth)
    {
        _growthFactor = growthFactor;
    }
    public static ExponentialExpansionStrategy Create(double growthFactor = 2.0, int minGrowth = 16)
    {
        var instance = new ExponentialExpansionStrategy(growthFactor, minGrowth);
        _validator.ValidateAndThrow(instance);
        return instance;
    }
    public int CalculateNewDimension(int currentSize, int requiredSize)
    {
        int newSize = currentSize;

        while (requiredSize >= newSize)
        {
            newSize *= (int)(newSize * _growthFactor);
        }

        return newSize;
    }

    public class Validator : AbstractValidator<ExponentialExpansionStrategy>
    {
        public Validator()
        {
            RuleFor(x => x._growthFactor)
                .GreaterThan(1)
                .WithMessage("GrowthFactor must be more 1");
        }
    }
    private static readonly Validator _validator = new Validator();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return _growthFactor;
    }
}

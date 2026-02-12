using Core.Domain.Common;
using Core.Domain.Grid.Interfaces;

namespace Core.Domain.Grid.ValueObjects;

internal class ExponentialExpansionStrategy : ValueObject, IArrayExpansionStrategy
{
    private readonly double _growthFactor;

    public ExponentialExpansionStrategy(double growthFactor = 2.0, int minGrowth = 16)
    {
        if (growthFactor <= 1.0)
            throw new ArgumentException("Коэффициент роста должен быть больше 1", nameof(growthFactor));

        _growthFactor = growthFactor;
    }

    public (int newY, int newZ) CalculateNewDimensions(int currentY, int currentZ, int requiredY, int requiredZ)
    {
        int newY = currentY;
        int newZ = currentZ;

        while (requiredY >= newY)
        {
            newY *= (int)(newY * _growthFactor);
        }

        while (requiredZ >= newZ)
        {
            newZ = (int)(newZ * _growthFactor);
        }

        return (newY, newZ);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return _growthFactor;
    }
}

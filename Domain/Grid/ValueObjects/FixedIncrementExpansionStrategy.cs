using Core.Domain.Common;
using Core.Domain.Grid.Interfaces;

namespace Core.Domain.Grid.ValueObjects;

internal class FixedIncrementExpansionStrategy : ValueObject, IArrayExpansionStrategy
{
    private readonly int _increment;

    public FixedIncrementExpansionStrategy(int increment = 32)
    {
        if (increment <= 0)
            throw new ArgumentException("Приращение должно быть положительным", nameof(increment));

        _increment = increment;
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

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return _increment;
    }
}

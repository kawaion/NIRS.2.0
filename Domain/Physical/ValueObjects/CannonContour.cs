using Core.Domain.Common;
using Core.Domain.interfaces;
using Core.Domain.Physical.ValueObjects.Main;
using FluentValidation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Physical.ValueObjects;

internal class CannonContour : ValueObject, IReadOnlyList<BendPoint>
{
    private OrderedList<BendPoint> _bendPoints { get; }

    private CannonContour(OrderedList<BendPoint> bendPoints)
    {
        _bendPoints = bendPoints;
    }

    public static CannonContour Create(OrderedList<BendPoint> bendPoints)
    {
        return new CannonContour(bendPoints);
    }

    // Явная реализация IReadOnlyList
    public BendPoint this[int index] => _bendPoints[index];
    public int Count => _bendPoints.Count;

    public IEnumerator<BendPoint> GetEnumerator() => _bendPoints.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // Дополнительные методы
    public BendPoint First() => _bendPoints.First();
    public BendPoint Last() => _bendPoints.Last();

    // Валидатор
    public class Validator : AbstractValidator<CannonContour>
    {
        public Validator()
        {
            RuleFor(x => x.Count)
                .GreaterThan(0)
                .WithMessage("Должна быть хотя бы одна точка изгиба");
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        foreach (var point in _bendPoints)
        {
            yield return point;
        }
    }
}
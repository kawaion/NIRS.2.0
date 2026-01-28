
namespace Core.Domain.Common;

using System.Collections;
using System.Collections.Immutable;

public sealed class OrderedList<T> : IReadOnlyList<T>
{
    private IReadOnlyList<T> _items;

    public T this[int index] => _items[index];
    public int Count => _items.Count;

    private OrderedList(ImmutableList<T> items)
    {
        _items = items;
    }
    private static OrderedList<T> Create(List<T> items, Func<T, T, bool> isInCorrectOrder)
    {
        if (items == null)
            throw new ArgumentNullException(nameof(items));

        if (isInCorrectOrder == null)
            throw new ArgumentNullException(nameof(isInCorrectOrder));

        ValidateOrdering(items, isInCorrectOrder);

        return new OrderedList<T>(items.ToImmutableList());
    }

    public static OrderedList<T> Create<TKey>(
        List<T> items,
        Func<T, TKey> keySelector)
        where TKey : IComparable<TKey>
    {
        if (keySelector == null)
            throw new ArgumentNullException(nameof(keySelector));

        Func<T, T, bool> orderingRule = (current, next) =>
            keySelector(current).CompareTo(keySelector(next)) <= 0;

        return Create(items, orderingRule);
    }

    private static void ValidateOrdering(
        IReadOnlyList<T> items,
        Func<T, T, bool> isInCorrectOrder)
    {
        if (items.Count < 2) return;

        for (int i = 0; i < items.Count - 1; i++)
        {
            if (!isInCorrectOrder(items[i], items[i + 1]))
            {
                throw new ArgumentException(
                    $"Items are not ordered correctly. " +
                    $"Elements at indices {i} and {i + 1} violate the ordering rule.");
            }
        }
    }
    public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IReadOnlyList<T> ToList() => _items;
}

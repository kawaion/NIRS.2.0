using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Domain.Common;

/// <summary>
/// Базовый класс для всех сущностей в системе
/// Обеспечивает уникальный идентификатор, сравнение и хеширование
/// </summary>
public abstract class Entity : IEquatable<Entity>
{
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// Уникальный идентификатор сущности
    /// </summary>
    public Guid Id { get; protected set; }


    /// <summary>
    /// События домена
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Конструктор по умолчанию (создает новый Guid)
    /// </summary>
    protected Entity()
    {
        Id = Guid.NewGuid();
    }
    // ==================== МЕТОДЫ ДЛЯ ДОМЕННЫХ СОБЫТИЙ ====================

    /// <summary>
    /// Добавить доменное событие
    /// </summary>
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Удалить доменное событие
    /// </summary>
    protected void RemoveDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    /// <summary>
    /// Очистить все доменные события
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    // ==================== РЕАЛИЗАЦИЯ IEquatable ====================

    /// <summary>
    /// Сравнение по идентификатору
    /// </summary>
    public bool Equals(Entity other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Entity)obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    // ==================== ОПЕРАТОРЫ СРАВНЕНИЯ ====================

    public static bool operator ==(Entity left, Entity right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(Entity left, Entity right)
    {
        return !(left == right);
    }

    // ==================== ДОПОЛНИТЕЛЬНЫЕ МЕТОДЫ ====================

    /// <summary>
    /// Проверка на равенство по идентификатору (безопасный вариант)
    /// </summary>
    public bool IsSameAs(Entity other)
    {
        return Id == other?.Id;
    }

    /// <summary>
    /// Создать копию с новым идентификатором
    /// </summary>
    public virtual T Clone<T>() where T : Entity
    {
        var clone = (T)MemberwiseClone();
        clone.Id = Guid.NewGuid();
        clone.ClearDomainEvents();
        return clone;
    }

    public override string ToString()
    {
        return $"{GetType().Name} [Id={Id}]";
    }
}

public interface IDomainEvent
{
}

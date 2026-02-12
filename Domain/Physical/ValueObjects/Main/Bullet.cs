using Core.Domain.Common;
using Core.Domain.Physical.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Physical.ValueObjects.Main;

internal class Bullet : ValueObject
{
    public double q { get; }

    private Bullet(double q)
    {
        this.q = q;
    }
    public static Bullet Create(double q)
    {
        var instance = new Bullet(q);

        _validator.ValidateAndThrow(instance);

        return instance;
    }
    private class Validator : AbstractValidator<Bullet>
    {
        public Validator()
        {
            RuleFor(x => x.q)
                .GreaterThan(0)
                .WithMessage("Масса должна должна быть положительной");
        }
    }
    private static readonly Validator _validator = new Validator();
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return q;
    }
}

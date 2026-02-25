using Core.Domain.Common;
using Core.Domain.Physical.Interfaces;
using Core.Domain.Physical.ValueObjects;
using Core.Domain.Physical.ValueObjects.Main;
using Core.Domain.Points.ValueObjects;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Physical.Entities;

internal class Gunpowder_12_7 : Gunpowder
{
    public override double Omega { get; }
    public override double Delta { get; }
    public override double S0 { get; }
    public override double LAMBDA0 { get; }

    public override double e1 { get; }
    private double d0 { get; }

    protected override IFunctionOfBurning FunctionOfBurning {  get; }

    private Gunpowder_12_7(double D0, double L0, double omega, double delta, double u1)
    {
        (d0, e1) = Initialised0e1(D0);
        FunctionOfBurning = FunctionOfBurning_12_7.Create(D0, d0, L0, e1, u1);

        S0 = InitialiseS0(D0, d0, L0);
        LAMBDA0 = InitialiseLAMBDA0(D0, d0, L0);

        Delta = delta;
        Omega = omega;
    }
    public static Gunpowder_12_7 Create(double D0, double L0, double omega, double delta, double u1)
    {
        var instance = new Gunpowder_12_7(D0, L0, omega, delta, u1);

        _validator.ValidateAndThrow(instance);

        return instance;
    }
    private (double d0, double e1) Initialised0e1(double D0)
    {
        double d0 = D0 / 11;
        double e1 = D0 / 11;
        return (d0, e1);
    }
    private double InitialiseS0(double D0, double d0, double L0)
    {
        var s0 = 2 * (Math.PI * Math.Pow(D0, 2) / 4 - 7 * Math.PI * Math.Pow(d0, 2) / 4) + Math.PI * D0 * L0 + 7 * Math.PI * d0 * L0;
        return s0;
    }
    private double InitialiseLAMBDA0(double D0, double d0, double L0)
    {
        var lambda0 = (Math.PI * Math.Pow(D0, 2) / 4 - 7 * Math.PI * Math.Pow(d0, 2) / 4) * L0;
        return lambda0;
    }

    private class Validator : AbstractValidator<Gunpowder_12_7>
    {
        public Validator()
        {
            RuleFor(x => x.Omega)
                .GreaterThan(0)
                .WithMessage("Масса пороха должна быть положительной");

            RuleFor(x => x.Delta)
                .GreaterThan(0)
                .WithMessage("Плотность пороха должна быть положительной");
        }
    }
    private static readonly Validator _validator = new Validator();
}

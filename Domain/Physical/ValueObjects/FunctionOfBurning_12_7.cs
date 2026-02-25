using Core.Domain.Common;
using Core.Domain.Physical.Entities;
using Core.Domain.Physical.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Physical.ValueObjects;

internal class FunctionOfBurning_12_7 : ValueObject, IFunctionOfBurning
{
    public double D0 { get; }
    public double d0 { get; }
    public double L0 { get; }
    public double e1 { get; }
    public double u1 { get; }

    private double _u1 { get; }
    private double _kappa { get; }
    private double _lamda { get; }
    private double _mu { get; }

    private double _psi_s { get; }

    private FunctionOfBurning_12_7(double D0, double d0, double L0, double e1, double u1)
    {
        this.D0 = D0;
        this.d0 = d0;
        this.L0 = L0;
        this.e1 = e1;
        this.u1 = u1;

        (double P, double Q, double beta) = InitialisePQBeta(D0, d0, L0, e1);
        (_kappa, _lamda, _mu) = InitialiseKappaLamdaMu(P, Q, beta);
        _psi_s = InitializePsi_s(_kappa, _lamda, _mu);

        _u1 = u1;
    }
    public static FunctionOfBurning_12_7 Create(double D0, double d0, double L0, double e1, double u1)
    {
        var instance = new FunctionOfBurning_12_7(D0, d0, L0, e1, u1);

        _validator.ValidateAndThrow(instance);

        return instance;
    }
    private (double kappa, double lamda, double mu) InitialiseKappaLamdaMu(double P, double Q, double beta)
    {
        double kappa = (Q + 2 * P) / Q * beta;
        double lamda = 2 * (3 - P) / (Q + 2 * P) * beta;
        double mu = -6 * Math.Pow(beta, 2) / (Q + 2 * P);
        return (kappa, lamda, mu);
    }
    private (double P, double Q, double Beta) InitialisePQBeta(double D0, double d0, double L0, double e1)
    {
        double P = (D0 + 7 * d0) / L0;
        double Q = (Math.Pow(D0, 2) - 7 * Math.Pow(d0, 2)) / Math.Pow(L0, 2);
        double beta = 2 * e1 / L0;
        return (P, Q, beta);
    }
    private double InitializePsi_s(double kappa, double lamda, double mu)
    {
        return kappa * (1 + lamda + mu);
    }
    


    public double Sigma(double psi, double z)
    {
        ValidateSigmaInputs(psi, z);

        var _z = z;
        if (IsPreDisintegrationPhase(_z))
            return 1.0 + 2.0 * _lamda * _z + 3.0 * _mu * Math.Pow(_z, 2);



        return (1.0 + 2.0 * _lamda + 3.0 * _mu)
            * Math.Sqrt(Math.Abs((1.0 - psi) / (1.0 - _psi_s)));
    }

    public double Uk(double p)
    {
        return _u1 * p;
    }

    /// <summary>
    /// Проверка, находится ли точка в до-распадной фазе
    /// </summary>
    public bool IsPreDisintegrationPhase(double z)
    {
        return z < 1.0;
    }
    private void ValidateSigmaInputs(double psi, double z)
    {
        if (psi < 0 || psi > 1)
            throw new ArgumentException("ψ должен быть в диапазоне [0, 1]");

        if (z < 0)
            throw new ArgumentException("z[0] должен быть неотрицательным");
    }
    private class Validator : AbstractValidator<FunctionOfBurning_12_7>
    {
        public Validator()
        {
            RuleFor(x => x.D0)
                .GreaterThan(0)
                .WithMessage("Наружный диаметр D0 должен быть положительным");

            RuleFor(x => x.d0)
                .GreaterThan(0)
                .LessThan(x => x.D0)
                .WithMessage("Диаметр канала d0 должен быть положительным и меньше D0");

            RuleFor(x => x.L0)
                .GreaterThan(0)
                .WithMessage("Длина L0 должна быть положительной");

            RuleFor(x => x.e1)
                .GreaterThan(0)
                .WithMessage("Толщина горящего свода e1 должна быть положительной");

            RuleFor(x => x.u1)
                .GreaterThan(0)
                .WithMessage("Коэффициент скорости горения u1 должен быть положительным");

            RuleFor(x => x._psi_s)
                .InclusiveBetween(0.01, 0.99)
                .WithMessage("ψ_s1 должен быть в пределах (0.01, 0.99)");
        }
    }
    private static readonly Validator _validator = new Validator();
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return _kappa;
        yield return _lamda;
        yield return _mu;
        yield return _u1;

    }
}

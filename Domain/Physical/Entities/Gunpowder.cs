using Core.Domain.Common;
using Core.Domain.Physical.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Physical.Entities;

internal abstract class Gunpowder : Entity, IGunpowder
{
    public abstract double Omega { get; }
    public abstract double Delta { get; }
    public abstract double S0 { get; }
    public abstract double LAMBDA0 { get; }
    public virtual double f { get; } = 1.0e6;
    public virtual double alpha { get; } = 1.0e-3;
    public virtual double kappa { get => 1.25; }
    public virtual double Theta { get => kappa - 1; }

    protected abstract IFunctionOfBurning FunctionOfBurning {  get; }

    public double Uk(double p)
    {
        return FunctionOfBurning.Uk(p);
    }

    public double Sigma(double psi, List<double> z)
    {
        return FunctionOfBurning.Sigma(psi, z);
    }
}

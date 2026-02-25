using Core.Domain.Enums;
using Core.Domain.Limited_Double;
using Core.Domain.Numerical_methods.SEL.Services;

namespace Core.Domain.Numerical_methods.SEL.Functions.Interfaces;

internal interface IHFunctionsProjectileCalculator
{

    public double H3(LimitedDouble N);

    public double H4(LimitedDouble N);

    public double H5(LimitedDouble N);

    public double HPsi(LimitedDouble N);
}

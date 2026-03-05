using Core.Domain.Grid.Interfaces;
using Core.Domain.Limited_Double;

namespace Core.Domain.Numerical_methods.SEL.Functions.Interfaces;

internal interface ICalculatorValuesInNodes
{
    public IGrid CalculateDynamic(IGrid g, LimitedDouble n, LimitedDouble k);
    public IGrid CalculateMixture(IGrid g, LimitedDouble n, LimitedDouble k);
}

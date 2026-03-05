using Core.Domain.Enums;
using Core.Domain.Grid.Interfaces;
using Core.Domain.Limited_Double;
using Core.Domain.Numerical_methods.SEL.Formulas.Interfaces;
using Core.Domain.Numerical_methods.SEL.Functions.Interfaces;

namespace Core.Domain.Numerical_methods.SEL.Functions.Services;

internal class BoundaryCalculator : IBoundaryCalculator
{
    private IBoundaryFormulas _bf;
    public BoundaryCalculator(IBoundaryFormulas boundaryFormulas)
    {
        _bf = boundaryFormulas;
    }

    public IGrid CalculateInLeft(IGrid g, LimitedDouble n, LimitedDouble k)
    {
        g.At(PN.dynamic_m, n, k).Set(_bf.dynamic_m0());
        g.At(PN.M, n, k).Set(_bf.M0());
        g.At(PN.v, n, k).Set(_bf.v0());
        g.At(PN.w, n, k).Set(_bf.w0());

        return g;
    }

    public IGrid CalculateInRight(IGrid g, LimitedDouble n, LimitedDouble k)
    {
        g.At(PN.dynamic_m, n, k).Set(_bf.dynamic_mK());
        g.At(PN.M, n, k).Set(_bf.MK());
        g.At(PN.v, n, k).Set(_bf.vK());
        g.At(PN.w, n, k).Set(_bf.wK());

        return g;
    }
}

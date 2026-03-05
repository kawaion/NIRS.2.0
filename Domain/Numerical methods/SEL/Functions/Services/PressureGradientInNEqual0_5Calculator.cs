using Core.Domain.Enums;
using Core.Domain.Grid.Interfaces;
using Core.Domain.Limited_Double;
using Core.Domain.Numerical_methods.SEL.Functions.Interfaces;
using Core.Domain.Numerical_methods.SEL.Helper.Services;
using Core.Domain.Numerical_methods.SEL.Interfaces;

using static Core.Domain.Limited_Double.LimitedDouble;

namespace Core.Domain.Numerical_methods.SEL.Functions.Services;

internal class PressureGradientInNEqual0_5Calculator : IPressureGradientCalculator
{
    private double _h;
    private IGrid g;
    private INumericalMethodSettings _settings;
    public PressureGradientInNEqual0_5Calculator(IGrid grid,
                                                 INumericalMethodSettings settings)
    {
        g = grid;
        _settings = settings;
        _h = settings.h;
    }

    public double dPStrokeDivdx(LimitedDouble n, LimitedDouble k)
    {
        var res = (PStroke(n, k + ld0_5) - PStroke(n, k - ld0_5)) / _h;
        return res;
    }
    private double PStroke(LimitedDouble n, LimitedDouble k)
    {
        (n, k) = OffseterNK.AppointAndOffset(n, ld0, k, +ld0_5);

        var p = g[PN.p, n, k + ld0_5];
        var res = p;

        return res;
    }
}

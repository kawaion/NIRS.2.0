using Core.Domain.Enums;
using Core.Domain.Grid.Interfaces;
using Core.Domain.Limited_Double;
using Core.Domain.Numerical_methods.SEL.Functions.Interfaces;
using Core.Domain.Numerical_methods.SEL.Interfaces;
using Core.Domain.Numerical_methods.SEL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

using static Core.Domain.Limited_Double.LimitedDouble;

namespace Core.Domain.Numerical_methods.SEL.Functions.Services;

internal class PressureGradientCalculator : IPressureGradientCalculator
{
    private double _h;
    private IGrid g;
    private INumericalMethodSettings _settings;
    private IPseudoViscosityMechanism _q;
    public PressureGradientCalculator(IGrid grid,
                                      INumericalMethodSettings settings,
                                      IPseudoViscosityMechanism q)
    {
        g = grid;
        _settings = settings;
        _q = q;
        _h = settings.h;
    }

    public double dPStrokeDivdx(LimitedDouble n, LimitedDouble k)
    {
        var res = (PStroke(n, k + ld0_5) - PStroke(n, k - ld0_5)) / _h;
        return res;
    }
    private double PStroke(LimitedDouble N, LimitedDouble K)
    {
        (var n, var k) = OffseterNK.AppointAndOffset(N, ld0, K, +ld0_5);

        var p = g[PN.p, n, k + ld0_5];
        var res = p + _q.Calculate(n - ld0_5, k + ld0_5);

        return res;
    }
}

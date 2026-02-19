using Core.Domain.Enums;
using Core.Domain.Limited_Double;
using static Core.Domain.Limited_Double.LimitedDouble;
using Core.Domain.Numerical_methods.SEL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Numerical_methods.SEL.Functions.Interfaces;
using Core.Domain.Grid.Interfaces;

namespace Core.Domain.Numerical_methods.SEL.Functions.Services;

internal static class BlurryP
{
    public static double PStroke(IGrid g, LimitedDouble N, LimitedDouble K)
    {
        (var n, var k) = OffseterNK.AppointAndOffset(N, ld0, K, + ld0_5);
        var p = g[PN.p, n, k + ld0_5];
        var res = p + PseudoViscosityMechanism.q(g, n - ld0_5, k + ld0_5);

        return res;
    }
}

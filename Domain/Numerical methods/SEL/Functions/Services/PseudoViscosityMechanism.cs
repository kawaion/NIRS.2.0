using Core.Domain.Enums;
using Core.Domain.Limited_Double;
using static Core.Domain.Limited_Double.LimitedDouble;
using Core.Domain.Numerical_methods.SEL.Interfaces;
using Core.Domain.Numerical_methods.SEL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Grid.Interfaces;
using Core.Domain.Numerical_methods.SEL.Functions.Interfaces;

namespace Core.Domain.Numerical_methods.SEL.Functions.Services;

internal class PseudoViscosityMechanism : IPseudoViscosityMechanism
{
    private double _h;
    private INumericalMethodSettings _settings;
    private IWaypointCalculator wc;
    private IGrid g;
    public PseudoViscosityMechanism(IGrid grid, IWaypointCalculator waypointCalculator, INumericalMethodSettings settings)
    {
        g = grid;
        wc = waypointCalculator;
        _settings = settings;
        _h = settings.h;
    }

    public double Calculate( LimitedDouble N, LimitedDouble K)
    {
        (var n, var k) = OffseterNK.AppointAndOffset(N, + ld0_5, K, - ld0_5);

        double NablaV = wc.Nabla(PN.v, n + ld0_5, k - ld0_5);
        if (NablaV < 0)
        {
            var res = Math.Pow(_h, 2) * g[PN.rho, n + ld1, k - ld0_5] * Math.Pow(NablaV, 2);
            return res;
        }
        else
            return 0;
    }
}

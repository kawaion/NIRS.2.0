using Core.Domain.Enums;
using Core.Domain.Grid.Interfaces;
using Core.Domain.Limited_Double;
using Core.Domain.Numerical_methods.SEL.Functions.Interfaces;
using Core.Domain.Numerical_methods.SEL.Interfaces;
using Core.Domain.Numerical_methods.SEL.Services;
using Core.Domain.Physical.Interfaces;
using Core.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Numerical_methods.SEL.Functions.Services;

internal class WaypointCalculatorProjectile : IWaypointCalculatorProjectile
{
    private readonly IGrid g;
    private readonly INumericalMethodSettings _settings;
    private readonly ICannon _c;

    private readonly IXGetter _x;

    private double _h;

    public WaypointCalculatorProjectile(IGrid grid, INumericalMethodSettings settings, ICannon cannon, IXGetter xGetter)
    {
        g = grid;
        _settings = settings;
        _h = settings.h;
        _c = cannon;

        _x = xGetter;
    }
    public double NablaWithS(PNsn mu, PN v, LimitedDouble N)
    {
        var n = OffseterN.AppointAndOffset(N, +0.5);

        return (AverageProjectile(mu, PNsn.vSn, n + 0.5) - AverageK(mu, v, n + 0.5))
               / _h;
    }
    private double AverageProjectile(PNsn mu, PNsn v, LimitedDouble N)
    {
        var n = OffseterN.AppointAndOffset(N, +0.5);

        double muValue;

        if (mu == PNsn.One_minus_m)
        {
            muValue = (1 - g.sn[PNsn.m, n]);
        }
        else
            muValue = g.sn[PNsn.m, n];

        return g.sn[v, n + 0.5] * muValue * _c.S(g.sn[PNsn.x, n]);
    }

    private double AverageK(PNsn mu_sn, PN v, LimitedDouble N)
    {
        var n = OffseterN.AppointAndOffset(N, +0.5);

        var K = g.LastIndexK(v, n + 0.5);

        double V = g[v, n + 0.5, K];
        if (V >= 0)
        {
            var mu = PNsnConverterToPn.Convert(mu_sn);
            return V * g[mu, n, K - 0.5] * _c.S(_x[K - 0.5]);
        }
            
        else
            return V * g.sn[mu_sn, n] * _c.S(g.sn[PNsn.x, n]);
    }

}

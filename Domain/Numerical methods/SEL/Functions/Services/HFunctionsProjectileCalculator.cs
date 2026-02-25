using Core.Domain.Enums;
using Core.Domain.Grid.Interfaces;
using Core.Domain.Limited_Double;
using Core.Domain.Numerical_methods.SEL.Formulas.Interfaces;
using Core.Domain.Numerical_methods.SEL.Formulas.Services;
using Core.Domain.Numerical_methods.SEL.Functions.Interfaces;
using Core.Domain.Numerical_methods.SEL.Interfaces;
using Core.Domain.Numerical_methods.SEL.Services;
using Core.Domain.Physical.Entities;
using Core.Domain.Physical.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Numerical_methods.SEL.Functions.Services;

internal class HFunctionsProjectileCalculator : IHFunctionsProjectileCalculator
{
    private IGrid g;
    private readonly ICannon _cannon;
    private readonly Gunpowder _powder;

    private readonly IHFormulasProjectile hFormulas;

    private readonly IXGetter _x;

    public HFunctionsProjectileCalculator(IGrid grid,
                                ICannon cannon,
                                Gunpowder powder,
                                IXGetter xGetter)
    {
        g = grid;
        _cannon = cannon;
        _powder = powder;

        _x = xGetter;

        hFormulas = new HFormulasProjectile();
    }

    public double H3(LimitedDouble N)
    {
        var n = OffseterN.AppointAndOffset(N, +0.5);

        var x_n = g.sn[PNsn.x, n];

        double S_n = _cannon.S(x_n);
        double G_n = G(n);

        var res = hFormulas.H3_Sn_nP05(S_n, G_n);
        return res;
    }

    public double H4(LimitedDouble N)
    {
        var n = OffseterN.AppointAndOffset(N, +0.5);

        var x_n = g.sn[PNsn.x, n];

        double S_n = _cannon.S(x_n);
        double G_n = G(n);
        double Q = _powder.Q;

        var res = hFormulas.H4_Sn_nP05(S_n, G_n, Q);
        return res;
    }

    public double H5(LimitedDouble N)
    {
        var n = OffseterN.AppointAndOffset(N, +0.5);

        double Uk_n = _powder.Uk(g.sn[PNsn.p, n]);
        double e1 = _powder.e1;

        var res = hFormulas.H5_Sn_nP05(Uk_n, e1);
        return res;
    }

    public double HPsi(LimitedDouble N)
    {
        var n = OffseterN.AppointAndOffset(N, +0.5);

        double Sigma_n = _powder.Sigma(g.sn[PNsn.psi, n], g.sn[PNsn.z, n]);
        double Uk_n = _powder.Uk(g.sn[PNsn.p, n]);
        double S0 = _powder.S0;
        double LAMBDA0 = _powder.LAMBDA0;

        var res = hFormulas.HPsi_Sn_nP05(Sigma_n, Uk_n, S0, LAMBDA0);
        return res;
    }

    private double G(LimitedDouble n)
    {
        double a_n = g.sn[PNsn.a, n];
        double Sigma_n = _powder.Sigma(g.sn[PNsn.psi, n], g.sn[PNsn.z, n]);
        double Uk_n = _powder.Uk(g.sn[PNsn.p, n]);
        double S0 = _powder.S0;
        double Delta = _powder.Delta;

        var res = hFormulas.G_Sn_n(a_n, Sigma_n, Uk_n, S0, Delta);
        return res;
    }
}

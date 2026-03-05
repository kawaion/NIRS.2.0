using Core.Domain.Grid.Interfaces;
using Core.Domain.Limited_Double;
using Core.Domain.Numerical_methods.SEL.Formulas.Interfaces;
using Core.Domain.Numerical_methods.SEL.Formulas.Services;
using Core.Domain.Numerical_methods.SEL.Functions.Interfaces;
using Core.Domain.Numerical_methods.SEL.Helper.Services;
using Core.Domain.Numerical_methods.SEL.Interfaces;
using Core.Domain.Physical.Entities;
using Core.Domain.Physical.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Numerical_methods.SEL.Functions.Services;
internal class HFunctionsInNEqual0_5Calculator : IHFunctionsCalculator
{
    private IGrid g;
    private readonly ICannon _cannon;
    private readonly Gunpowder _powder;
    private readonly IConstants _constants;

    private readonly IHFormulas _hFormulas;

    private readonly IXGetter _x;

    public IHFunctionsProjectileCalculator sn { get; set; }

    public HFunctionsInNEqual0_5Calculator(IGrid grid,
                                           ICannon cannon,
                                           Gunpowder powder,
                                           IXGetter xGetter,
                                           IHFormulas hFormulas)
    {
        g = grid;
        _cannon = cannon;
        _powder = powder;

        _x = xGetter;

        _hFormulas = hFormulas;

        sn = new HFunctionsProjectileCalculator(grid, cannon, powder, xGetter);
    }

    public double H1(LimitedDouble n, LimitedDouble k)
    {
        return 0;
    }

    public double H2(LimitedDouble n, LimitedDouble k)
    { 
        return 0;
    }

    public double H3(LimitedDouble N, LimitedDouble K)
    {
        (var n, var k) = OffseterNK.AppointAndOffset(N, +0.5, K, -0.5);

        double x_kM05 = _x[k - 0.5];

        double S_kM05 = _cannon.S(x_kM05);
        double G_n_k = G(n, k);

        var res = _hFormulas.H3_nP05_kM05(S_kM05, G_n_k);
        return res;
    }

    public double H4(LimitedDouble N, LimitedDouble K)
    {
        (var n, var k) = OffseterNK.AppointAndOffset(N, +0.5, K, -0.5);

        double x_kM05 = _x[k - 0.5];

        double S_kM05 = _cannon.S(x_kM05);
        double G_n_k = G(n, k);
        double v_nP05_k = g[PN.v, n + 0.5, k];
        double w_nP05_k = g[PN.w, n + 0.5, k];
        double tau_w_nP1_k = tauW(n, k);
        double Q = _powder.Q;

        var res = _hFormulas.H4_nP05_kM05(S_kM05, G_n_k, v_nP05_k, w_nP05_k, tau_w_nP1_k, Q);
        return res;
    }

    public double H5(LimitedDouble N, LimitedDouble K)
    {
        (var n, var k) = OffseterNK.AppointAndOffset(N, +0.5, K, -0.5);

        double uk_p_n_kM05 = _powder.Uk(g[PN.p, n, k - 0.5]);
        double e1 = _powder.e1;

        var res = _hFormulas.H5_nP05_kM05(uk_p_n_kM05, e1);
        return res;
    }

    public double HPsi(LimitedDouble N, LimitedDouble K)
    {
        (var n, var k) = OffseterNK.AppointAndOffset(N, +0.5, K, -0.5);

        double S0 = _powder.S0;
        double LAMBDA0 = _powder.LAMBDA0;
        double z0_n_kM05 = g[PN.z, n, k - 0.5];
        double sigma_n_kM05 = _powder.Sigma(g[PN.psi, n, k - 0.5], g[PN.z, n, k - 0.5]);
        double uk_p_n_kM05 = _powder.Uk(g[PN.p, n, k - 0.5]);

        var res = _hFormulas.HPsi_nP05_kM05(S0, LAMBDA0, sigma_n_kM05, uk_p_n_kM05);
        return res;
    }


    private double tauW(LimitedDouble n, LimitedDouble k)
    {
        double rho_n_kM05 = g[PN.rho, n, k - 0.5];
        double v_nM05_k = g[PN.v, n - 0.5, k];
        double w_nM05_k = g[PN.w, n - 0.5, k];
        double a_n_kM05 = g[PN.a, n, k - 0.5];
        double sigma_n_kM05 = _powder.Sigma(g[PN.psi, n, k - 0.5], g[PN.z, n, k - 0.5]);
        double lambda0 = _constants.lamda0;
        double S0 = _powder.S0;

        var res = _hFormulas.tau_w_n_k(rho_n_kM05, v_nM05_k, w_nM05_k, a_n_kM05, sigma_n_kM05, lambda0, S0);
        return res;
    }
    private double G(LimitedDouble n, LimitedDouble k)
    {
        double a_n_kM05 = g[PN.a, n, k - 0.5];
        double S0 = _powder.S0;
        double sigma_n_kM0 = _powder.Sigma(g[PN.psi, n, k - 0.5], g[PN.z, n, k - 0.5]);
        double delta = _powder.Delta;
        double uk_p_n_kM05 = _powder.Uk(g[PN.p, n, k - 0.5]);

        var res = _hFormulas.G_n_k(a_n_kM05, S0, sigma_n_kM0, delta, uk_p_n_kM05);
        return res;
    }
}

using Core.Domain.Enums;
using Core.Domain.Grid.Interfaces;
using Core.Domain.Limited_Double;
using Core.Domain.Numerical_methods.SEL.Formulas.Interfaces;
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

using static Core.Domain.Limited_Double.LimitedDouble;

namespace Core.Domain.Numerical_methods.SEL.Functions.Services;

internal class CalculatorValuesInNodesNequal0_5 
{
}
internal class CalculatorValuesInNodesNequal0_5 : ICalculatorValuesInNodesNequal0_5
{
    private IGrid g;
    private IWaypointCalculator _wc;
    private readonly ICannon _cannon;
    private IHFunctionsCalculator _hf;
    private readonly Gunpowder _powder;
    private readonly IPressureGradientCalculator _pg;
    private readonly IPseudoViscosityMechanism _q;
    private readonly INumericalMethodSettings _settings;

    private readonly IStepFormulas stepFormulas;

    private double tau;

    private readonly IXGetter _x;

    public CalculatorValuesInNodesNequal0_5(IGrid grid,
                                   ICannon cannon,
                                   Gunpowder powder,
                                   IStepFormulas stepFormulas,
                                   IHFunctionsCalculator hFunctions,
                                   IWaypointCalculator waypointCalculator,
                                   IPressureGradientCalculator pg,
                                   IPseudoViscosityMechanism q,
                                   IXGetter x,
                                   INumericalMethodSettings settings
                                   )

    {
        g = grid;
        _q = q;
        _wc = waypointCalculator;
        _hf = hFunctions;
        _cannon = cannon;
        _powder = powder;
        _settings = settings;
        _pg = pg;

        _x = x;
        tau = settings.tau;
    }
    public IGrid CalculateDynamic(IGrid g, LimitedDouble n, LimitedDouble kFirst, LimitedDouble kLast)
    {
        for (LimitedDouble k = kFirst.Copy(); k <= kLast; k += 1)
        {

            (var offset_n, var offset_k) = OffseterNK.AppointAndOffset(n, ld0_5, k, ld0); // имеют одно и то же смещение

            g.At(PN.dynamic_m, offset_n, offset_k).Set(Calculate_dynamic_m(offset_n, offset_k));
            g.At(PN.v, offset_n, offset_k).Set(Calculate_v(offset_n, offset_k));
            g.At(PN.M, offset_n, offset_k).Set(Calculate_M(offset_n, offset_k));
            g.At(PN.w, offset_n, offset_k).Set(Calculate_dynamic_m(offset_n, offset_k));
        }

        return g;
    }


    public double Calculate_dynamic_m(LimitedDouble n, LimitedDouble k)
    {
        //(offset_n, offset_k) = OffseterNK.AppointAndOffset(offset_n, ld0_5, offset_k, ld0);

        double x_kM05 = _x[k - ld0_5];
        double x_kP05 = _x[k + ld0_5];

        double dynamicm_nM05_k = 0;
        double nabla_dynamicm_v_nM05_k = _wc.Nabla(PN.dynamic_m, PN.v, n - ld0_5, k);
        double m_n_kM05 = g[PN.m, n, k - ld0_5];
        double m_n_kP05 = g[PN.m, n, k + ld0_5];
        double S_kM05 = _cannon.S(x_kM05);
        double S_kP05 = _cannon.S(x_kP05);
        double dpStrokeDivDx_n_k = _pg.dPStrokeDivdx(n, k);
        double H1_n_k = _hf.H1(n, k);
        double tau = this.tau;

        var res = stepFormulas.dynamic_m_nP05_k(dynamicm_nM05_k, nabla_dynamicm_v_nM05_k, m_n_kM05, m_n_kP05,
                                                 S_kM05, S_kP05, dpStrokeDivDx_n_k, H1_n_k, tau);

        return res;
    }
    public double Calculate_v(LimitedDouble n, LimitedDouble k)
    {
        //(offset_n, offset_k) = OffseterNK.AppointAndOffset(offset_n, ld0_5, offset_k, ld0);

        double dynamicm_nP05_k = g[PN.dynamic_m, n + ld0_5, k];
        double r_n_kM05 = g[PN.r, n, k - ld0_5];
        double r_n_kP05 = g[PN.r, n, k + ld0_5];

        var res = stepFormulas.v_nP05_k(dynamicm_nP05_k, r_n_kM05, r_n_kP05);

        return res;
    }
    public double Calculate_M(LimitedDouble n, LimitedDouble k)
    {
        //(offset_n, offset_k) = OffseterNK.AppointAndOffset(offset_n, ld0_5, offset_k, ld0);

        double x_kM05 = _x[k - ld0_5];
        double x_kP05 = _x[k + ld0_5];

        double M_nM05_k = 0;
        double nabla_Mv_nM05_k = _wc.Nabla(PN.M, PN.w, n - ld0_5, k);
        double oneMm_n_kM05 = g[PN.One_minus_m, n, k - ld0_5];
        double oneMm_n_kP05 = g[PN.One_minus_m, n, k + ld0_5];
        double S_kM05 = _cannon.S(x_kM05);
        double S_kP05 = _cannon.S(x_kP05);
        double dpStrokeDivDx_n_k = _pg.dPStrokeDivdx(n, k);
        double H2_n_k = _hf.H2(n, k);
        double tau = this.tau;

        var res = stepFormulas.M_nP05_k(M_nM05_k, nabla_Mv_nM05_k,
                                          oneMm_n_kM05, oneMm_n_kP05,
                                          S_kM05, S_kP05,
                                          dpStrokeDivDx_n_k, H2_n_k,
                                          tau);
        return res;
    }
    public double Calculate_w(LimitedDouble n, LimitedDouble k)
    {
        //(offset_n, offset_k) = OffseterNK.AppointAndOffset(offset_n, ld0_5, offset_k, ld0);

        double x_kM05 = _x[k - ld0_5];
        double x_kP05 = _x[k + ld0_5];

        double M_nP05_k = g[PN.M, n + ld0_5, k];
        double oneMm_n_kM05 = g[PN.One_minus_m, n, k - ld0_5];
        double oneMm_n_kP05 = g[PN.One_minus_m, n, k + ld0_5];
        double S_kM05 = _cannon.S(x_kM05);
        double S_kP05 = _cannon.S(x_kP05);
        double delta = _powder.Delta;

        var res = stepFormulas.w_nP05_k(M_nP05_k, oneMm_n_kM05, oneMm_n_kP05,
                                         S_kM05, S_kP05, delta);
        return res;
    }
}

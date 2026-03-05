using Core.Domain.Enums;
using Core.Domain.Grid.Interfaces;
using Core.Domain.Limited_Double;
using Core.Domain.Numerical_methods.SEL.Formulas.Interfaces;
using Core.Domain.Numerical_methods.SEL.Formulas.Services;
using Core.Domain.Numerical_methods.SEL.Functions.Interfaces;
using Core.Domain.Numerical_methods.SEL.Helper.Services;
using Core.Domain.Numerical_methods.SEL.Interfaces;
using Core.Domain.Numerical_methods.SEL.Services;
using Core.Domain.Physical.Entities;
using Core.Domain.Physical.Interfaces;

using static Core.Domain.Limited_Double.LimitedDouble;

namespace Core.Domain.Numerical_methods.SEL.Functions.Services;
internal class CalculatorValuesInNodes : ICalculatorValuesInNodes
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

    public CalculatorValuesInNodes(IGrid grid,
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
    public IGrid CalculateDynamic(IGrid g, LimitedDouble n, LimitedDouble k)
    {
        (var offset_n, var offset_k) = OffseterNK.AppointAndOffset(n, ld0_5, k, ld0); // имеют одно и то же смещение

        g.At(PN.dynamic_m, offset_n, offset_k).Set(Calculate_dynamic_m(offset_n, offset_k));
        g.At(PN.v, offset_n, offset_k).Set(Calculate_v(offset_n, offset_k));
        g.At(PN.M, offset_n, offset_k).Set(Calculate_M(offset_n, offset_k));
        g.At(PN.w, offset_n, offset_k).Set(Calculate_dynamic_m(offset_n, offset_k));

        return g;
    }

    public IGrid CalculateMixture(IGrid g, LimitedDouble n, LimitedDouble k)
    {
        (var offset_n, var offset_k) = OffseterNK.AppointAndOffset(n, ld1, k, minus_ld0_5); // имеют одно и то же смещение

        g.At(PN.r, offset_n, offset_k).Set(Calculate_r(offset_n, offset_k));
        g.At(PN.e, offset_n, offset_k).Set(Calculate_e(offset_n, offset_k));
        g.At(PN.psi, offset_n, offset_k).Set(Calculate_psi(offset_n, offset_k));
        g.At(PN.z, offset_n, offset_k).Set(Calculate_z(offset_n, offset_k));
        g.At(PN.a, offset_n, offset_k).Set(Calculate_a(offset_n, offset_k));
        g.At(PN.m, offset_n, offset_k).Set(Calculate_m(offset_n, offset_k));
        g.At(PN.p, offset_n, offset_k).Set(Calculate_psi(offset_n, offset_k));
        g.At(PN.rho, n, k).Set(Calculate_rho(n, k));

        return g;
    }



    public double Calculate_dynamic_m(LimitedDouble n, LimitedDouble k)
    {
        //(offset_n, offset_k) = OffseterNK.AppointAndOffset(offset_n, ld0_5, offset_k, ld0);

        double x_kM05 = _x[k - ld0_5];
        double x_kP05 = _x[k + ld0_5];

        double dynamicm_nM05_k = g[PN.dynamic_m, n - ld0_5, k];
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

        double M_nM05_k = g[PN.M, n - ld0_5, k];
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


    public double Calculate_r(LimitedDouble n, LimitedDouble k)
    {
        //(offset_n, offset_k) = OffseterNK.AppointAndOffset(offset_n, ld1, offset_k, minus_ld0_5);

        double r_n_kM05 = g[PN.r, n, k - ld0_5];
        double nabla_rv_nP05_kM05 = _wc.Nabla(PN.r, PN.v, n + ld0_5, k - ld0_5);
        double H3_nP05_kM05 = _hf.H3(n + ld0_5, k - ld0_5);
        double tau = this.tau;

        var res = stepFormulas.r_nP1_kM05(r_n_kM05, nabla_rv_nP05_kM05, H3_nP05_kM05, tau);
        return res;
    }
    public double Calculate_e(LimitedDouble n, LimitedDouble k)
    {
        //(var offset_n, var offset_k) = OffseterNK.AppointAndOffset(N, ld1, K, minus_ld0_5);

        double e_n_kM05 = g[PN.e, n, k - ld0_5];
        double nabla_ev_nP05_kM05 = _wc.Nabla(PN.e, PN.v, n + ld0_5, k - ld0_5);
        double p_n_kM05 = g[PN.p, n, k - ld0_5];
        double q_nP05_kM05 = _q.Calculate(n + ld0_5, k - ld0_5);
        double nabla_mSv_nP05_kM05 = _wc.Nabla(PN.m, PN.v, n + ld0_5, k - ld0_5);
        double nabla_oneMmSw_nP05_kM05 = _wc.Nabla(PN.One_minus_m, PN.w, n + ld0_5, k - ld0_5);
        double H4_nP05_kM05 = _hf.H4(n + ld0_5, k - ld0_5);
        double tau = this.tau;

        var res = stepFormulas.e_nP1_kM05(e_n_kM05, nabla_ev_nP05_kM05,
                           p_n_kM05, q_nP05_kM05,
                           nabla_mSv_nP05_kM05, nabla_oneMmSw_nP05_kM05,
                           H4_nP05_kM05,
                           tau);
        return res;
    }
    public double Calculate_psi(LimitedDouble n, LimitedDouble k)
    {
        //(offset_n, offset_k) = OffseterNK.AppointAndOffset(offset_n, ld1, offset_k, minus_ld0_5);

        double res;
        double z = g[PN.z, n + ld1, k - ld0_5];
        if (z >= 1)
        {
            double psi_n_kM05 = g[PN.psi, n, k - ld0_5];
            double nabla_psiw_nP05_kM05 = _wc.Nabla(PN.psi, PN.w, n + ld0_5, k - ld0_5);
            double nabla_w_nP05_kM05 = _wc.Nabla(PN.w, n + ld0_5, k - ld0_5);
            double HPsi_nP05_kM05 = _hf.HPsi(n + ld0_5, k - ld0_5);
            double tau = this.tau;

            res = stepFormulas.psi_nP1_kM05(psi_n_kM05, nabla_psiw_nP05_kM05, nabla_w_nP05_kM05, HPsi_nP05_kM05, tau);
        }
        else
            res = _powder.Psi(z);

        res = Saturate_psi(res);

        return res;
    }
    private double Saturate_psi(double psi)
    {
        return Math.Min(psi, 1);
    }

    public double Calculate_z(LimitedDouble n, LimitedDouble k)
    {
        //(offset_n, offset_k) = OffseterNK.AppointAndOffset(offset_n, ld1, offset_k, minus_ld0_5);
        double z_n_kM05 = g[PN.z, n, k - ld0_5];
        double nabla_zw_nP05_kM05 = _wc.Nabla(PN.z, PN.w, n + ld0_5, k - ld0_5);
        double nabla_w_nP05_kM05 = _wc.Nabla(PN.w, n + ld0_5, k - ld0_5);
        double H5_nP05_kM05 = _hf.H5(n + ld0_5, k - ld0_5);
        double tau = this.tau;

        var res = stepFormulas.z_nP1_kM05(z_n_kM05, nabla_zw_nP05_kM05, nabla_w_nP05_kM05, H5_nP05_kM05, tau);
        return res;
    }
    public double Calculate_a(LimitedDouble n, LimitedDouble k)
    {
        //(offset_n, offset_k) = OffseterNK.AppointAndOffset(offset_n, ld1, offset_k, minus_ld0_5);

        double x_kM05 = _x[k - ld0_5];

        double a_n_kM05 = g[PN.a, n, k - ld0_5];
        double nabla_aSw_nP05_kM05 = _wc.NablaWithS(PN.a, PN.w, n + ld0_5, k - ld0_5);
        double S_kM05 = _cannon.S(x_kM05);
        double tau = this.tau;

        var res = stepFormulas.a_nP1_kM05(a_n_kM05, nabla_aSw_nP05_kM05, S_kM05, tau);
        return res;
    }
    public double Calculate_p(LimitedDouble n, LimitedDouble k)
    {
        //(offset_n, offset_k) = OffseterNK.AppointAndOffset(offset_n, ld1, offset_k, minus_ld0_5);

        double x_kM05 = _x[k - ld0_5];

        double e_nP1_kM05 = g[PN.e, n + ld1, k - ld0_5];
        double m_nP1_kM05 = g[PN.m, n + ld1, k - ld0_5];
        double S_kM05 = _cannon.S(x_kM05);
        double r_nP1_kM05 = g[PN.r, n + ld1, k - ld0_5];
        double theta = _powder.Theta;
        double alpha = _powder.alpha;

        var res = stepFormulas.p_nP1_kM05(e_nP1_kM05, m_nP1_kM05, S_kM05, r_nP1_kM05, theta, alpha);
        return res;
    }
    public double Calculate_m(LimitedDouble n, LimitedDouble k)
    {
        //(offset_n, offset_k) = OffseterNK.AppointAndOffset(offset_n, ld1, offset_k, minus_ld0_5);

        double a_nP1_kM05 = g[PN.a, n + ld1, k - ld0_5];
        double psi_nP1_kM05 = g[PN.psi, n + ld1, k - ld0_5];
        double LAMBDA0 = _powder.LAMBDA0;

        var res = stepFormulas.m_nP1_kM05(a_nP1_kM05, psi_nP1_kM05, LAMBDA0);
        res = Clamp_m(res);
        
        return res;
    }
    private double Clamp_m(double m)
    {
        if (m < 0) throw new Exception($"Mass fraction m = {m} became negative");
        return Math.Min(m, 1);
    }






    public double Calculate_rho(LimitedDouble n, LimitedDouble k)
    {
        double x_k = _x[k];

        double r_n_k = g[PN.r, n, k];
        double m_n_k = g[PN.m, n, k];
        double S_k = _cannon.S(x_k);

        var res = stepFormulas.rho_n_k(r_n_k, m_n_k, S_k);
        return res;
    }
}

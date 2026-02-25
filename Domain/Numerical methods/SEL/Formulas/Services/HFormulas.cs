using Core.Domain.Numerical_methods.SEL.Formulas.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Numerical_methods.SEL.Formulas.Services;

internal class HFormulas : IHFormulas
{
    public HFormulas()
    {
    }

    public double H1_n_k(double S_k, double tau_w_n_k,
                                    double G_n_k, double w_nM05_k)
    {
        return -S_k * tau_w_n_k + S_k * G_n_k * w_nM05_k;
    }
    public double H2_n_k(double S_k, double tau_w_n_k,
                               double G_n_k, double w_nM05_k)
    {
        return S_k * tau_w_n_k - S_k * G_n_k * w_nM05_k;
    }
    public double H3_nP05_kM05(double S_kM05, double G_n_k)
    {
        return S_kM05 * G_n_k;
    }
    public double H4_nP05_kM05(double S_kM05, double G_n_k,
                                double v_nP05_k, double w_nP05_k,
                                double tau_w_nP1_k,
                                double Q)
    {
        return S_kM05 * G_n_k * (Q + Math.Pow(v_nP05_k - w_nP05_k, 2) / 2)
                + S_kM05 * tau_w_nP1_k * (v_nP05_k - w_nP05_k);
    }
    public double H5_nP05_kM05(double uk_p_n_kM05, double e1)
    {
        return uk_p_n_kM05 / e1;
    }
    public double HPsi_nP05_kM05(double S0, double LAMBDA0,
                                        double sigma_n_kM05, double uk_p_n_kM05)
    {
        return (S0 / LAMBDA0) * sigma_n_kM05 * uk_p_n_kM05;
    }




    public double tau_w_n_k(double rho_n_kM05, double v_nM05_k,
                               double w_nM05_k, double a_n_kM05,
                               double sigma_n_kM05,
                               double lambda0, double S0)
    {
        double velocityDiff = v_nM05_k - w_nM05_k;
        double absVelocityDiff = Math.Abs(velocityDiff);

        return lambda0 * (rho_n_kM05 * velocityDiff * absVelocityDiff / 2.0)
               * a_n_kM05 * (S0 * sigma_n_kM05 / 4.0);
    }
    public double G_n_k(double a_n_kM05,
                               double S0, double sigma_n_kM0, double delta, double uk_p_n_kM05)
    {
        return a_n_kM05 * S0 * sigma_n_kM0 * delta * uk_p_n_kM05;
    }
}

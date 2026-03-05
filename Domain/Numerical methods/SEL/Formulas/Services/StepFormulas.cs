using Core.Domain.Numerical_methods.SEL.Formulas.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Numerical_methods.SEL.Formulas.Services;

internal class StepFormulas : IStepFormulas
{
    public StepFormulas()
    {

    }
    public double dynamic_m_nP05_k(double dynamicm_nM05_k, double nabla_dynamicm_v_nM05_k,
                                   double m_n_kM05, double m_n_kP05,
                                   double S_kM05, double S_kP05,
                                   double dpStrokeDivDx_n_k, double H1_n_k,
                                   double tau)
    {
        return dynamicm_nM05_k - tau *
            (
                nabla_dynamicm_v_nM05_k
                + (m_n_kM05 * S_kM05 + m_n_kP05 * S_kP05) / 2 * dpStrokeDivDx_n_k
                - H1_n_k
            );
    }
    public double v_nP05_k(double dynamicm_nP05_k, double r_n_kM05, double r_n_kP05)
    {
        return 2 * dynamicm_nP05_k / (r_n_kM05 + r_n_kP05);
    }
    public double M_nP05_k(double M_nM05_k, double nabla_Mv_nM05_k,
                                          double oneMm_n_kM05, double oneMm_n_kP05,
                                          double S_kM05, double S_kP05,
                                          double dpStrokeDivDx_n_k, double H2_n_k,
                                          double tau)
    {
        return M_nM05_k - tau *
            (
                nabla_Mv_nM05_k
                + (oneMm_n_kM05 * S_kM05 + oneMm_n_kP05 * S_kP05) / 2 * dpStrokeDivDx_n_k
                - H2_n_k
            );
    }
    public double w_nP05_k(double M_nP05_k, double oneMm_n_kM05, double oneMm_n_kP05,
                                         double S_kM05, double S_kP05, double delta)
    {
        return 2 * M_nP05_k / (delta * (oneMm_n_kM05 * S_kM05 + oneMm_n_kP05 * S_kP05));
    }
    public double r_nP1_kM05(double r_n_kM05, double nabla_rv_nP05_kM05, double H3_nP05_kM05, double tau)
    {
        return r_n_kM05 - tau * (nabla_rv_nP05_kM05 - H3_nP05_kM05);
    }
    public double e_nP1_kM05(double e_n_kM05, double nabla_ev_nP05_kM05,
                           double p_n_kM05, double q_nP05_kM05,
                           double nabla_mSv_nP05_kM05, double nabla_oneMmSw_nP05_kM05,
                           double H4_nP05_kM05,
                           double tau)
    {
        return e_n_kM05 - tau *
            (
                nabla_ev_nP05_kM05
                + (p_n_kM05 + q_nP05_kM05) * (nabla_mSv_nP05_kM05 + nabla_oneMmSw_nP05_kM05)
                - H4_nP05_kM05
            );
    }
    public double z_nP1_kM05(double z_n_kM05, double nabla_zw_nP05_kM05,
                           double nabla_w_nP05_kM05, double H5_nP05_kM05, double tau)
    {
        return z_n_kM05 - tau *
            (
                nabla_zw_nP05_kM05
                - z_n_kM05 * nabla_w_nP05_kM05
                - H5_nP05_kM05
            );
    }
    public double psi_nP1_kM05(double psi_n_kM05, double nabla_psiw_nP05_kM05,
                   double nabla_w_nP05_kM05,
                   double HPsi_nP05_kM05, double tau)
    {
        return psi_n_kM05 - tau *
            (
                nabla_psiw_nP05_kM05
                - psi_n_kM05 * nabla_w_nP05_kM05
                - HPsi_nP05_kM05
            );
    }
    public double a_nP1_kM05(double a_n_kM05, double nabla_aSw_nP05_kM05, double S_kM05, double tau)
    {
        return a_n_kM05 - tau * (nabla_aSw_nP05_kM05 / S_kM05);
    }
    public double p_nP1_kM05(double e_nP1_kM05, double m_nP1_kM05,
                                  double S_kM05, double r_nP1_kM05, double teta, double alpha)
    {
        return teta * e_nP1_kM05 / (m_nP1_kM05 * S_kM05 - alpha * r_nP1_kM05);
    }
    public double m_nP1_kM05(double a_nP1_kM05,
                                      double psi_nP1_kM05, double LAMBDA0)
    {
        return 1 - a_nP1_kM05 * LAMBDA0 * (1 - psi_nP1_kM05);
    }
    public double rho_n_k(double r_n_k,
                              double m_n_k, double S_k)
    {
        return r_n_k / (m_n_k * S_k);
    }
}

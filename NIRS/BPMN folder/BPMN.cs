using NIRS.Helpers;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NIRS.BPMN_folder
{
    static class BPMN
    {
        public static double dynamic_m_nP05_k(double dynamicm_nM05_k, double nabla_dynamicm_v_nM05_k,
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
        public static double v_nP05_k(double M_nP05_k, double r_n_kM05, double r_n_kP05)
        {
            return 2 * M_nP05_k / (r_n_kM05 + r_n_kP05);
        }
        public static double M_nP05_k(double M_nM05_k, double nabla_Mv_nM05_k,
                                              double oneMm_n_kM05, double oneMm_n_kP05,
                                              double S_kM05, double S_kP05,
                                              double dpStrokeDivDx_n_k, double H2_n_k, 
                                              double tau )
        {
            return M_nM05_k - tau *
                (
                    nabla_Mv_nM05_k
                    + (oneMm_n_kM05 * S_kM05 + oneMm_n_kP05 * S_kP05) / 2 * dpStrokeDivDx_n_k
                    - H2_n_k
                );
        }
        public static double w_nP05_k(double M_nP05_k, double oneMm_n_kM05, double oneMm_n_kP05,
                                             double S_kM05_n, double S_kP05_n, double delta)
        {
            return 2 * M_nP05_k / delta * (oneMm_n_kM05 * S_kM05_n + oneMm_n_kP05 * S_kP05_n);
        }
        public static double r_nP1_kM05(double r_n_kM05, double nabla_rv_nP05_kM05, double H3_nP05_kM05, double tau)
        {
            return r_n_kM05 - tau * (nabla_rv_nP05_kM05 - H3_nP05_kM05);
        }
        public static double e_nP1_kM05(double e_n_kM05, double nabla_ev_nP05_kM05,
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
        public static double z_nP1_kM05(double z_n_kM05, double nabla_zw_nP05_kM05,
                               double z_nP05_kM05, double nabla_w_nP05_kM05,
                               double H5_nP05_kM05, double tau)
        {
            return z_n_kM05 - tau *
                (
                    nabla_zw_nP05_kM05
                    - z_n_kM05 * nabla_w_nP05_kM05
                    - H5_nP05_kM05
                );
        }
        public static double psi_nP1_kM05(double psi_n_kM05, double nabla_psiw_nP05_kM05,
                       double z_nP05_kM05, double nabla_w_nP05_kM05,
                       double HPsi_nP05_kM05, double tau)
        {
            return psi_n_kM05 - tau *
                (
                    nabla_psiw_nP05_kM05
                    - psi_n_kM05 * nabla_w_nP05_kM05
                    - HPsi_nP05_kM05
                );
        }
        public static double a_nP1_kM05(double a_n_kM05, double nabla_aSw_nP05_kM05, double S_n_kM05, double tau)
        {
            return a_n_kM05 - tau * (nabla_aSw_nP05_kM05 / S_n_kM05);
        }
        public static double p_nP1_kM05(double e_nP1_kM05, double m_nP1_kM05,
                                      double S_kM1, double r_nP1_kM05, double teta, double alpha)
        {
            return teta * e_nP1_kM05 / (m_nP1_kM05 * S_kM1 - alpha * r_nP1_kM05);
        }
        public static double m_nP1_kM05(double a_nP1_kM05, double z_nP1_kM05,
                                          double psi_nP1_kM05, double LAMBDA0)
        {
            return 1 - a_nP1_kM05 * LAMBDA0 * (1 - psi_nP1_kM05);
        }




        public static double H1_n_k(double S_k, double tau_w_n_k,
                                    double G_n_k, double w_nM05_k)
        {
            return -S_k * tau_w_n_k + S_k * G_n_k * w_nM05_k;
        }
        public static double H2_n_k(double S_k, double tau_w_n_k,
                                   double G_n_k, double w_nM05_k)
        {
            return S_k * tau_w_n_k - S_k * G_n_k * w_nM05_k;
        }
        public static double H3_n_k(double S_kM05, double G_n_k)
        {
            return S_kM05 * G_n_k;
        }
        public static double H4_n_k(double S_kM05, double G_n_k,
                                    double v_nP05_k, double w_nP05_k,
                                    double tau_w_nP1_k,
                                    double Q)
        {
            return  S_kM05 * G_n_k * (Q + Math.Pow(v_nP05_k - w_nP05_k, 2) / 2)
                    + S_kM05 * tau_w_nP1_k * (v_nP05_k - w_nP05_k);
        }
        public static double H5_n_k(double uk_p_n_kM05, double e1)
        {
            return uk_p_n_kM05 / e1;
        }
        public static double HPsi_nP05_kM05(double S0, double LAMBDA0,
                                            double sigma_psi_n_kM05, double uk_p_n_kM05)
        {
            return (S0 / LAMBDA0) * sigma_psi_n_kM05 * uk_p_n_kM05;
        }




        public static double tau_w_n_k(double rho_n_kM05, double v_nM05_k,
                                   double w_nM05_k, double a_n_kM05,
                                   double z_n_kM05, double sigma_z_n_kM05,
                                   double lambda0, double S0) 
        {
            double velocityDiff = v_nM05_k - w_nM05_k;
            double absVelocityDiff = Math.Abs(velocityDiff);

            return lambda0 * (rho_n_kM05 * velocityDiff * absVelocityDiff / 2.0)
                   * a_n_kM05 * (S0 * sigma_z_n_kM05 / 4.0);
        }
        public static double G_n_k(double a_n_kM05,
                                   double S0, double sigma_z_n_kM0, double delta, double uk_p_n_kM05)
        {
            return a_n_kM05 * S0 * sigma_z_n_kM0 * delta * uk_p_n_kM05;
        }
        private static double U_k(double p, double u1)
        {
            return u1 * p;
        }        
        public static double sigma_z(double z, double lambda, double mu)
        {
            return 1.0 + 2.0 * lambda * z + 3.0 * mu * Math.Pow(z, 2);
        }
    }
}

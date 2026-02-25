using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Numerical_methods.SEL.Formulas.Interfaces;

internal interface IHFormulas
{
    double H1_n_k(double S_k, double tau_w_n_k,
                                    double G_n_k, double w_nM05_k);
    double H2_n_k(double S_k, double tau_w_n_k,
                               double G_n_k, double w_nM05_k);
    double H3_nP05_kM05(double S_kM05, double G_n_k);
    double H4_nP05_kM05(double S_kM05, double G_n_k,
                                double v_nP05_k, double w_nP05_k,
                                double tau_w_nP1_k,
                                double Q);
    double H5_nP05_kM05(double uk_p_n_kM05, double e1);
    double HPsi_nP05_kM05(double S0, double LAMBDA0,
                                        double sigma_n_kM05, double uk_p_n_kM05);




    double tau_w_n_k(double rho_n_kM05, double v_nM05_k,
                               double w_nM05_k, double a_n_kM05,
                               double sigma_n_kM05,
                               double lambda0, double S0);
    double G_n_k(double a_n_kM05,
                               double S0, double sigma_n_kM0, double delta, double uk_p_n_kM05);
}

using Core.Domain.Numerical_methods.SEL.Formulas.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Numerical_methods.SEL.Formulas.Services;

internal class HFormulasProjectile : IHFormulasProjectile
{
    public double H3_Sn_nP05(double S_n, double G_n)
    {
        return S_n * G_n;
    }

    public double H4_Sn_nP05(double S_n, double G_n, double Q)
    {
        return S_n * G_n * Q;
    }

    public double H5_Sn_nP05(double Uk_n, double e1)
    {
        return Uk_n / e1;
    }

    public double HPsi_Sn_nP05(double Sigma_n, double Uk_n, double S0, double LAMBDA0)
    {

        return (S0 / LAMBDA0) * Sigma_n * Uk_n;
    }

    public double G_Sn_n(double a_n, double Sigma_n, double Uk_n, double S0, double Delta)
    {
        return a_n * S0 * Sigma_n * Delta * Uk_n;
    }
}

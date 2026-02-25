using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Numerical_methods.SEL.Formulas.Interfaces;

internal interface IHFormulasProjectile
{
    public double H3_Sn_nP05(double S_n, double G_n);

    public double H4_Sn_nP05(double S_n, double G_n, double Q);

    public double H5_Sn_nP05(double Uk_n, double e1);

    public double HPsi_Sn_nP05(double Sigma_n, double Uk_n, double S0, double LAMBDA0);

    public double G_Sn_n(double a_n, double Sigma_n, double Uk_n, double S0, double Delta);
}

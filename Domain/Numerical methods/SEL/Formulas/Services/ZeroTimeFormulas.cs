using Core.Domain.Numerical_methods.SEL.Formulas.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Numerical_methods.SEL.Formulas.Services;

internal class ZeroTimeFormulas : IZeroTimeFormulas
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="omegaV">ωₐ</param>
    /// <param name="f">f</param>
    /// <param name="Wkm">Wkm</param>
    /// <param name="omega">ω</param>
    /// <param name="delta">δ</param>
    /// <param name="alpha">α</param>
    /// <returns>pₐ</returns>
    public double pV(double omegaV, double f, double Wkm, double omega, double delta, double alpha)
    {
        return (0.3 * omegaV * f) /
               (Wkm - omega / delta - alpha * omegaV);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pV">pₐ</param>
    /// <returns></returns>
    public double p0(double pV)
    {
        return pV;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pV">pₐ</param>
    /// <param name="alpha">α</param>
    /// <param name="f">f</param>
    /// <returns></returns>
    public double rh0(double pV, double alpha, double f)
    {
        return pV / (alpha * pV + f);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="f">а</param>
    /// <param name="theta">θ</param>
    /// <returns></returns>
    public double eps0(double f, double theta)
    {
        return f / theta;
    }
    public double z0()
    {
        return 0;
    }
    public double psi0()
    {
        return 0;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="DELTA">Δ</param>
    /// <param name="delta">δ</param>
    /// <returns></returns>
    public double m0(double DELTA, double delta)
    {
        return 1 - DELTA / delta;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="omega">ω</param>
    /// <param name="LAMBDA0">Λ₀</param>
    /// <param name="delta">δ</param>
    /// <param name="Wkm">Wkm</param>
    /// <returns></returns>
    public double a0(double omega, double LAMBDA0, double delta, double Wkm)
    {
        return omega /
              (LAMBDA0 * delta * Wkm);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ro0">ρ0</param>
    /// <param name="m0">m0</param>
    /// <param name="Sk">Sₖ</param>
    /// <returns></returns>
    public double r0(double ro0, double m0, double Sk)
    {
        return ro0 * m0 * Sk;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ro0">ρ0</param>
    /// <param name="m0">m0</param>
    /// <param name="Sk">Sₖ</param>
    /// <param name="eps0">ε0</param>
    /// <returns></returns>
    public double e0(double ro0, double m0, double Sk, double eps0)
    {
        return ro0 * m0 * Sk * eps0;
    }
}

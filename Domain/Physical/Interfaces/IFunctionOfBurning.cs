using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Physical.Interfaces;

internal interface IFunctionOfBurning
{
    /// <summary>
    /// u_k(p) - Скорость горения пороха при давлении p
    /// </summary>
    double Uk(double p);

    /// <summary>
    /// σ(ψ, z) - Отношение текущей поверхности горения к первоначальной
    /// </summary>
    double Sigma(double psi, double z);
}

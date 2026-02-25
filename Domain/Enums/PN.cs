using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Enums;

/// <summary>
/// PN - сокращенно Parameter name
/// </summary>
public enum PN
{
    dynamic_m = 0,
    M,
    v,
    w,
    r,
    e,
    eps,
    psi,
    z,
    a,
    p,
    rho,
    m,
    One_minus_m,
}
/// <summary>
/// PNsn - сокращенно Parameter name projectile
/// </summary>
public enum PNsn
{
    dynamic_m = 0,
    M,
    r,
    e,
    eps,
    psi,
    z,
    a,
    p,
    rho,
    m,
    vSn,
    One_minus_m,
    x,
}

﻿using MyDouble;
using NIRS.Parameter_names;

namespace NIRS.Interfaces
{
    public interface IProjectileFunctions
    {
        double Get(PN pn,LimitedDouble n);
        double Get_vSn(LimitedDouble n);
        double Get_x(LimitedDouble n);
        double Get_r(LimitedDouble n);
        double Get_z(LimitedDouble n);
        double Get_psi(LimitedDouble n);
        double Get_a(LimitedDouble n);
        double Get_m(LimitedDouble n);
        double Get_ro(LimitedDouble n);
        double Get_e(LimitedDouble n);
        double Get_p(LimitedDouble n);
    }
}

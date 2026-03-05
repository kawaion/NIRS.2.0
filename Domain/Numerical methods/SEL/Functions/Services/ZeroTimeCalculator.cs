using Core.Domain.Enums;
using Core.Domain.Grid.Interfaces;
using Core.Domain.Limited_Double;
using Core.Domain.Numerical_methods.SEL.Formulas.Interfaces;
using Core.Domain.Numerical_methods.SEL.Functions.Interfaces;
using Core.Domain.Numerical_methods.SEL.Interfaces;
using Core.Domain.Physical.Entities;
using Core.Domain.Physical.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Core.Domain.Limited_Double.LimitedDouble;

namespace Core.Domain.Numerical_methods.SEL.Functions.Services;

internal class ZeroTimeCalculator : IZeroTimeCalculator
{
    private IZeroTimeFormulas _ztf;

    private Gunpowder _gp;
    private ICannon _c;
    private IXGetter _x;
    private IConstants _constants;

    public ZeroTimeCalculator(Gunpowder gunpowder, 
                              ICannon cannon,
                              IXGetter xGetter,
                              IConstants constants)
    {
        _c = cannon;
        _x = xGetter;
        _constants = constants;
        _gp = gunpowder;
    }
    public IGrid CalculateMixture(IGrid g, LimitedDouble kFirst, LimitedDouble kLast)
    {
        var pVValue = _ztf.pV(_constants.OmegaV, _gp.f, _c.Wkm, _gp.Omega, _gp.Delta, _gp.alpha);

        var rho0Value = _ztf.rh0(pVValue, _gp.alpha, _gp.f);

        var eps0Value = _ztf.eps0(_gp.f, _gp.Theta);

        var z0Value = _ztf.z0();

        var psi0Value = _ztf.psi0();

        var m0Value = _ztf.m0(_constants.DELTA, _gp.Theta);

        var a0Value = _ztf.a0(_gp.Omega, _gp.LAMBDA0, _gp.Delta, _c.Wkm);

        for (var k = kFirst.Copy(); k <= kLast; k += ld1)
        {
            var Sk = _c.S(_x[k]);

            var r0Value = _ztf.r0(rho0Value, m0Value, Sk);

            var e0Value = _ztf.e0(rho0Value, m0Value, Sk, eps0Value);

            g.At(PN.rho, ld0, k).Set(rho0Value);
            g.At(PN.eps, ld0, k).Set(eps0Value);
            g.At(PN.z, ld0, k).Set(z0Value);
            g.At(PN.psi, ld0, k).Set(psi0Value);
            g.At(PN.m, ld0, k).Set(m0Value);
            g.At(PN.a, ld0, k).Set(a0Value);
            g.At(PN.r, ld0, k).Set(r0Value);
            g.At(PN.e, ld0, k).Set(e0Value);
        }

        return g;
    }
}


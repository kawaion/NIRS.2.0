using Core.Domain.Limited_Double;
using Core.Domain.Numerical_methods.SEL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Numerical_methods.SEL.Services;

internal class XGetter : IXGetter
{
    private double _h;
    public XGetter(INumericalMethodSettings settings)
    {
        _h = settings.h;
    }
    public double this[LimitedDouble value]
    {
        get
        {
            return _h * value.GetDouble();
        }
    }
}

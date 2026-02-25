using Core.Domain.Limited_Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Numerical_methods.SEL.Functions.Interfaces;

internal interface IPressureGradientCalculator
{
    public double dPStrokeDivdx(LimitedDouble n, LimitedDouble k);
}

using Core.Domain.Grid.Interfaces;
using Core.Domain.Limited_Double;
using Core.Domain.Numerical_methods.SEL.Formulas.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Numerical_methods.SEL.Functions.Interfaces;

internal interface IBoundaryCalculator
{
    public IGrid CalculateInLeft(IGrid g, LimitedDouble n, LimitedDouble k);

    public IGrid CalculateInRight(IGrid g, LimitedDouble n, LimitedDouble k);
}

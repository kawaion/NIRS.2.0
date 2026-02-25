using Core.Domain.Limited_Double;
using Core.Domain.Numerical_methods.SEL.Formulas.Services;
using Core.Domain.Numerical_methods.SEL.Interfaces;
using Core.Domain.Numerical_methods.SEL.Services;
using Core.Domain.Physical.Entities;
using Core.Domain.Physical.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Numerical_methods.SEL.Functions.Interfaces;

internal interface IHFunctionsCalculator
{
    public IHFunctionsProjectileCalculator sn{ get; }
    public double H1(LimitedDouble n, LimitedDouble k);
    public double H2(LimitedDouble n, LimitedDouble k);
    public double H3(LimitedDouble N, LimitedDouble K);
    public double H4(LimitedDouble N, LimitedDouble K);
    public double H5(LimitedDouble N, LimitedDouble K);
    public double HPsi(LimitedDouble N, LimitedDouble K);

}

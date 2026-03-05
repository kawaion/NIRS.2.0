using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Numerical_methods.SEL.Formulas.Interfaces;

internal interface IUnderTimeFormulas
{
    public double dynamic_mMinus0_5();

    public double MMinus0_5();
    
    public double vMinus0_5();

    public double wMinus0_5();
}

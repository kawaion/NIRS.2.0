using Core.Domain.Grid.Interfaces;
using Core.Domain.Limited_Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Numerical_methods.SEL.Functions.Interfaces;

internal interface IPseudoViscosityMechanism
{
    public double Calculate(LimitedDouble N, LimitedDouble K);
}

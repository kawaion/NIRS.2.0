using Core.Domain.Enums;
using Core.Domain.Limited_Double;
using Core.Domain.Numerical_methods.SEL.Services;
using Core.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Numerical_methods.SEL.Functions.Interfaces;

internal interface IWaypointCalculator
{
    double NablaWithS(PN param1, PN param2, LimitedDouble N, LimitedDouble K);

    double Nabla(double param1, PN param2, PN param3, LimitedDouble N, LimitedDouble K);


    double Nabla(PN param1, PN v, LimitedDouble N, LimitedDouble K);
    double Nabla(PN v, LimitedDouble N, LimitedDouble K);


    double dPStrokeDivdx(LimitedDouble n, LimitedDouble k);
}

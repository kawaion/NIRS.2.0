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
    double NablaWithS(PN mu, PN v, LimitedDouble N, LimitedDouble K);
    double Nabla(PN mu, PN v, LimitedDouble N, LimitedDouble K);
    double Nabla(PN v, LimitedDouble N, LimitedDouble K);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.interfaces;

internal interface IInterpolator
{
    public double Interpolate(double x);
}

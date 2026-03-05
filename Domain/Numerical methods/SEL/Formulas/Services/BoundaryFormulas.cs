using Core.Domain.Limited_Double;
using Core.Domain.Numerical_methods.SEL.Formulas.Interfaces;
using Core.Domain.Numerical_methods.SEL.Interfaces;
using Core.Domain.Physical.Entities;
using Core.Domain.Physical.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Numerical_methods.SEL.Formulas.Services;

internal class BoundaryFormulas : IBoundaryFormulas
{
    public BoundaryFormulas()
    {
    }
    public double dynamic_m0()
    {
        return 1e-05;
    }
    public double M0()
    {
        return 1e-05;
    }
    public double v0()
    {
        return 1e-05;
    }
    public double w0()
    {
        return 1e-05;
    }


    public double dynamic_mK()
    {
        return 1e-05;
    }
    public double MK()
    {
        return 1e-05;
    }
    public double vK()
    {
        return 1e-05;
    }
    public double wK()
    {
        return 1e-05;
    }
}

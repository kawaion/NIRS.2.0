using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Numerical_methods.SEL.Formulas.Interfaces;

internal interface IBoundaryFormulas
{
    public double dynamic_m0();

    public double M0();

    public double v0();

    public double w0();



    public double dynamic_mK();

    public double MK();

    public double vK();

    public double wK();

}

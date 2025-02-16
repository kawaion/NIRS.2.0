using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Interfaces
{
    public interface IBurningPowdersSize
    {
        double Psi(double z);
        double z(double z);

        double Sigma(double z, double psi);
        double Uk(double p);
    }
}

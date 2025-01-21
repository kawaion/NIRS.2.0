using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Cannon_Folder.Powder_Folder
{
    interface ICombustionFunctions
    {
        double Psi(double z);
        double z(double z);

        double Sigma(double z, double psi);
        double Uk(double p);
    }
}

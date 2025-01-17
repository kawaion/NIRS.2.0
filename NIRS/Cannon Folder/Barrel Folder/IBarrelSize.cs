using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Cannon_Folder.Barrel_Folder
{
    interface IBarrelSize
    {
        double Skn { get; }
        double Wkm { get; }

        double R(double x);        
        double D(double x);
        double S(double x);
        double W(double x);
        double W(double x1, double x2);
    }
}

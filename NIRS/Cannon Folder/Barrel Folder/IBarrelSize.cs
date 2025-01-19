using MyDouble;

namespace NIRS.Cannon_Folder.Barrel_Folder
{
    interface IBarrelSize
    {
        double h { get; }
        double Skn { get; }
        double Wkm { get; }

        double R(double x);        
        double D(double x);
        double S(double x);
        double W(double x);
        double W(double x1, double x2);
    }
}

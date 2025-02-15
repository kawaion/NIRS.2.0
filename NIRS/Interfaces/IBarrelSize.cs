using MyDouble;

namespace NIRS.Interfaces
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

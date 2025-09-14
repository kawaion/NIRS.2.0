

namespace NIRS.Interfaces
{
    public interface IPowder
    {
        double Omega { get; }
        double Delta { get; }
        double DELTA { get; }
        double D0 { get; }
        double d0 { get; }
        double L0 { get; }
        double e1 { get; }

        double S0 { get; }
        double LAMBDA0 { get; }
        double U1 { get; }

        IBurningPowdersSize BurningPowdersSize { get;}
    }
}

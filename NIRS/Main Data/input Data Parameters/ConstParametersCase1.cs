

using NIRS.Helpers;
using NIRS.Interfaces;

namespace NIRS.Data_Parameters.Input_Data_Parameters
{
    class ConstParametersCase1 : IConstParameters
    {
        private double Tau;
        private double H;
        private int CountDivideChamber;

        public double cp => 1838.8;
        public double cv => 1497.4;
        public ConstParametersCase1(double tau, int countDivideChamber, Point2D chamber)
        {
            Tau = tau;
            H = chamber.X/countDivideChamber;
            CountDivideChamber = countDivideChamber;
        }
        public int countDivideChamber { get { return CountDivideChamber; } }

        public double tau { get {return Tau; } }
        public double h { get { return H; } }
        public double teta => cp / cv - 1;
        public double alpha => 9.5e-4;
        public double PowderDelta => 1520; //GunpowderMass":19
        public double D0 => 0.0115;
        public double d0 => 0.0009;
        public double L0 => 0.019;
        public double mu0 => 5.18e-5;
        public double lamda0 => 0.45;
        public double Q => f / teta;
        public double e1 => 0.0022/2;
        public double u1 => 1.91e-9;
        public double omegaV => 0.1;
        public double f => 900000;
        public double q => 9.5;//46;
        public double forcingPressure => 30e6;
    }
}

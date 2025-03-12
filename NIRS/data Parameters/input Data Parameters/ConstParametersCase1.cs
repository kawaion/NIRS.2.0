

using NIRS.Interfaces;

namespace NIRS.Data_Parameters.Input_Data_Parameters
{
    class ConstParametersCase1 : IConstParameters
    {
        private double Tau;
        private double H;

        private double cp =1838.8;
        private double cv =1497.4;
        public ConstParametersCase1(double tau,double h)
        {
            Tau = tau;
            H = h;
        }
        public double tau { get {return Tau; } }
        public double h { get { return H; } }
        public double teta => cp / cv - 1;
        public double alpha => 9.5e-4;
        public double delta => 1520;
        public double D0 => 0.0115;
        public double d0 => 0.0009;
        public double L0 => 0.019;
        public double mu0 => 5.18e-5;
        public double lamda0 => 0.45;//
        public double Q => f / teta;
        public double e1 => 0.0022/2;
        public double u1 => 5.9e-10;
        public double omegaV => 0.1;
        public double f => 900000;
        public double q => 46;
    }
}

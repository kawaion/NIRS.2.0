using System;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Interfaces;

namespace NIRS.Cannon_Folder.Powder_Folder
{
    class Powder_12_7 : IPowder
    {
        public double Omega { get; }
        public double Delta { get; }
        public double DELTA { get; }
        public double D0 { get; }
        public double d0 { get; }
        public double L0 { get; }
        public double e1 { get; }
        public Powder_12_7(double omega, double delta, double DELTA, double D0, double d0, double L0,  double e1, double u1)
        {
            Omega = omega;
            Delta = delta;
            this.Delta = DELTA;
            this.D0 = D0;
            this.d0 = d0;
            this.L0 = L0;
            this.e1 = e1;            
            U1 = u1;

            (S0, LAMDA0) = InitialiseS0LAMDA0(D0, d0, L0);
        }
        public double S0 { get; }
        public double LAMDA0 { get; }
        public double U1 { get; }

        

        private (double S0, double LAMDA0) InitialiseS0LAMDA0(double D0, double d0, double L0)
        {
            var s0 = 2 * (Math.PI * Math.Pow(D0, 2) / 4 - 7 * Math.PI * Math.Pow(d0, 2) / 4) + Math.PI * D0 * L0 + 7 * Math.PI * d0 * L0;
            var lamda0 = (Math.PI * Math.Pow(D0, 2) / 4 - 7 * Math.PI * Math.Pow(d0, 2) / 4) * L0;
            return (s0, lamda0);
        }          
    }
}

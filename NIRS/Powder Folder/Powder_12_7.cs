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
        public IBurningPowdersSize BurningPowdersSize { get; }
        public Powder_12_7(IConstParameters constParameters, IBarrelSize barrelSize, double omega)
        {
            Omega = omega;
            Delta = constParameters.delta;
            this.DELTA = omega/barrelSize.Wkm;
            this.D0 = constParameters.D0;
            this.d0 = constParameters.d0;
            this.L0 = constParameters.L0;
            this.e1 = constParameters.e1;            
            U1 = constParameters.u1;

            (S0, LAMDA0) = InitialiseS0LAMDA0(D0, d0, L0);

            BurningPowdersSize = new BurningPowdersSize_12_7(this, constParameters);
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

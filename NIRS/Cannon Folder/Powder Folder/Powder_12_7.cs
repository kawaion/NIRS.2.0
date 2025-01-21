using System;
using NIRS.Data_Parameters.Input_Data_Parameters;

namespace NIRS.Cannon_Folder.Powder_Folder
{
    class Powder_12_7 : IPowder
    {
        public Powder_12_7(double D0, double d0, double L0, double u1, double e1)
        {
            InitialiseParameters(D0, d0, L0, e1);
            (S0, LAMDA0) = InitialiseS0LAMDA0(D0, d0, L0);
            PsiS = InitialisePsiS(Kappa, Lamda, Mu);
            U1 = u1;
        }
        public double Kappa { get; set; }
        public double Lamda { get; set; }
        public double Mu { get; set; }
        public double S0 { get; set; }
        public double LAMDA0 { get; set; }
        public double PsiS { get; set; }
        public double U1 { get; set; }

        private void InitialiseParameters(double D0, double d0, double L0, double e1)
        {
            

            (var P,var Q,var beta) = InitialisePQBeta(D0, d0, L0, e1);
            (Kappa,Lamda,Mu) = InitialiseKappaLamdaMu(P, Q, beta);
            
            
        }        
        private (double S0, double LAMDA0) InitialiseS0LAMDA0(double D0, double d0, double L0)
        {
            var s0 = 2 * (Math.PI * Math.Pow(D0, 2) / 4 - 7 * Math.PI * Math.Pow(d0, 2) / 4) + Math.PI * D0 * L0 + 7 * Math.PI * d0 * L0;
            var lamda0 = (Math.PI * Math.Pow(D0, 2) / 4 - 7 * Math.PI * Math.Pow(d0, 2) / 4) * L0;
            return (s0, lamda0);
        }        
        private (double P, double Q, double Beta) InitialisePQBeta(double D0, double d0, double L0, double e1)
        {
            double P = (D0 + 7 * d0) / L0;
            double Q = (Math.Pow(D0, 2) - 7 * Math.Pow(d0, 2)) / Math.Pow(L0, 2);
            double beta = -2 * e1 / L0;
            return (P, Q, beta);
        }
        private (double Kappa, double Lamda, double Mu) InitialiseKappaLamdaMu(double P, double Q, double beta)
        {
            double kappa = (Q + 2 * P) / Q * beta;
            double lamda = 2 * (3 - P) / (Q + 2 * P) * beta;
            double mu = 6 * Math.Pow(beta, 2) / (Q + 2 * P);
            return (kappa, lamda, mu);
        }
        private double InitialisePsiS(double Kappa, double Lamda, double Mu)
        {
            return Kappa * (1 + Lamda + Mu);
        }    
    }
}

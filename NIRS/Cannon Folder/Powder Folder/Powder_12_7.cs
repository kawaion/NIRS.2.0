using NIRS.ConstParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.CannonFolder.PowderFolder
{
    class Powder_12_7 : Powder
    {
        public Powder_12_7(double D0, double d0, double L0) : base(D0,d0,L0)
        {
            InitialiseParameters(_D0, _d0, _L0);
        }
        public override double Kappa { get; set; }
        public override double Lamda { get; set; }
        public override double Mu { get; set; }
        public override double PsiS { get; set; }
        public override double S0 { get; set; }
        public override double LAMDA0 { get; set; }
        public override double KappaP { get; set; }

        public override double Psi(double z)
        {
            return Kappa * z * (1 + Lamda * z + Mu * Math.Pow(z, 2));
        }
        public override double Sigma(double z, double psi)
        {
            if (z < 1)
                return 1.0 + 2.0 * Lamda * z + 3.0 * Mu * Math.Pow(z, 2);

            if (psi <= 1)
                return (1.0 + 2.0 * Lamda + 3.0 * Mu) * Math.Sqrt(Math.Abs((1.0 - psi)
                    / (1.0 - PsiS)));

            throw new ArgumentException();
        }

        protected override (double Kappa, double Lamda, double Mu, double KappaP) InitialiseKappaLadaMu(double P, double Q, double beta)
        {
            double kappa = (Q + 2 * P) / Q * beta;
            double lamda = 2 * (3 - P) / (Q + 2 * P) * beta;
            double mu = 6 * Math.Pow(beta, 2) / (Q + 2 * P);
            double kappaP = Kappa - 0.5 * Kappa * Mu;
            return (kappa, lamda, mu, kappaP);
        }

        protected override void InitialiseParameters(double D0, double d0, double L0)
        {
            (P, Q, beta) = InitialisePQBeta(D0, d0, L0);
            (Kappa,Lamda,Mu,KappaP) = InitialiseKappaLadaMu(P, Q, beta);
            InitialisePsiS();
        }
        private void InitialisePsiS()
        {
            PsiS = Kappa * (1 + Lamda + Mu);
        }

        protected override (double P, double Q, double Beta) InitialisePQBeta(double D0, double d0, double L0)
        {
            double P = (D0 + 7 * d0) / L0;
            double Q = (Math.Pow(D0, 2) - 7 * Math.Pow(d0, 2)) / Math.Pow(L0, 2);
            double beta = -2 * e1 / L0;
            return (P, Q, beta);
        }

        protected override void InitialiseS0LAMDA0(double D0, double d0, double L0)
        {
            S0 = 2 * (Math.PI * Math.Pow(D0, 2) / 4 - 7 * Math.PI * Math.Pow(d0, 2) / 4) + Math.PI * D0 * L0 + 7 * Math.PI * d0 * L0;
            LAMDA0 = (Math.PI * Math.Pow(D0, 2) / 4 - 7 * Math.PI * Math.Pow(d0, 2) / 4) * L0;
        }

        public override double Uk(double p)
        {
            return ConstPowder.u1* p;
        }
    }
}

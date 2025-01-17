using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.CannonFolder.PowderFolder
{
    abstract class Powder
    {
        protected readonly double _d0;
        protected readonly double _D0;
        protected readonly double _L0;

        public Powder (double D0, double d0, double L0)
        {
            _D0 = D0;
            _d0 = d0;
            _L0 = L0;
            e1 = _d0;
        }
        public double e1 { get; }
        protected abstract void InitialiseParameters(double D0, double d0, double L0);

        protected abstract void InitialiseS0LAMDA0(double D0, double d0, double L0);
        protected abstract (double P, double Q, double Beta) InitialisePQBeta(double D0, double d0, double L0);
        protected abstract (double Kappa, double Lamda, double Mu, double KappaP) InitialiseKappaLadaMu(double P, double Q, double beta);

        protected double P;
        protected double Q;
        protected double beta;

        public abstract double Kappa { get; set; }
        public abstract double Lamda { get; set; }
        public abstract double Mu { get; set; }

        public abstract double KappaP { get; set; }

        public abstract double PsiS { get; set; }
        public abstract double S0 { get; set; }
        public abstract double LAMDA0 { get; set; }

        public abstract double Psi(double z);

        public abstract double Sigma(double z, double psi);
        public abstract double Uk(double p);
    }
}

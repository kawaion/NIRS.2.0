using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Cannon_Folder.Powder_Folder
{
    class BurningPowdersSize_12_7 : IBurningPowdersSize
    {
        private readonly IConstParameters _constP;

        private double kappa;
        private double lamda;
        private double mu;
        private double psiS;

        public BurningPowdersSize_12_7(IMainData mainData)
        {
            IPowder powder = mainData.Powder;
            (kappa,lamda,mu)=InitialiseKappaLamdaMu(powder);
            psiS = InitialisePsiS(kappa, lamda, mu);
        }
        public double Psi(double z)
        {
            var psi = kappa * z * (1 + lamda * z + mu * Math.Pow(z, 2));
            psi = PsiValidation(psi);
            return psi;
        }
        public double Sigma(double z, double psi)
        {
            if (z < 1)
                return 1.0 + 2.0 * lamda * z + 3.0 * mu * Math.Pow(z, 2);

            if (psi <= 1)
                return (1.0 + 2.0 * lamda + 3.0 * mu) 
                        * Math.Sqrt( Math.Abs( 
                                                (1.0 - psi) / (1.0 - psiS) 
                                             ) 
                                   );

            throw new ArgumentException();
        }
        public double z(double z)
        {
            throw new NotImplementedException();
        }
        public double Uk(double p)
        {
            return _constP.u1 * p;
        }

        private static double PsiValidation(double psi)
        {
            if (psi > 1) 
                psi = 1;
            return psi;
        }
        private (double Kappa, double Lamda, double Mu) InitialiseKappaLamdaMu(IPowder powder)
        {
            (var P, var Q, var beta) = InitialisePQBeta(powder.D0, powder.d0, powder.L0, powder.e1);

            double kappa = (Q + 2 * P) / Q * beta;
            double lamda = 2 * (3 - P) / (Q + 2 * P) * beta;
            double mu = 6 * Math.Pow(beta, 2) / (Q + 2 * P);
            return (kappa, lamda, mu);
        }
        private (double P, double Q, double Beta) InitialisePQBeta(double D0, double d0, double L0, double e1)
        {
            double P = (D0 + 7 * d0) / L0;
            double Q = (Math.Pow(D0, 2) - 7 * Math.Pow(d0, 2)) / Math.Pow(L0, 2);
            double beta = -2 * e1 / L0;
            return (P, Q, beta);
        }
        private double InitialisePsiS(double kappa, double lamda, double mu)
        {
            return kappa * (1 + lamda + mu);
        }



    }
}

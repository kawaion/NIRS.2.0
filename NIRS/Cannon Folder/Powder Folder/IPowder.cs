

namespace NIRS.Cannon_Folder.Powder_Folder
{
    interface IPowder
    {
        double d0 { get; }
        double D0 { get; }
        double L0 { get; }
        double e1 { get; }
        void InitialiseParameters(double D0, double d0, double L0);

        void InitialiseS0LAMDA0(double D0, double d0, double L0);
        (double P, double Q, double Beta) InitialisePQBeta(double D0, double d0, double L0);
        (double Kappa, double Lamda, double Mu, double KappaP) InitialiseKappaLadaMu(double P, double Q, double beta);

        double P;
        double Q;
        double beta;

        double Kappa { get; set; }
        double Lamda { get; set; }
        double Mu { get; set; }

        double KappaP { get; set; }

        double PsiS { get; set; }
        double S0 { get; set; }
        double LAMDA0 { get; set; }

        double Psi(double z);

        double Sigma(double z, double psi);
    }
}

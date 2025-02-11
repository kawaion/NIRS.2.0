

namespace NIRS.Cannon_Folder.Powder_Folder
{
    public interface IPowder
    {
        double Kappa { get; set; }
        double Lamda { get; set; }
        double Mu { get; set; }
        double PsiS { get; set; }
        double S0 { get; set; }
        double LAMDA0 { get; set; }
        double U1 { get; set; }
    }
}

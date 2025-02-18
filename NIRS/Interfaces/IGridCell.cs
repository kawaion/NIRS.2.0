using NIRS.Parameter_Type;

namespace NIRS.Interfaces
{
    public interface IGridCell
    {
        double dynamic_m { get; set; }
        double M { get; set; }
        double v { get; set; }
        double w { get; set; }

        double r { get; set; }
        double e { get; set; }
        double eps { get; set; }
        double psi { get; set; }
        double z { get; set; }
        double a { get; set; }
        double m { get; set; }
        double p { get; set; }
        double ro { get; set; }
    }
}

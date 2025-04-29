using MyDouble;
using NIRS.Grid_Folder;
using NIRS.Parameter_names;

namespace NIRS.Interfaces
{
    public interface ISubGrid
    {
        LimitedDouble n { get; set; }

        SpaceCellWithK this[LimitedDouble i] { get; set; }
        LimitedDouble LastIndex(PN pn);
        double Last(PN pn);
        SpaceCellProjectile sn { get; set; }
    }
}

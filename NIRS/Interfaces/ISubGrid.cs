using MyDouble;
using NIRS.Parameter_names;

namespace NIRS.Interfaces
{
    public interface ISubGrid
    {
        LimitedDouble n { get; set; }

        IGridCellWithK this[LimitedDouble i] { get; set; }
        LimitedDouble LastIndex(PN pn);
        double Last(PN pn);
        IGridCellProjectile sn { get; set; }
    }
}

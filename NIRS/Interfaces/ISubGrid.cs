using MyDouble;
using NIRS.Parameter_names;

namespace NIRS.Interfaces
{
    public interface ISubGrid
    {
        LimitedDouble N { get; set; }
        IGridCell this[LimitedDouble i] { get; set; }
        LimitedDouble LastIndex();
        LimitedDouble LastIndex(PN pn);
        IGridCell Last();
        IGridCellProjectile sn { get; set; }
    }
}

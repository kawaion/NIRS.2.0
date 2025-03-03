using MyDouble;

namespace NIRS.Interfaces
{
    public interface ISubGrid
    {
        LimitedDouble N { get; set; }
        IGridCell this[LimitedDouble i] { get; set; }
        LimitedDouble LastIndex();
        IGridCell Last();
        IGridCellProjectile sn { get; set; }
    }
}

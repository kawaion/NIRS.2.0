using MyDouble;

namespace NIRS.Grid_Folder
{
    public interface ISubGrid
    {
        LimitedDouble N { get; set; }
        IGridCell this[LimitedDouble i] { get; set; } 
    }
}

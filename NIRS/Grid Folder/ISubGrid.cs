using MyDouble;

namespace NIRS.Grid_Folder
{
    interface ISubGrid
    {
        IGridCell this[LimitedDouble i] { get; set; } 
    }
}

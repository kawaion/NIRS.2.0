using MyDouble;

namespace NIRS.Grid_Folder
{
    interface IGrid
    {
        ISubGrid this[LimitedDouble i] { get;set; }
    }
}

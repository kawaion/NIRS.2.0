using MyDouble;

namespace NIRS.Grid_Folder
{
    public interface IGrid
    {
        ISubGrid this[LimitedDouble i] { get;set; }
    }
}

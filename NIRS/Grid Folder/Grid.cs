using MyDouble;

namespace NIRS.Grid_Folder
{
    abstract class Grid
    {
        public abstract SubGrid this[LimitedDouble i] { get;set; }
    }
}

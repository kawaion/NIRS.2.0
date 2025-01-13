using NIRS.MyDouble_Folder;

namespace NIRS.Grid_Folder
{
    abstract class Grid
    {
        public abstract SubGrid this[LimitedDouble n] { get;set; }
    }
}

using NIRS.Grid_Folder;
using NIRS.Visualization.Progress;

namespace NIRS.Interfaces
{
    interface INumericalMethod
    {
        IGrid Calculate();
        void ProgressActivate(Progresser progresser);
    }
}

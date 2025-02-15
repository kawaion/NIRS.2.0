using NIRS.Grid_Folder;
using NIRS.Interfaces;

namespace NIRS.Boundary_Interfaces
{
    interface IOutputDataTransmitter
    {
        IGrid GetOutputData(IGrid grid);
    }
}

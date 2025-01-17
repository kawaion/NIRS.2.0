using NIRS.Grid_Folder;

namespace NIRS.Boundary_Interfaces
{
    interface IOutputDataTransmitter
    {
        IGrid GetOutputData(IGrid grid);
    }
}

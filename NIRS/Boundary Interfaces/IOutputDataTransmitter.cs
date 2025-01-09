using NIRS.Grid_Folder;

namespace NIRS.Boundary_Interfaces
{
    interface IOutputDataTransmitter
    {
        Grid GetOutputData(Grid grid);
    }
}

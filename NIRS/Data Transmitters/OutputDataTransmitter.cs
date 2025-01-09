using NIRS.Boundary_Interfaces;
using NIRS.Grid_Folder;

namespace NIRS.Data_Transmitters
{
    class OutputDataTransmitter : IOutputDataTransmitter
    {
        public Grid GetOutputData(Grid grid)
        {
            return grid;
        }
    }
}

using NIRS.Boundary_Interfaces;
using NIRS.Grid_Folder;
using NIRS.Interfaces;

namespace NIRS.Data_Transmitters
{
    class OutputDataTransmitter : IOutputDataTransmitter
    {
        public IGrid GetOutputData(IGrid grid)
        {
            return grid;
        }
    }
}

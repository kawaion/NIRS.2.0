using NIRS.Boundary_Interfaces;
using NIRS.Data_Parameters.Input_Data_Parameters;

namespace NIRS.Data_Transmitters
{
    class InputDataTransmitter : IInputDataTransmitter
    {
        public (IInitialParameters, IConstParameters) GetInputData(IInitialParameters initialParameters, IConstParameters constParameters)
        {
            return (initialParameters, constParameters);
        }
    }
}

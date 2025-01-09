using NIRS.Boundary_Interfaces;
using NIRS.Data_Parameters.Input_Data_Parameters;

namespace NIRS.Data_Transmitters
{
    class InputDataTransmitter : IInputDataTransmitter
    {
        public (InitialParameters, ConstParameters) GetInputData(InitialParameters initialParameters, ConstParameters constParameters)
        {
            return (initialParameters, constParameters);
        }
    }
}

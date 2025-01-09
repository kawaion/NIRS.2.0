using NIRS.Data_Parameters.Input_Data_Parameters;

namespace NIRS.Boundary_Interfaces
{
    interface IInputDataTransmitter
    {
        (InitialParameters, ConstParameters) GetInputData(InitialParameters initialParameters, ConstParameters constParameters);
    }
}

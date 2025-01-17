using NIRS.Data_Parameters.Input_Data_Parameters;

namespace NIRS.Boundary_Interfaces
{
    interface IInputDataTransmitter
    {
        (IInitialParameters, IConstParameters) GetInputData(IInitialParameters initialParameters, IConstParameters constParameters);
    }
}

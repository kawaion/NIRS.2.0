using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Interfaces;

namespace NIRS.Boundary_Interfaces
{
    interface IInputDataTransmitter
    {
        (IInitialParameters, IConstParameters) GetInputData(IInitialParameters initialParameters, IConstParameters constParameters);
    }
}

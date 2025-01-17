using NIRS.Data_Parameters.Input_Data_Parameters;

namespace NIRS.Grid_Folder
{
    interface IGridBorderFiller
    {
        IGrid Fill(IGrid grid, IInitialParameters initialParameters, IConstParameters constParameters);
    }
}

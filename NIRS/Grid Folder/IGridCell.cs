using NIRS.Parameter_Type;

namespace NIRS.Grid_Folder
{
    interface IGridCell
    {
        DynamicCharacteristicsFlow D { get; set; }
        MixtureStateParameters M { get; set; }
    }
}

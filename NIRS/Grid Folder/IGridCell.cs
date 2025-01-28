using NIRS.Parameter_Type;

namespace NIRS.Grid_Folder
{
    public interface IGridCell
    {
        DynamicCharacteristicsFlow D { get; set; }
        MixtureStateParameters M { get; set; }
    }
}

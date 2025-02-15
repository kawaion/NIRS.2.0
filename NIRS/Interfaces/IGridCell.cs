using NIRS.Parameter_Type;

namespace NIRS.Interfaces
{
    public interface IGridCell
    {
        DynamicCharacteristicsFlow D { get; set; }
        MixtureStateParameters M { get; set; }
    }
}

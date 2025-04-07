using MyDouble;
using NIRS.Parameter_names;
using NIRS.Parameter_Type;

namespace NIRS.Interfaces
{
    public interface IGridCellWithK : IGridCell
    {
        LimitedDouble k { get; set; }
    }
}

using MyDouble;
using NIRS.Data_Parameters.Input_Data_Parameters;

namespace NIRS.Interfaces
{
    interface IGridBorderFiller
    {
        IGrid FillAtZeroTime(IGrid grid);
        IGrid FillBarrelBorders(IGrid grid, LimitedDouble n);
    }
}

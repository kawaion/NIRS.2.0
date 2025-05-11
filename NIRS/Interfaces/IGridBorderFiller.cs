using MyDouble;
using NIRS.Data_Parameters.Input_Data_Parameters;

namespace NIRS.Interfaces
{
    interface IGridBorderFiller
    {
        IGrid FillAtZeroTime(IGrid grid);
        IGrid FillBarrelBorders(IGrid grid, double n, bool isBeltIntact);
        IGrid FillProjectileAtFixedBorder(IGrid grid, double n, bool isBeltIntact);
        IGrid FillCoordinateProjectileAtFixedBorder(IGrid grid, double n, bool isBeltIntact);
        IGrid FillLastNodeOfMixture(IGrid grid, double n, bool isBeltIntact);
    }
}

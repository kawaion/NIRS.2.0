using MyDouble;
using NIRS.Data_Parameters.Input_Data_Parameters;

namespace NIRS.Interfaces
{
    interface IGridBorderFiller
    {
        IGrid FillAtZeroTime(IGrid grid, double KSn);
        IGrid FillBarrelBordersN(IGrid grid, LimitedDouble n);
        IGrid FillBarrelBordersK(IGrid grid, LimitedDouble n, LimitedDouble KDynamicLast);
        IGrid FillProjectileAtFixedBorder(IGrid grid, LimitedDouble n);
        IGrid FillCoordinateProjectileAtFixedBorder(IGrid grid, LimitedDouble n, bool isBeltIntact);
        IGrid FillLastNodeOfMixture(IGrid grid, LimitedDouble n);
    }
}



using MyDouble;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Helpers;
using NIRS.Interfaces;
using NIRS.Parameter_names;

namespace NIRS.Grid_Folder
{
    class GridBorderFiller : IGridBorderFiller
    {
        private readonly IBoundaryFunctions _boundaryFunctions;
        private readonly IBarrel b;

        private readonly KGetter k;

        public GridBorderFiller(IBoundaryFunctions boundaryFunctions, IMainData mainData)
        {
            _boundaryFunctions = boundaryFunctions;
            b = mainData.Barrel;

            KGetter K = new KGetter(mainData.ConstParameters);
        }
        public IGrid FillAtZeroTime(IGrid grid)
        {
            var K = GetK();
            var minus0Dot5 = new LimitedDouble(-0.5);
            var zero = new LimitedDouble(0);
            for (var k = new LimitedDouble(0); k <= K; k += 1)
            {
                grid[minus0Dot5][k].D.dynamic_m = _boundaryFunctions.GetDynamic_nMinus0Dot5(PN.dynamic_m, k);
                grid[minus0Dot5][k].D.M = _boundaryFunctions.GetDynamic_nMinus0Dot5(PN.M, k);
                grid[minus0Dot5][k].D.v = _boundaryFunctions.GetDynamic_nMinus0Dot5(PN.v, k);
                grid[minus0Dot5][k].D.w = _boundaryFunctions.GetDynamic_nMinus0Dot5(PN.w, k);
            }
            for (var k = new LimitedDouble(1); k <= K; k += 1)
            {
                grid[zero][k - 0.5].M.p = _boundaryFunctions.GetMixture_n0(PN.p, k - 0.5);
                grid[zero][k - 0.5].M.ro = _boundaryFunctions.GetMixture_n0(PN.ro, k - 0.5);
                grid[zero][k - 0.5].M.eps = _boundaryFunctions.GetMixture_n0(PN.eps, k - 0.5);
                grid[zero][k - 0.5].M.z = _boundaryFunctions.GetMixture_n0(PN.z, k - 0.5);
                grid[zero][k - 0.5].M.m = _boundaryFunctions.GetMixture_n0(PN.m, k - 0.5);
                grid[zero][k - 0.5].M.a = _boundaryFunctions.GetMixture_n0(PN.a, k - 0.5);
                grid[zero][k - 0.5].M.r = _boundaryFunctions.GetMixture_n0(PN.r, k - 0.5);
                grid[zero][k - 0.5].M.e = _boundaryFunctions.GetMixture_n0(PN.e, k - 0.5);
            }

            return grid;
        }

        private LimitedDouble GetK()
        {
            var xEndChamber = b.EndChamberPoint.X;
            var K = k[xEndChamber];
            return new LimitedDouble(K);
        }

        public IGrid FillBarrelBorders(IGrid grid, LimitedDouble n, IMainData mainData)
        {
            var zero = new LimitedDouble(0);
            var K = GetK();
            
            grid[n][zero].D.dynamic_m = _boundaryFunctions.GetDynamic_k0(PN.dynamic_m, n);
            grid[n][zero].D.M = _boundaryFunctions.GetDynamic_k0(PN.M, n);
            grid[n][zero].D.v = _boundaryFunctions.GetDynamic_k0(PN.v, n);
            grid[n][zero].D.w = _boundaryFunctions.GetDynamic_k0(PN.w, n);

            grid[n][K].D.dynamic_m = _boundaryFunctions.GetDynamic_K(PN.dynamic_m, n);
            grid[n][K].D.M = _boundaryFunctions.GetDynamic_K(PN.M, n);
            grid[n][K].D.v = _boundaryFunctions.GetDynamic_K(PN.v, n);
            grid[n][K].D.w = _boundaryFunctions.GetDynamic_K(PN.w, n);

            return grid;
        }
    }

}
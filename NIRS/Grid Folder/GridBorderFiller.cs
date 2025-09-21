

using MyDouble;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Helpers;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Windows.Forms.DataVisualization.Charting;

namespace NIRS.Grid_Folder
{
    class GridBorderFiller : IGridBorderFiller
    {
        private readonly IBoundaryFunctions _boundaryFunctions;
        private readonly IBarrel b;
        private readonly IBarrelSize bs;

        private readonly KGetter k;
        private readonly XGetter x;

        public GridBorderFiller(IBoundaryFunctions boundaryFunctions, IMainData mainData)
        {
            _boundaryFunctions = boundaryFunctions;
            b = mainData.Barrel;
            bs = b.BarrelSize;

            k = new KGetter(mainData.ConstParameters);
            x = new XGetter(mainData.ConstParameters);
        }
        public IGrid FillAtZeroTime(IGrid grid, double KSn)
        {
            var minus05 = new LimitedDouble(-0.5);
            var zero = new LimitedDouble(0);

            for (var k = new LimitedDouble(-1); k <= KSn; k += 1)
            {
                grid[PN.dynamic_m, minus05, k] = _boundaryFunctions.GetDynamic_nMinus0Dot5(PN.dynamic_m, k);
                grid[PN.M, minus05, k] = _boundaryFunctions.GetDynamic_nMinus0Dot5(PN.M, k);
                grid[PN.v, minus05, k] = _boundaryFunctions.GetDynamic_nMinus0Dot5(PN.v, k);
                grid[PN.w, minus05, k] = _boundaryFunctions.GetDynamic_nMinus0Dot5(PN.w, k);
            }
            for (var k = new LimitedDouble(0); k <= KSn + 0.5; k += 1)
            {
                grid[PN.p, zero, k - 0.5] = _boundaryFunctions.GetMixture_n0(PN.p, k - 0.5);
                grid[PN.rho, zero, k - 0.5] = _boundaryFunctions.GetMixture_n0(PN.rho, k - 0.5);
                grid[PN.z, zero, k - 0.5] = _boundaryFunctions.GetMixture_n0(PN.z, k - 0.5);
                grid[PN.psi, zero, k - 0.5] = _boundaryFunctions.GetMixture_n0(PN.psi, k - 0.5);
                grid[PN.m, zero, k - 0.5] = _boundaryFunctions.GetMixture_n0(PN.m, k - 0.5);
                grid[PN.a, zero, k - 0.5] = _boundaryFunctions.GetMixture_n0(PN.a, k - 0.5);
                grid[PN.r, zero, k - 0.5] = _boundaryFunctions.GetMixture_n0(PN.r, k - 0.5);
                grid[PN.e, zero, k - 0.5] = _boundaryFunctions.GetMixture_n0(PN.e, k - 0.5);
            }
            grid.SetSn(PN.x, zero,      b.EndChamberPoint.X);
            grid.SetSn(PN.x, minus05,    b.EndChamberPoint.X);
            grid.SetSn(PN.vSn, minus05,    0);
            grid.SetSn(PN.r, zero, _boundaryFunctions.GetMixture_n0(PN.r, KSn));
            grid.SetSn(PN.m, zero, _boundaryFunctions.GetMixture_n0(PN.m, KSn));

            return grid;
        }

        private double GetK()
        {
            var xEndChamber = b.EndChamberPoint.X;
            var K = k[xEndChamber];
            //K = Math.Floor(K);
            return K;
        }

        public IGrid FillBarrelBordersN(IGrid grid, LimitedDouble n)
        {

            if (n.IsHalfInt())
            {
                var zero = new LimitedDouble(0);
                var minus1 = new LimitedDouble(-1);

                grid[PN.dynamic_m, n, zero] = _boundaryFunctions.GetDynamic_k0(PN.dynamic_m, n);
                grid[PN.M, n, zero] = _boundaryFunctions.GetDynamic_k0(PN.M, n);
                grid[PN.v, n, zero] = _boundaryFunctions.GetDynamic_k0(PN.v, n);
                grid[PN.w, n, zero] = _boundaryFunctions.GetDynamic_k0(PN.w, n);

                grid[PN.dynamic_m, n, minus1] = _boundaryFunctions.GetDynamic_k0(PN.dynamic_m, n);
                grid[PN.M, n, minus1] = _boundaryFunctions.GetDynamic_k0(PN.M, n);
                grid[PN.v, n, minus1] = _boundaryFunctions.GetDynamic_k0(PN.v, n);
                grid[PN.w, n, minus1] = _boundaryFunctions.GetDynamic_k0(PN.w, n);
            }
            if (n.IsInt())
            {
                var minus05 = new LimitedDouble(-0.5);
                grid[PN.p, n, minus05] = 0;
                grid[PN.rho, n, minus05] = 0;
                grid[PN.z, n, minus05] = 0;
                grid[PN.psi, n, minus05] = 0;
                grid[PN.m, n, minus05] = 0;
                grid[PN.a, n, minus05] = 0;
                grid[PN.r, n, minus05] = 0;
                grid[PN.e, n, minus05] = 0;
            }

            return grid;
        }
        public IGrid FillBarrelBordersK(IGrid grid, LimitedDouble n, LimitedDouble KDynamicLast)
        {

            if (n.IsHalfInt())
            {
                var K = KDynamicLast;

                grid[PN.dynamic_m, n, K] = _boundaryFunctions.GetDynamic_K(PN.dynamic_m, n);
                grid[PN.M, n, K] = _boundaryFunctions.GetDynamic_K(PN.M, n);
                grid[PN.v, n, K] = _boundaryFunctions.GetDynamic_K(PN.v, n);
                grid[PN.w, n, K] = _boundaryFunctions.GetDynamic_K(PN.w, n);

            }

            return grid;
        }
        public IGrid FillProjectileAtFixedBorder(IGrid grid, LimitedDouble n)
        {
            if (n.IsHalfInt())
            {
                var K = grid.LastIndexK(PN.dynamic_m, n);

                grid.SetSn(PN.dynamic_m, n, grid[PN.dynamic_m, n, K]);
                grid.SetSn(PN.M, n, grid[PN.M, n, K]);
                grid.SetSn(PN.v, n, 0);
                grid.SetSn(PN.w, n, grid[PN.w, n, K]);
            }
            if (n.IsInt())
            {
                var K = grid.LastIndexK(PN.p, n);

                grid.SetSn(PN.p, n, grid[PN.p, n, K - 0.5]);
                grid.SetSn(PN.rho, n, grid[PN.rho, n, K - 0.5]);
                grid.SetSn(PN.z, n, grid[PN.z, n, K - 0.5]);
                grid.SetSn(PN.psi, n, grid[PN.psi, n, K - 0.5]);
                grid.SetSn(PN.a, n, grid[PN.a, n, K - 0.5]);
                grid.SetSn(PN.m, n, grid[PN.m, n, K - 0.5]);
                grid.SetSn(PN.e, n, grid[PN.e, n, K - 0.5]);
                grid.SetSn(PN.r, n, grid.GetSn(PN.rho, n) * grid.GetSn(PN.m, n) * bs.S(x[K - 0.5]));
            }
            grid.SetSn(PN.x, n, b.EndChamberPoint.X);

            return grid;
        }
        public IGrid FillCoordinateProjectileAtFixedBorder(IGrid grid, LimitedDouble n, bool isBeltIntact)
        {
            if (isBeltIntact)
            {
                grid.SetSn(PN.x, n,     b.EndChamberPoint.X);
            }
            return grid;
        }
        public IGrid FillLastNodeOfMixture(IGrid grid, LimitedDouble n)
        {
            if (n.IsInt())
            {
                var parameters = VectorPN.mixture;
                foreach (var pn in parameters)
                {                
                    var K = grid.LastIndexK(pn, n);
                    var KMinus1 = K - 1;
                    var KPlus1 = K + 1;

                    var p1 = new Point2D(KMinus1.GetDouble(), grid[pn, n, KMinus1]);
                    var p2 = new Point2D(K.GetDouble(), grid[pn, n, K]);
                    EquationOfLineFromTwoPoints equation = new EquationOfLineFromTwoPoints(p1, p2);
                    grid[pn, n, KPlus1] = equation.GetY(KPlus1.GetDouble());
                }
            }
            return grid;     
        }

    }

}
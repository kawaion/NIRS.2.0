

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
            for (var k = -1; k <= KSn; k += 1)
            {
                grid[PN.dynamic_m, -0.5, k] = _boundaryFunctions.GetDynamic_nMinus0Dot5(PN.dynamic_m, k);
                grid[PN.M, -0.5, k] = _boundaryFunctions.GetDynamic_nMinus0Dot5(PN.M, k);
                grid[PN.v, -0.5, k] = _boundaryFunctions.GetDynamic_nMinus0Dot5(PN.v, k);
                grid[PN.w, -0.5, k] = _boundaryFunctions.GetDynamic_nMinus0Dot5(PN.w, k);
            }
            for (var k = 0; k <= KSn + 0.5; k += 1)
            {
                grid[PN.p, 0, k - 0.5] = _boundaryFunctions.GetMixture_n0(PN.p, k - 0.5);
                grid[PN.ro, 0, k - 0.5] = _boundaryFunctions.GetMixture_n0(PN.ro, k - 0.5);
                grid[PN.z, 0, k - 0.5] = _boundaryFunctions.GetMixture_n0(PN.z, k - 0.5);
                grid[PN.psi, 0, k - 0.5] = _boundaryFunctions.GetMixture_n0(PN.psi, k - 0.5);
                grid[PN.m, 0, k - 0.5] = _boundaryFunctions.GetMixture_n0(PN.m, k - 0.5);
                grid[PN.a, 0, k - 0.5] = _boundaryFunctions.GetMixture_n0(PN.a, k - 0.5);
                grid[PN.r, 0, k - 0.5] = _boundaryFunctions.GetMixture_n0(PN.r, k - 0.5);
                grid[PN.e, 0, k - 0.5] = _boundaryFunctions.GetMixture_n0(PN.e, k - 0.5);
            }
            grid.SetSn(PN.x, 0,      b.EndChamberPoint.X);
            grid.SetSn(PN.x, -0.5,    b.EndChamberPoint.X);
            grid.SetSn(PN.vSn, -0.5,    0);

            return grid;
        }

        private double GetK()
        {
            var xEndChamber = b.EndChamberPoint.X;
            var K = k[xEndChamber];
            //K = Math.Floor(K);
            return K;
        }

        public IGrid FillBarrelBorders(IGrid grid, double n, bool isBeltIntact, double KDynamicLast)
        {

            if (n.IsHalfInt())
            {   
                grid[PN.dynamic_m, n, 0] = _boundaryFunctions.GetDynamic_k0(PN.dynamic_m, n);
                grid[PN.M, n, 0] = _boundaryFunctions.GetDynamic_k0(PN.M, n);
                grid[PN.v, n, 0] = _boundaryFunctions.GetDynamic_k0(PN.v, n);
                grid[PN.w, n, 0] = _boundaryFunctions.GetDynamic_k0(PN.w, n);

                grid[PN.dynamic_m, n, -1] = _boundaryFunctions.GetDynamic_k0(PN.dynamic_m, n);
                grid[PN.M, n, -1] = _boundaryFunctions.GetDynamic_k0(PN.M, n);
                grid[PN.v, n, -1] = _boundaryFunctions.GetDynamic_k0(PN.v, n);
                grid[PN.w, n, -1] = _boundaryFunctions.GetDynamic_k0(PN.w, n);

                if (isBeltIntact)
                {
                    var K = KDynamicLast;

                    grid[PN.dynamic_m, n, K] = _boundaryFunctions.GetDynamic_K(PN.dynamic_m, n);
                    grid[PN.M, n, K] = _boundaryFunctions.GetDynamic_K(PN.M, n);
                    grid[PN.v, n, K] = _boundaryFunctions.GetDynamic_K(PN.v, n);
                    grid[PN.w, n, K] = _boundaryFunctions.GetDynamic_K(PN.w, n);
                }
            }
            if (n.IsInt())
            {
                grid[PN.p, n, -0.5] = 0;
                grid[PN.ro, n, -0.5] = 0;
                grid[PN.z, n, -0.5] = 0;
                grid[PN.psi, n, -0.5] = 0;
                grid[PN.m, n, -0.5] = 0;
                grid[PN.a, n, -0.5] = 0;
                grid[PN.r, n, -0.5] = 0;
                grid[PN.e, n, -0.5] = 0;
            }

            return grid;
        }
        public IGrid FillProjectileAtFixedBorder(IGrid grid, double n)
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
                grid.SetSn(PN.ro, n, grid[PN.ro, n, K - 0.5]);
                grid.SetSn(PN.z, n, grid[PN.z, n, K - 0.5]);
                grid.SetSn(PN.psi, n, grid[PN.psi, n, K - 0.5]);
                grid.SetSn(PN.a, n, grid[PN.a, n, K - 0.5]);
                grid.SetSn(PN.m, n, grid[PN.m, n, K - 0.5]);
                grid.SetSn(PN.e, n, grid[PN.e, n, K - 0.5]);
                grid.SetSn(PN.r, n, grid.GetSn(PN.ro, n) * grid.GetSn(PN.m, n) * bs.S(x[K - 0.5]));
            }
            grid.SetSn(PN.x, n, b.EndChamberPoint.X);

            return grid;
        }
        public IGrid FillCoordinateProjectileAtFixedBorder(IGrid grid, double n, bool isBeltIntact)
        {
            if (isBeltIntact)
            {
                grid.SetSn(PN.x, n,     b.EndChamberPoint.X);
            }
            return grid;
        }
        public IGrid FillLastNodeOfMixture(IGrid grid, double n, bool isBeltIntact)
        {
            if (isBeltIntact && n.IsInt())
            {
                var K = grid.LastIndexK(PN.r, n);
                var KMinus1 = K - 1;
                var KPlus1 = K + 1;

                var parameters = new List<PN> { PN.r, PN.z, PN.a, PN.m, PN.ro, PN.e, PN.p, PN.psi };
                foreach (var pn in parameters)
                {
                    var p1 = new Point2D(KMinus1, grid[pn, n, KMinus1]);
                    var p2 = new Point2D(K, grid[pn, n, K]);
                    EquationOfLineFromTwoPoints equation = new EquationOfLineFromTwoPoints(p1, p2);
                    grid[pn, n, KPlus1] = equation.GetY(KPlus1);
                }
            }
            return grid;     
        }

    }

}
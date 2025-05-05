

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
        public IGrid FillAtZeroTime(IGrid grid)
        {
            var K = GetK();
            var minus0Dot5 = new LimitedDouble(-0.5);
            var zero = new LimitedDouble(0);
            for (var k = new LimitedDouble(-1); k <= K; k += 1)
            {
                grid[minus0Dot5][k].dynamic_m = _boundaryFunctions.GetDynamic_nMinus0Dot5(PN.dynamic_m, k);
                grid[minus0Dot5][k].M = _boundaryFunctions.GetDynamic_nMinus0Dot5(PN.M, k);
                grid[minus0Dot5][k].v = _boundaryFunctions.GetDynamic_nMinus0Dot5(PN.v, k);
                grid[minus0Dot5][k].w = _boundaryFunctions.GetDynamic_nMinus0Dot5(PN.w, k);
            }
            for (var k = new LimitedDouble(0); k <= K; k += 1)
            {
                grid[zero][k - 0.5].p = _boundaryFunctions.GetMixture_n0(PN.p, k - 0.5);
                grid[zero][k - 0.5].ro = _boundaryFunctions.GetMixture_n0(PN.ro, k - 0.5);
                grid[zero][k - 0.5].z = _boundaryFunctions.GetMixture_n0(PN.z, k - 0.5);
                grid[zero][k - 0.5].psi = _boundaryFunctions.GetMixture_n0(PN.psi, k - 0.5);
                grid[zero][k - 0.5].m = _boundaryFunctions.GetMixture_n0(PN.m, k - 0.5);
                grid[zero][k - 0.5].a = _boundaryFunctions.GetMixture_n0(PN.a, k - 0.5);
                grid[zero][k - 0.5].r = _boundaryFunctions.GetMixture_n0(PN.r, k - 0.5);
                grid[zero][k - 0.5].e = _boundaryFunctions.GetMixture_n0(PN.e, k - 0.5);
            }
            grid[zero].sn.x = b.EndChamberPoint.X;
            grid[minus0Dot5].sn.x = b.EndChamberPoint.X;

            return grid;
        }

        private LimitedDouble GetK()
        {
            var xEndChamber = b.EndChamberPoint.X;
            var K = k[xEndChamber];
            K = Math.Floor(K);
            return new LimitedDouble(K);
        }

        public IGrid FillBarrelBorders(IGrid grid, LimitedDouble n, bool isBeltIntact)
        {

            if (n.IsHalfInt())
            {   
                var zero = new LimitedDouble(0);
                var minus1 = new LimitedDouble(-1);


                grid[n][zero].dynamic_m = _boundaryFunctions.GetDynamic_k0(PN.dynamic_m, n);
                grid[n][zero].M = _boundaryFunctions.GetDynamic_k0(PN.M, n);
                grid[n][zero].v = _boundaryFunctions.GetDynamic_k0(PN.v, n);
                grid[n][zero].w = _boundaryFunctions.GetDynamic_k0(PN.w, n);

                grid[n][minus1].dynamic_m = _boundaryFunctions.GetDynamic_k0(PN.dynamic_m, n);
                grid[n][minus1].M = _boundaryFunctions.GetDynamic_k0(PN.M, n);
                grid[n][minus1].v = _boundaryFunctions.GetDynamic_k0(PN.v, n);
                grid[n][minus1].w = _boundaryFunctions.GetDynamic_k0(PN.w, n);

                if (isBeltIntact)
                {
                    var K = GetK();

                    grid[n][K].dynamic_m = _boundaryFunctions.GetDynamic_K(PN.dynamic_m, n);
                    grid[n][K].M = _boundaryFunctions.GetDynamic_K(PN.M, n);
                    grid[n][K].v = _boundaryFunctions.GetDynamic_K(PN.v, n);
                    grid[n][K].w = _boundaryFunctions.GetDynamic_K(PN.w, n);
                }
            }
            if (n.IsInt())
            {
                var minus0dot5 = new LimitedDouble(-0.5);

                grid[n][minus0dot5].p = 0;
                grid[n][minus0dot5].ro = 0;
                grid[n][minus0dot5].z = 0;
                grid[n][minus0dot5].psi = 0;
                grid[n][minus0dot5].m = 0;
                grid[n][minus0dot5].a = 0;
                grid[n][minus0dot5].r = 0;
                grid[n][minus0dot5].e = 0;
            }

            return grid;
        }
        public IGrid FillProjectileAtFixedBorder(IGrid grid, LimitedDouble n, bool isBeltIntact)
        {
            if (isBeltIntact)
            {
                var K = GetK();
                if (n.IsHalfInt())
                {
                    grid[n].sn.dynamic_m = grid[n][K].dynamic_m;
                    grid[n].sn.M = grid[n][K].M;
                    grid[n].sn.v = 0;
                    grid[n].sn.w = grid[n][K].w;
                }
                if (n.IsInt())
                {
                    grid[n].sn.p = grid[n][K - 0.5].p;
                    grid[n].sn.ro = grid[n][K - 0.5].ro;
                    grid[n].sn.z = grid[n][K - 0.5].z;
                    grid[n].sn.psi = grid[n][K - 0.5].psi;
                    grid[n].sn.a = grid[n][K - 0.5].a;
                    grid[n].sn.m = grid[n][K - 0.5].m;
                    grid[n].sn.e = grid[n][K - 0.5].e;
                    grid[n].sn.r = grid[n].sn.ro * grid[n].sn.m * bs.S(x[K - 0.5]);
                }
            }
            return grid;
        }
        public IGrid FillCoordinateProjectileAtFixedBorder(IGrid grid, LimitedDouble n, bool isBeltIntact)
        {
            if (isBeltIntact)
            {
                grid[n].sn.x = b.EndChamberPoint.X;
            }
            return grid;
        }
        public IGrid FillLastNodeOfMixture(IGrid grid, LimitedDouble n, bool isBeltIntact)
        {
            if (isBeltIntact && n.IsInt())
            {
                var K = GetK()-0.5;
                var KMinus1 = K - 1;
                var KPlus1 = K + 1;
                var tmp = k[b.EndChamberPoint.X];
                if (KPlus1 <= k[b.EndChamberPoint.X])
                {
                    var parameters = new List<PN> { PN.r, PN.z, PN.a, PN.m, PN.ro, PN.e, PN.p, PN.psi };
                    foreach (var pn in parameters)
                    {
                        var p1 = new Point2D(KMinus1.Value, grid[n][KMinus1][pn]);
                        var p2 = new Point2D(K.Value, grid[n][K][pn]);
                        EquationOfLineFromTwoPoints equation = new EquationOfLineFromTwoPoints(p1, p2);
                        grid[n][KPlus1][pn] = equation.GetY(KPlus1.Value);
                    }
                }                   
            }
            return grid;     
        }

    }

}
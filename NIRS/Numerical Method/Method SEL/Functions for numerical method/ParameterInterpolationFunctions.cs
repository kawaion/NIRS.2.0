using MyDouble;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Grid_Folder;
using NIRS.Helpers;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Functions_for_numerical_method
{
    public class ParameterInterpolationFunctions : IParameterInterpolationFunctions
    {
        private IGrid g;
        private readonly IConstParameters constP;
        private readonly IBarrelSize bs;
        private readonly XGetter x;
        private readonly Differencial d;
        private InterpolateStep step;

        public ParameterInterpolationFunctions(IGrid grid, IMainData mainData)
        {
            x = new XGetter(mainData.ConstParameters);
            g = grid;
            constP = mainData.ConstParameters;
            bs = mainData.Barrel.BarrelSize;

            d = new Differencial(grid, mainData.ConstParameters);
            step = new InterpolateStep(grid); 
        }
        private double StraightLineInterpolation(IGrid g, PN pn, LimitedDouble n, double xSn, LimitedDouble kLast)
        {
            var p1 = new Point2D(x[kLast], g[pn, n, kLast]);
            var p2 = new Point2D(xSn, g.GetSn(pn, n));
            EquationOfLineFromTwoPoints equationOfLineFromTwoPoints = new EquationOfLineFromTwoPoints(p1, p2);

            kLast++;

            return equationOfLineFromTwoPoints.GetY(x[kLast]);
        }


        public double InterpolateMixture(PN pn, LimitedDouble n, double xSn, LimitedDouble kLast)
        {
            return StraightLineInterpolation(g, pn, n, xSn, kLast);
        }   
        

        public double InterpolateDynamic(PN pn, LimitedDouble n, LimitedDouble kLast, double xSn)
        {
            switch (pn)
            {
                case PN.v: return Interpolate_v(n, kLast, xSn);
                case PN.w: return Interpolate_w(n, kLast, xSn);
                case PN.dynamic_m: return Interpolate_dynamic_m(n, kLast + 1);
                case PN.M: return Interpolate_M(n, kLast + 1);
            }
            throw new Exception();
        }      
        public double Interpolate_v(LimitedDouble n, LimitedDouble kLast, double xSn)
        {
            return StraightLineInterpolation(g, PN.v, n, xSn, kLast);
        }
        public double Interpolate_w(LimitedDouble n, LimitedDouble kLast, double xSn)
        {
            return StraightLineInterpolation(g, PN.w, n, xSn, kLast);
        }
        public double Interpolate_dynamic_m(LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.AppointAndOffset(N, + 0.5, K, + 1);

            var opt = ChooseACalculationOptionFor_m_M(n, k);
            if(opt == Option.opt1)
            {
                return g[PN.v, n + 0.5, k + 1] *
                       (g[PN.r, n, k + 0.5] + g[PN.r, n, k + 1.5]) 
                       / 2;
            }
            if (opt == Option.opt2)
            {
                return g[PN.v, n + 0.5, k + 1] *
                       (g[PN.r, n, k + 0.5] + g.GetSn(PN.r, n))
                       / 2;
            }
            if (opt == Option.opt3)
            {
                throw new Exception();
                //return g[n + 0.5][k + 2].v *
                //       (g[n][k + 0.5].r + g[n].sn.r)
                //       / 2;
            }
            throw new Exception();
        }
        public double Interpolate_M(LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.AppointAndOffset(N, + 0.5, K, + 1);

            var opt = ChooseACalculationOptionFor_m_M(n, k);
            if (opt == Option.opt1)
            {
                return g[PN.w, n + 0.5, k + 1] * constP.PowderDelta *
                       (g[PN.One_minus_m, n, k + 0.5] * bs.S(x[k + 0.5]) + g[PN.One_minus_m, n, k + 1.5] * bs.S(x[k + 1.5]))
                       / 2;
            }
            if (opt == Option.opt2)
            {
                return g[PN.w, n + 0.5, k + 1] * constP.PowderDelta *
                       (g[PN.One_minus_m, n, k + 0.5] * bs.S(x[k + 0.5]) + (g.GetSn(PN.One_minus_m, n)) * bs.S(g.GetSn(PN.x, n)))
                       / 2;
            }
            if (opt == Option.opt3)
            {
                throw new Exception();
                //return g[n + 0.5][k + 2].w * constP.PowderDelta *
                //       ((1 - g[n][k + 0.5].m) * bs.S(x[k + 0.5]) + (1 - g[n].sn.m) * bs.S(g[n].sn.x))
                //       / 2;
            }
            throw new Exception();
        }        

        private Option ChooseACalculationOptionFor_m_M(LimitedDouble n, LimitedDouble k)
        {
            //if (k + 2 <= g[n + 0.5].sn.x / constP.h)
            //    return Option.opt3;
            //else 
            if (x[k + 1.5] <= g.GetSn(PN.x, n))
                return Option.opt1;
            else if (x[k + 1] <= g.GetSn(PN.x, n + 0.5))
                return Option.opt2;
            else if (x[k + 2] <= g.GetSn(PN.x, n + 0.5))
                return Option.opt3;

            throw new Exception();
        }

        public void Update(IGrid grid)
        {
            g = grid;
        }

        enum Option
        {
            opt1,
            opt2,
            opt3
        }
    }
}

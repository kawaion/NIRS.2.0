using MyDouble;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Grid_Folder;
using NIRS.Helpers;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using System;
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


        public double InterpolateMixture(PN pn, LimitedDouble n, LimitedDouble k)
        {
            var kLast = g[n].LastIndex(pn);

            return g[n][kLast][pn]
                   + d.dPNdx(n,pn)
                   * step.Get(n,k,pn) * constP.h;
        }   
        

        public double InterpolateDynamic(PN pn, LimitedDouble n, LimitedDouble k)
        {
            switch (pn)
            {
                case PN.v: return Interpolate_v(n, k);
                case PN.w: return Interpolate_w(n, k);
                case PN.dynamic_m: return Interpolate_dynamic_m(n, k);
                case PN.M: return Interpolate_M(n, k);
            }
            throw new Exception();
        }      
        public double Interpolate_v(LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.Appoint(N, K).Offset(N + 0.5, K + 1);

            return g[n + 0.5][k].v
                   + d.dvdx(n + 0.5)
                   * step.Get(n + 0.5, k + 1, PN.v) * constP.h;
        }
        public double Interpolate_w(LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.Appoint(N, K).Offset(N + 0.5, K + 1);

            return g[n + 0.5][k].w
                   + d.dwdx(n + 0.5)
                   * step.Get(n + 0.5, k + 1, PN.w) * constP.h;
        }
        public double Interpolate_dynamic_m(LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.Appoint(N, K).Offset(N + 0.5, K + 1);

            var opt = ChooseACalculationOptionFor_m_M(n, k);
            if(opt == Option.opt1)
            {
                return g[n + 0.5][k + 1].v *
                       (g[n][k + 0.5].r + g[n][k + 1.5].r) 
                       / 2;
            }
            if (opt == Option.opt2)
            {
                return g[n + 0.5][k + 1].v *
                       (g[n][k + 0.5].r + g[n].sn.r)
                       / 2;
            }
            //if (opt == Option.opt3)
            //{
            //    return g[n + 0.5][k + 2].v *
            //           (g[n][k + 0.5].r + g[n].sn.r)
            //           / 2;
            //}
            throw new Exception();
        }
        public double Interpolate_M(LimitedDouble N, LimitedDouble K)
        {
            (var n, var k) = OffseterNK.Appoint(N, K).Offset(N + 0.5, K + 1);

            var opt = ChooseACalculationOptionFor_m_M(n, k);
            if (opt == Option.opt1)
            {
                return g[n + 0.5][k + 1].w * constP.PowderDelta *
                       ((1-g[n][k + 0.5].m) * bs.S(x[k+0.5]) + (1 - g[n][k + 1.5].m) * bs.S(x[k + 1.5]))
                       / 2;
            }
            if (opt == Option.opt2)
            {
                return g[n + 0.5][k + 1].w * constP.PowderDelta *
                       ((1 - g[n][k + 0.5].m) * bs.S(x[k + 0.5]) + (1 - g[n].sn.m) * bs.S(g[n].sn.x))
                       / 2;
            }
            //if (opt == Option.opt3)
            //{
            //    return g[n + 0.5][k + 2].w * constP.delta *
            //           ((1 - g[n][k + 0.5].m) * bs.S(x[k + 0.5]) + (1 - g[n].sn.m) * bs.S(g[n].sn.x))
            //           / 2;
            //}
            throw new Exception();
        }        

        private Option ChooseACalculationOptionFor_m_M(LimitedDouble n, LimitedDouble k)
        {
            //if (k + 2 <= g[n + 0.5].sn.x / constP.h)
            //    return Option.opt3;
            //else 
            if (k + 1.5 <= g[n].sn.x / constP.h)
                return Option.opt1;
            else if (k + 1 <= g[n + 0.5].sn.x / constP.h)
                return Option.opt2;

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

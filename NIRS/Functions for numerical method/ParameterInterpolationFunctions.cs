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
        private readonly IGrid g;
        private readonly IConstParameters constP;

        private readonly XGetter x;
        private readonly Differencial d;
        public ParameterInterpolationFunctions(IGrid grid, IMainData mainData)
        {
            x = new XGetter(mainData.ConstParameters);
            g = grid;
            constP = mainData.ConstParameters;

            d = new Differencial(grid, mainData.ConstParameters);
        }
        public double Interpolate_v(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 0.5, k + 1);
            int inerpolateStep = GetInterpolateStep(n + 0.5, k, PN.v);

            return g[n + 0.5][k].v
                   + d.dvdx(n + 0.5)
                   * inerpolateStep * constP.h;
        }
        public double Interpolate_w(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 0.5, k + 1);
            int inerpolateStep = GetInterpolateStep(n + 0.5, k, PN.w);

            return g[n + 0.5][k].w
                   + d.dwdx(n + 0.5)
                   * inerpolateStep * constP.h;
        }



        public double InterpolateMixture(PN pn, LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 1, k + 0.5);

            int inerpolateStep = (int)(k - g[n + 1].LastIndex()).Value;

            return g[n + 1][k - 0.5][pn]
                   + (g[n + 1].sn[pn] - g[n + 1][k - 0.5][pn]) /
                     (g[n + 1].sn[PN.x] - x[k - 0.5])
                   * inerpolateStep * constP.h;
        }
        public double InterpolateDynamic(PN pn, LimitedDouble n, LimitedDouble k)
        {

        }
        public double Interpolate_m(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 0.5, k + 1);

            return g[n + 0.5][k + 1].v *
                   (g[n][k + 0.5].r + g[n][k + 1.5].r) 
                   / 2;
        }
        public double Interpolate_M(LimitedDouble n, LimitedDouble k)
        {
            (n, k) = OffseterNK.Appoint(n, k).Offset(n + 0.5, k + 1);

            return g[n + 0.5][k + 1].w * 
                   (g[n][k + 0.5].r + g[n][k + 1.5].r)
                   / 2;
        }        
        
        private int GetInterpolateStep(LimitedDouble n, LimitedDouble k, PN pn)
        {
            var kLast = g[n].LastIndex();

            while (g[n][kLast][pn] == g.NULL)
                kLast -= 1;

            var distanceToPointK = k - kLast;
            return (int)(distanceToPointK).Value;
        }
    }
}

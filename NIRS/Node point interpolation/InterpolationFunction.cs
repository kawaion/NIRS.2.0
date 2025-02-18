using MyDouble;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Grid_Folder;
using NIRS.Helpers;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Node_point_interpolation
{
    class InterpolationFunction : IInterpolationFunction
    {
        private readonly IConstParameters constP;
        GetterValueByPN gCell;

        public InterpolationFunction(IGrid grid, IConstParameters constParameters)
        {
            constP = constParameters;
            gCell = new GetterValueByPN(grid);
        }
        public double Interpolate(PN pn, LimitedDouble n, LimitedDouble k,int inerpolateStep)
        {
            return gCell.GetParamCell(pn, n, k - 1)
                   + (gCell.GetParamCellSn(pn, n) - gCell.GetParamCell(pn, n, k - 1)) /
                     (gCell.GetParamCellSn(PN.x, n) - (k - 1).Value * constP.h) 
                   * inerpolateStep * constP.h;
        }
    }
}

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

namespace NIRS.Functions_for_numerical_method
{
    public class ParameterInterpolationFunctions : IParameterInterpolationFunctions
    {
        private readonly IGrid g;
        private readonly IConstParameters constP;
        private GetterValueByPN gCell;

        private XGetter x;
        public ParameterInterpolationFunctions(IGrid grid, IMainData mainData)
        {
            x = new XGetter(mainData.ConstParameters);
            g = grid;
            constP = mainData.ConstParameters;
            gCell = new GetterValueByPN(grid);
        }
        public double Interpolate(PN pn, LimitedDouble n, LimitedDouble k)
        {
            int inerpolateStep = (int)(k - g[n].LastIndex()).Value;
            return gCell.GetParamCell(pn, n, k - 1)
                   + (gCell.GetParamCellSn(pn, n) - gCell.GetParamCell(pn, n, k - 1)) /
                     (gCell.GetParamCellSn(PN.x, n) - x[k - 1])
                   * inerpolateStep * constP.h;
        }
    }
}

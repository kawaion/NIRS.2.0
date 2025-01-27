using NIRS.Nabla_Functions.Close;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Nabla_Functions
{
    public static class WaypointCalculatorExtension
    {
        public static NablaNode Nabla(this IWaypointCalculator waypointCalculator, PN param1, PN param2, PN param3)
        {
            List<PN> list = new List<PN>();
            list.Add(param1);
            list.Add(param2);
            list.Add(param3);
            return new NablaNode(list, NablaFunctionType.Nabla3, waypointCalculator);
        }
        public static NablaNode Nabla(this IWaypointCalculator waypointCalculator, PN param1, PN param2)
        {
            List<PN> list = new List<PN>();
            list.Add(param1);
            list.Add(param2);
            return new NablaNode(list, NablaFunctionType.Nabla2, waypointCalculator);
        }
        public static NablaNode Nabla(this IWaypointCalculator waypointCalculator, PN param)
        {
            List<PN> list = new List<PN>();
            list.Add(param);
            return new NablaNode(list, NablaFunctionType.Nabla1, waypointCalculator);
        }
        public static NablaNode dPStrokeDivdx(this IWaypointCalculator waypointCalculator)
        {
            List<PN> uselessList = new List<PN>();//
            return new NablaNode(uselessList, NablaFunctionType.DDivDx, waypointCalculator);
        }
    }
}

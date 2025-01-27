using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDouble;
using NIRS.Nabla_Functions.Close;
using NIRS.Parameter_names;

namespace NIRS.Nabla_Functions
{
    public class NablaNode
    {
        public NablaNode(List<PN> pns, NablaFunctionType type, IWaypointCalculator nabla)
        {
            _pns = pns;
            _type = type;
            _nabla = nabla;
        }
        private List<PN> _pns;
        private NablaFunctionType _type;
        private readonly IWaypointCalculator _nabla;

        public double Cell(LimitedDouble n, LimitedDouble k)
        {
            switch (_type)
            {
                case NablaFunctionType.Nabla1: return _nabla.Nabla(_pns[0], n, k);
                case NablaFunctionType.Nabla2: return _nabla.Nabla(_pns[0], _pns[1], n, k);
                case NablaFunctionType.Nabla3: return _nabla.Nabla(_pns[0], _pns[1], _pns[2], n, k);
                case NablaFunctionType.DDivDx: return _nabla.dPStrokeDivdx(n, k);
                default: throw new Exception("неинициализированный тип");
            }
        }
    }
}

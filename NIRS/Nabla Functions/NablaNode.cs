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
    class NablaNode
    {
        public NablaNode(List<PN> pNs, NablaFunctionType type, INabla nabla)
        {
            _pNs = pNs;
            _type = type;
            _nabla = nabla;
        }
        private List<PN> _pNs;
        private NablaFunctionType _type;
        private readonly INabla _nabla;

        public double Cell(LimitedDouble n, LimitedDouble k)
        {
            switch (_type)
            {
                case NablaFunctionType.Nabla1: return _nabla.Nabla(_pNs[0], n, k);
                case NablaFunctionType.Nabla2: return _nabla.Nabla(_pNs[0], _pNs[1], n, k);
                case NablaFunctionType.Nabla3: return _nabla.Nabla(_pNs[0], _pNs[1], _pNs[2], n, k);
                case NablaFunctionType.DDivDx: return _nabla.dDivdx(_pNs[0], n, k);
                default: throw new Exception("неинициализированный тип");
            }
        }
    }
}

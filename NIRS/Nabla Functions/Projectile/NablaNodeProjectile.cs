using MyDouble;
using NIRS.Nabla_Functions.Close;
using NIRS.Nabla_Functions.Projectile;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Nabla_Functions.Projectile
{
    public class NablaNodeProjectile
    {
        public NablaNodeProjectile(List<PN> pns, NablaFunctionType type, IWaypointCalculatorProjectile nabla)
        {
            _pns = pns;
            _type = type;
            _nabla = nabla;
        }
        private List<PN> _pns;
        private NablaFunctionType _type;
        private readonly IWaypointCalculatorProjectile _nabla;

        public double Cell(LimitedDouble n)
        {
            switch (_type)
            {
                case NablaFunctionType.Nabla3: return _nabla.Nabla(_pns[0], _pns[1], _pns[2], n);
                default: throw new Exception("неинициализированный тип");
            }
        }
    }
}

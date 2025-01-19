using MyDouble;
using NIRS.Parameter_names;

namespace NIRS.Nabla_Functions
{
    interface INabla
    {
        double Nabla(PN param1, PN param2, PN param3, LimitedDouble n, LimitedDouble k);
        double Nabla(PN param1, PN param2, LimitedDouble n, LimitedDouble k);
        double Nabla(PN param, LimitedDouble n, LimitedDouble k);
        double dDivdx(PN param, LimitedDouble n, LimitedDouble k);
    }
}

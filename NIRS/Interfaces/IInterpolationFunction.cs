using MyDouble;
using NIRS.Parameter_names;

namespace NIRS.Interfaces
{
    public interface IInterpolationFunction
    {
        double Get(PN pn, LimitedDouble n, LimitedDouble k, int inerpolateStep);
    }
}

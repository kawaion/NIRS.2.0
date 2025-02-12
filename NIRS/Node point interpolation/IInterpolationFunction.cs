using MyDouble;
using NIRS.Parameter_names;

namespace NIRS.Node_point_interpolation
{
    public interface IInterpolationFunction
    {
        double Get(PN pn, LimitedDouble n, LimitedDouble k, int inerpolateStep);
    }
}

using MyDouble;
using NIRS.FunctionsNK;
using NIRS.Parameter_names;

namespace NIRS.Node_point_interpolation
{
    public interface IInterpolationFunction : IFunctionNK
    {
        double Get(PN pn, LimitedDouble n, LimitedDouble k, int inerpolateStep);
    }
}

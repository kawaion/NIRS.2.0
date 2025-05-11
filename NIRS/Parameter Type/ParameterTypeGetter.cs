using MyDouble;
using NIRS.Helpers;
using NIRS.Parameter_names;

namespace NIRS.Parameter_Type
{
    static class ParameterTypeGetter
    {
        public static bool IsDynamic(double n, double k)
        {
            return n.IsHalfInt() && k.IsInt();
        }
        public static bool IsMixture(double n, double k)
        {
            return n.IsInt() && k.IsHalfInt();
        }
        public static bool IsDynamic(this PN pn)
        {
            return pn.GetTypeFromParameterName() == PT.Dynamic;
        }
        public static bool IsMixture(this PN pn)
        {
            return pn.GetTypeFromParameterName() == PT.Mixture;
        }
    }
}

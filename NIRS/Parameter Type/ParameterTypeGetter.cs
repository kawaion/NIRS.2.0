using MyDouble;
using NIRS.Parameter_names;

namespace NIRS.Parameter_Type
{
    static class ParameterTypeGetter
    {
        public static bool IsDynamic(LimitedDouble n, LimitedDouble k)
        {
            return n.IsHalfInt() && k.IsInt();
        }
        public static bool IsMixture(LimitedDouble n, LimitedDouble k)
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

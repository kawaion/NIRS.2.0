using MyDouble;

namespace NIRS.Parameter_Type
{
    static class ParameterTypeGetter
    {
        public static bool isDynamic(LimitedDouble n, LimitedDouble k)
        {
            return n.Type == DoubleType.HalfInt && k.Type == DoubleType.Int;
        }
        public static bool isMixture(LimitedDouble n, LimitedDouble k)
        {
            return n.Type == DoubleType.Int && k.Type == DoubleType.HalfInt;
        }
    }
}

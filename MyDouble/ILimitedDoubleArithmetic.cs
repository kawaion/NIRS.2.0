

namespace MyDouble
{
    public interface ILimitedDoubleArithmetic
    {
        LimitedDouble Add(LimitedDouble myDouble1, LimitedDouble myDouble2);
        LimitedDouble Add(LimitedDouble myDouble, int otherValue);
        LimitedDouble Add(LimitedDouble myDouble, double otherValue);

        LimitedDouble Minus(LimitedDouble myDouble1, LimitedDouble myDouble2);
        LimitedDouble Minus(LimitedDouble myDouble, int otherValue);
        LimitedDouble Minus(LimitedDouble myDouble, double otherValue);
    }
}

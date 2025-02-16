

namespace MyDouble
{
    public class LimitedDouble
    {
        public double Value { get; }
        public DoubleType Type { get; }
        public LimitedDouble(double value)
        {
            Value = value;
            Type = DoubleTypeIssuer.Get(value);
        }



        protected internal LimitedDouble(double value, DoubleType type)
        {
            Value = value;
            Type = type;
        }




        private static ILimitedDoubleArithmetic limitedDoubleArithmetic = new LimitedDoubleArithmetic();

        public void ChangeLimitedDoubleArithmetic(ILimitedDoubleArithmetic newLimitedDoubleArithmetic)
        {
            limitedDoubleArithmetic = newLimitedDoubleArithmetic;
        }
        public static LimitedDouble operator +(LimitedDouble myDouble1, LimitedDouble myDouble2)
        {
            return limitedDoubleArithmetic.Add(myDouble1, myDouble2);
        }
        public static LimitedDouble operator +(LimitedDouble myDouble, int otherValue)
        {
            return limitedDoubleArithmetic.Add(myDouble, otherValue);
        }
        public static LimitedDouble operator +(LimitedDouble myDouble, double otherValue)
        {
            return limitedDoubleArithmetic.Add(myDouble, otherValue);
        }

        // a - b = a + (-b)
        public static LimitedDouble operator -(LimitedDouble myDouble1, LimitedDouble myDouble2)
        {
            return limitedDoubleArithmetic.Minus(myDouble1, myDouble2);
        }
        public static LimitedDouble operator -(LimitedDouble myDouble, int otherValue)
        {
            return limitedDoubleArithmetic.Minus(myDouble, otherValue);
        }
        public static LimitedDouble operator -(LimitedDouble myDouble, double otherValue)
        {
            return limitedDoubleArithmetic.Minus(myDouble, otherValue);
        }      

        public static bool operator !=(LimitedDouble myDouble1, LimitedDouble myDouble2)
        {
            return myDouble1.Value != myDouble2.Value;
        }
        public static bool operator ==(LimitedDouble myDouble1, LimitedDouble myDouble2)
        {
            return myDouble1.Value == myDouble2.Value;
        }
        public static bool operator <(LimitedDouble myDouble1, LimitedDouble myDouble2)
        {
            return myDouble1.Value < myDouble2.Value;
        }
        public static bool operator >(LimitedDouble myDouble1, LimitedDouble myDouble2)
        {
            return myDouble1.Value > myDouble2.Value;
        }
        public static bool operator <=(LimitedDouble myDouble1, LimitedDouble myDouble2)
        {
            return myDouble1 < myDouble2 || myDouble1 == myDouble2;
        }
        public static bool operator >=(LimitedDouble myDouble1, LimitedDouble myDouble2)
        {
            return myDouble1 > myDouble2 || myDouble1 == myDouble2;
        }



    }
    public enum DoubleType
    {
        Int,
        HalfInt
    }
}

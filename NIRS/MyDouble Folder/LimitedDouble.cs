using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.MyDouble_Folder
{
    class LimitedDouble
    {
        public double Value { get; }
        public DoubleType Type { get; }
        public LimitedDouble(double value)
        {
            Value = value;
            Type = DoubleTypeIssuer.Get(value);
        }
        private LimitedDouble(double value, DoubleType type)
        {
            Value = value;
            Type = type;
        }



        public static LimitedDouble operator +(LimitedDouble myDouble1, LimitedDouble myDouble2)
        {
            var newType = GetTypeFromArithmetic(myDouble1.Type, myDouble2.Type);
            var newValue = myDouble1.Value + myDouble2.Value;
            return new LimitedDouble(newValue, newType);
        }
        public static LimitedDouble operator +(LimitedDouble myDouble, int otherValue)
        {
            var newValue = myDouble.Value + otherValue;
            return new LimitedDouble(newValue, myDouble.Type);
        }
        public static LimitedDouble operator +(LimitedDouble myDouble, double otherValue)
        {
            var otherValueType = DoubleTypeIssuer.Get(otherValue);
            var newType = GetTypeFromArithmetic(myDouble.Type, otherValueType);
            var newValue = myDouble.Value + otherValue;
            return new LimitedDouble(newValue, newType);
        }

        // a - b = a + (-b)
        public static LimitedDouble operator -(LimitedDouble myDouble1, LimitedDouble myDouble2)
        {
            var myDouble2Negative = new LimitedDouble(-myDouble2.Value, myDouble2.Type);
            var additionResult = myDouble1 + myDouble2Negative;
            return additionResult;
        }
        public static LimitedDouble operator -(LimitedDouble myDouble, int otherValue)
        {
            var otherValueNegative = -otherValue;
            var additionResult = myDouble + otherValueNegative;
            return additionResult;
        }
        public static LimitedDouble operator -(LimitedDouble myDouble, double otherValue)
        {
            var otherValueNegative = -otherValue;
            var additionResult = myDouble + otherValueNegative;
            return additionResult;
        }



        private static DoubleType GetTypeFromArithmetic(DoubleType type1, DoubleType type2)
        {
            // при сложении с целым типом тип не изменяется
            if (type1 == DoubleType.Int) return type2;
            if (type2 == DoubleType.Int) return type1;
            // в этом случае оба типа полуцелые
            return DoubleType.Int;
        }
    }
    enum DoubleType
    {
        Int,
        HalfInt
    }
}

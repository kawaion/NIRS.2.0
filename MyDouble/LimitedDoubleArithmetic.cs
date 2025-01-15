

namespace MyDouble
{
    class LimitedDoubleArithmetic : ILimitedDoubleArithmetic
    {
        public LimitedDouble Add(LimitedDouble myDouble1, LimitedDouble myDouble2)
        {
            var newType = GetTypeFromArithmetic(myDouble1.Type, myDouble2.Type);
            var newValue = myDouble1.Value + myDouble2.Value;
            return new LimitedDouble(newValue, newType);
        }
        public LimitedDouble Add(LimitedDouble myDouble, int otherValue)
        {
            var newValue = myDouble.Value + otherValue;
            return new LimitedDouble(newValue, myDouble.Type);
        }
        public LimitedDouble Add(LimitedDouble myDouble, double otherValue)
        {
            var otherValueType = DoubleTypeIssuer.Get(otherValue);
            var newType = GetTypeFromArithmetic(myDouble.Type, otherValueType);
            var newValue = myDouble.Value + otherValue;
            return new LimitedDouble(newValue, newType);
        }

        // a - b = a + (-b)
        public LimitedDouble Minus(LimitedDouble myDouble1, LimitedDouble myDouble2)
        {
            var myDouble2Negative = new LimitedDouble(-myDouble2.Value, myDouble2.Type);
            var additionResult = myDouble1 + myDouble2Negative;
            return additionResult;
        }
        public LimitedDouble Minus(LimitedDouble myDouble, int otherValue)
        {
            var otherValueNegative = -otherValue;
            var additionResult = myDouble + otherValueNegative;
            return additionResult;
        }
        public LimitedDouble Minus(LimitedDouble myDouble, double otherValue)
        {
            var otherValueNegative = -otherValue;
            var additionResult = myDouble + otherValueNegative;
            return additionResult;
        }


        private static DoubleType GetTypeFromArithmetic(DoubleType type1, DoubleType type2)
        {
            // при сложении конкретного типа с целым типом конкретный тип не изменяется
            if (type1 == DoubleType.Int) return type2;
            if (type2 == DoubleType.Int) return type1;
            // в этом случае оба типа полуцелые, и при сложении всегда получается целый
            return DoubleType.Int;
        }
    }
}

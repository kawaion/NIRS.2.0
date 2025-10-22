
using System;
using System.Net.Http.Headers;

namespace MyDouble
{
    class LimitedDoubleArithmetic
    {
        FractionalArithmetic fractionalArithmetic = new FractionalArithmetic();
        ConverterOfCertainDouble converterOfCertainDouble = new ConverterOfCertainDouble();
        public LimitedDouble Add(LimitedDouble myDouble1, LimitedDouble myDouble2)
        {
            int integerValue = myDouble1.Integer + myDouble2.Integer;
            (int additiveInteger, bool isFractional) = fractionalArithmetic.Add(myDouble1.IsFractional, myDouble2.IsFractional);
            return new LimitedDouble(integerValue + additiveInteger, isFractional);
        }
        public LimitedDouble Add(LimitedDouble myDouble, int otherValue)
        {
            int integerValue = myDouble.Integer + otherValue;
            return new LimitedDouble(integerValue, myDouble.IsFractional);
        }
        public LimitedDouble Add(LimitedDouble myDouble, double otherValue)
        {
            LimitedDouble otherMyDouble = converterOfCertainDouble.Convert(otherValue);
            return Add(myDouble, otherMyDouble);
        }

        public LimitedDouble Subtract(LimitedDouble myDouble1, LimitedDouble myDouble2)
        {
            int integerValue = myDouble1.Integer - myDouble2.Integer;
            (int additiveInteger, bool isFractional) = fractionalArithmetic.Minus(myDouble1.IsFractional, myDouble2.IsFractional);
            return new LimitedDouble(integerValue + additiveInteger, isFractional);
        }
        public LimitedDouble Subtract(LimitedDouble myDouble, int otherValue)
        {
            int integerValue = myDouble.Integer - otherValue;
            return new LimitedDouble(integerValue, myDouble.IsFractional);
        }
        public LimitedDouble Subtract(LimitedDouble myDouble, double otherValue)
        {
            LimitedDouble otherMyDouble = converterOfCertainDouble.Convert(otherValue);
            return Subtract(myDouble, otherMyDouble);
        }

        public LimitedDouble Multiply(LimitedDouble myDouble, int multiplier)
        {
            if(multiplier == 2)
            {
                int newInteger = myDouble.Integer * 2;
                if (myDouble.IsFractional)
                    newInteger++;
                return new LimitedDouble(newInteger, false);
            }
            throw new Exception();
        }
        
    }
}

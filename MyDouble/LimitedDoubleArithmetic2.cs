using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDouble
{
    class LimitedDoubleArithmetic2
    {
        static ConverterToLimitedDoubleValue converter = new ConverterToLimitedDoubleValue();
        public LimitedDouble Add(LimitedDouble myDouble1, LimitedDouble myDouble2)
        {
            return new LimitedDouble(myDouble1._value + myDouble2._value, BooleanField.Add(myDouble1._half,myDouble2._half));
        }
        public LimitedDouble Add(LimitedDouble myDouble, int otherValue)
        {
            (int convertedOtherValue,_) = converter.LimitedDoubleConverter(otherValue);
            return new LimitedDouble(myDouble._value + convertedOtherValue, myDouble._half);
        }
        public LimitedDouble Add(LimitedDouble myDouble, double otherValue)
        {
            (int convertedOtherValue, bool halfOtherValue) = converter.LimitedDoubleConverter(otherValue);
            return new LimitedDouble(myDouble._value + convertedOtherValue, BooleanField.Add(myDouble._half,halfOtherValue));
        }

        public LimitedDouble Subtract(LimitedDouble myDouble1, LimitedDouble myDouble2)
        {
            return new LimitedDouble(myDouble1._value - myDouble2._value, BooleanField.Subtract(myDouble1._half, myDouble2._half));
        }
        public LimitedDouble Subtract(LimitedDouble myDouble, int otherValue)
        {
            (int convertedOtherValue, _) = converter.LimitedDoubleConverter(otherValue);
            return new LimitedDouble(myDouble._value - convertedOtherValue, myDouble._half);
        }
        public LimitedDouble Subtract(LimitedDouble myDouble, double otherValue)
        {
            (int convertedOtherValue, bool halfOtherValue) = converter.LimitedDoubleConverter(otherValue);
            return new LimitedDouble(myDouble._value - convertedOtherValue, BooleanField.Subtract(myDouble._half, halfOtherValue));
        }

        public LimitedDouble Multiply(LimitedDouble myDouble, int multiplier)
        {
            if(multiplier%2 == 0)
                return new LimitedDouble(myDouble._value * multiplier, myDouble._half);
            else
                return new LimitedDouble(myDouble._value * multiplier, !myDouble._half);
        }
    }
}

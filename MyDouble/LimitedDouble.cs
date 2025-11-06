

using System;
using System.ComponentModel;

namespace MyDouble
{
    public class LimitedDouble
    {
        internal int _value;
        internal bool _half;

        static ConverterToLimitedDoubleValue converter = new ConverterToLimitedDoubleValue();
        public LimitedDouble(double value)
        {
            (_value, _half) = converter.LimitedDoubleConverter(value);
        }
        public LimitedDouble(int value)
        {
            (_value,_half) = converter.LimitedDoubleConverter(value);
        }
        public LimitedDouble(int value, bool half)
        {
            _value = value;
            _half = half;
        }
        public bool IsHalfInt() => _half == true;
        public bool IsInt() => _half == false;

        public double GetDouble()
        {
            return _value / 2.0;
        }
        public int GetInt()
        {
            return _value / 2;
        }
        public int GetIndex()
        {
            return _value;
        }
        public LimitedDouble Copy()
        {
            return new LimitedDouble(_value, _half);
        }

        public override string ToString()
        {
            return GetDouble().ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is LimitedDouble @double &&
                   _value == @double._value;
        }

        public override int GetHashCode()
        {
            return -1939223833 + _value.GetHashCode();
        }

        private static LimitedDoubleArithmetic2 limitedDoubleArithmetic = new LimitedDoubleArithmetic2();

        #region методы арифметики
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

        public static LimitedDouble operator -(LimitedDouble myDouble1, LimitedDouble myDouble2)
        {
            return limitedDoubleArithmetic.Subtract(myDouble1, myDouble2);
        }
        public static LimitedDouble operator -(LimitedDouble myDouble, int otherValue)
        {
            return limitedDoubleArithmetic.Subtract(myDouble, otherValue);
        }
        public static LimitedDouble operator -(LimitedDouble myDouble, double otherValue)
        {
            return limitedDoubleArithmetic.Subtract(myDouble, otherValue);
        }

        public static LimitedDouble operator *(LimitedDouble myDouble, int multiplier)
        {
            return limitedDoubleArithmetic.Multiply(myDouble, multiplier);
        }


        public static LimitedDouble operator ++(LimitedDouble myDouble)
        {
            myDouble._value += 2;
            return myDouble;
        }
        #endregion

        #region методы сравнения
        public static bool operator ==(LimitedDouble myDouble1, LimitedDouble myDouble2)
        {
            return myDouble1._value == myDouble2._value;
        }
        public static bool operator !=(LimitedDouble myDouble1, LimitedDouble myDouble2)
        {
            return !(myDouble1 == myDouble2);
        }
        public static bool operator <(LimitedDouble myDouble1, LimitedDouble myDouble2)
        {
            return myDouble1._value < myDouble2._value;
        }
        public static bool operator >(LimitedDouble myDouble1, LimitedDouble myDouble2)
        {
            return myDouble1._value > myDouble2._value;
        }
        public static bool operator <=(LimitedDouble myDouble1, LimitedDouble myDouble2)
        {
            return myDouble1 < myDouble2 || myDouble1 == myDouble2;
        }
        public static bool operator >=(LimitedDouble myDouble1, LimitedDouble myDouble2)
        {
            return myDouble1 > myDouble2 || myDouble1 == myDouble2;
        }





        public static bool operator ==(LimitedDouble myDouble, double otherValue)
        {
            (int convertedOtherValue, _) = converter.LimitedDoubleConverter(otherValue);
            return myDouble._value == convertedOtherValue;
        }
        public static bool operator !=(LimitedDouble myDouble, double otherValue)
        {
            return !(myDouble == otherValue);
        }
        public static bool operator <(LimitedDouble myDouble, double otherValue)
        {
            (int convertedOtherValue, _) = converter.LimitedDoubleConverter(otherValue);
            return myDouble._value < convertedOtherValue;
        }
        public static bool operator >(LimitedDouble myDouble, double otherValue)
        {
            (int convertedOtherValue, _) = converter.LimitedDoubleConverter(otherValue);
            return myDouble._value > convertedOtherValue;
        }
        public static bool operator <=(LimitedDouble myDouble, double otherValue)
        {
            return myDouble < otherValue || myDouble == otherValue;
        }
        public static bool operator >=(LimitedDouble myDouble, double otherValue)
        {
            return myDouble > otherValue || myDouble == otherValue;
        }
        #endregion

        //public static bool operator !=(LimitedDouble myDouble1, double otherValue)
        //{
        //    return myDouble1.WholeValue != otherValue;
        //}
        //public static bool operator ==(LimitedDouble myDouble1, double otherValue)
        //{
        //    return myDouble1.WholeValue == otherValue;
        //}
        //public static bool operator <(LimitedDouble myDouble1, double otherValue)
        //{
        //    return myDouble1.WholeValue < otherValue;
        //}
        //public static bool operator >(LimitedDouble myDouble1, double otherValue)
        //{
        //    return myDouble1.WholeValue > otherValue;
        //}
        //public static bool operator <=(LimitedDouble myDouble1, double otherValue)
        //{
        //    return myDouble1 < otherValue || myDouble1 == otherValue;
        //}
        //public static bool operator >=(LimitedDouble myDouble1, double otherValue)
        //{
        //    return myDouble1 > otherValue || myDouble1 == otherValue;
        //}
    }
    public enum DoubleType
    {
        Int,
        HalfInt
    }
}

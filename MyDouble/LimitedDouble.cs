

using System;
using System.ComponentModel;

namespace MyDouble
{
    public struct LimitedDouble
    {
        public int Integer { get; }
        public bool IsFractional { get; }

        private const double half = 0.5; 

        public LimitedDouble(double value)
        {
            int integer = (int)value;
            IsFractional = ConverterOfCertainDouble.Is05(value - integer);
            Integer = HalfValueTransformer.Transform(integer, IsFractional);
        }
        public LimitedDouble(int integer, int fractional)
        {
            IsFractional = СonverterNumberToFlag.Convert(fractional);
            Integer = HalfValueTransformer.Transform(integer, IsFractional);
        }
        internal LimitedDouble(int integer, bool isFractional)
        {
            IsFractional = isFractional;
            Integer = integer;
        }
        public bool IsHalfInt() => IsFractional;
        public bool IsInt() => !IsFractional;

        public double Double()
        {
            if (IsFractional)
                return Integer + half;
            else
                return Integer;
        }
        public int Int()
        {
            return Integer;
        }

        public LimitedDouble Copy()
        {
            return new LimitedDouble(Integer, IsFractional);
        }

        //public static LimitedDouble Floor(double value)
        //{
        //    var doubleFloor = (int)value;
        //    var MyDouble = new LimitedDouble(doubleFloor);
        //    if (MyDouble + 0.5 <= value)
        //        return MyDouble + 0.5;
        //    else
        //        return MyDouble;
        //}


        private static LimitedDoubleArithmetic limitedDoubleArithmetic = new LimitedDoubleArithmetic();

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

        public static bool operator ==(LimitedDouble myDouble1, LimitedDouble myDouble2)
        {
            return (myDouble1.Integer == myDouble2.Integer) && (myDouble1.IsFractional == myDouble2.IsFractional);
        }
        public static bool operator !=(LimitedDouble myDouble1, LimitedDouble myDouble2)
        {
            return !(myDouble1 == myDouble2);
        }
        public static bool operator <(LimitedDouble myDouble1, LimitedDouble myDouble2)
        {
            if (myDouble1.Integer == myDouble2.Integer)
                return (myDouble1.IsFractional == false && myDouble2.IsFractional == true);

            return myDouble1.Integer < myDouble2.Integer;
        }
        public static bool operator >(LimitedDouble myDouble1, LimitedDouble myDouble2)
        {
            if (myDouble1.Integer == myDouble2.Integer)
                return (myDouble1.IsFractional == true && myDouble2.IsFractional == false);

            return myDouble1.Integer > myDouble2.Integer;
        }
        public static bool operator <=(LimitedDouble myDouble1, LimitedDouble myDouble2)
        {
            return myDouble1 < myDouble2 || myDouble1 == myDouble2;
        }
        public static bool operator >=(LimitedDouble myDouble1, LimitedDouble myDouble2)
        {
            return myDouble1 > myDouble2 || myDouble1 == myDouble2;
        }


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

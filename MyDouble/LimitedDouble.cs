

using System;
using System.ComponentModel;

namespace MyDouble
{
    public struct LimitedDouble
    {
        internal int Integer { get; private set; }
        internal bool IsFractional { get; private set; }

        private const double half = 0.5; 

        public LimitedDouble(double value)
        {
            Integer = (int)Math.Floor(value);
            IsFractional = Is05(value - Integer);
        }
        private const double eps = 1e-9;
        public static bool Is05(double value)
        {
            return 0.5 - eps < value && value < 0.5 + eps;
        }
        public LimitedDouble(int integer, int fractional)
        {
            IsFractional = СonverterNumberToFlag.Convert(fractional);
            Integer = HalfValueTransformer.TransformForLimitedDouble(integer, IsFractional);
        }
        internal LimitedDouble(int integer, bool isFractional)
        {
            IsFractional = isFractional;
            Integer = integer;
        }
        public bool IsHalfInt() => IsFractional;
        public bool IsInt() => !IsFractional;

        public double GetDouble()
        {
            if (IsFractional)
                return Integer + half;
            else
                return Integer;
        }
        public int GetInt()
        {
            return HalfValueTransformer.TransformForInt(Integer, IsFractional);
        }

        public LimitedDouble Copy()
        {
            return new LimitedDouble(Integer, IsFractional);
        }

        public override bool Equals(object obj)
        {
            return obj is LimitedDouble @double &&
                   Integer == @double.Integer &&
                   IsFractional == @double.IsFractional;
        }

        public override int GetHashCode()
        {
            int hashCode = -317837413;
            hashCode = hashCode * -1521134295 + Integer.GetHashCode();
            hashCode = hashCode * -1521134295 + IsFractional.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return GetDouble().ToString();
        }

        private static LimitedDoubleArithmetic limitedDoubleArithmetic = new LimitedDoubleArithmetic();

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
            myDouble.Integer += 1;
            return myDouble;
        }
        #endregion

        #region методы сравнения
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





        public static bool operator ==(LimitedDouble myDouble, double otherValue)
        {
            return myDouble.GetDouble() == otherValue;
        }
        public static bool operator !=(LimitedDouble myDouble, double otherValue)
        {
            return !(myDouble.GetDouble() == otherValue);
        }
        public static bool operator <(LimitedDouble myDouble, double otherValue)
        {
            return myDouble.GetDouble() < otherValue;
        }
        public static bool operator >(LimitedDouble myDouble, double otherValue)
        {
            return myDouble.GetDouble() > otherValue;
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

using System;

namespace MyDouble
{
    static class DoubleTypeIssuer
    {
        public static DoubleType Get(double value)
        {
            value = Math.Abs(value);

            if (value == 1 || value == 0 || IsInt(value)) return DoubleType.Int;
            else if ( value == 0.5 || value == 1.5 || IsHalfInt(value)) return DoubleType.HalfInt;
            else throw new Exception($"значение {value} не является ни целым ни полуцелым");
        }
        public static bool IsInt(double value) => IsEven(value/0.5);
        public static bool IsHalfInt(double value) => IsOdd(value/0.5);
        private static bool IsEven(double value) => Math.Abs(value) % 2 == 0;
        private static bool IsOdd(double value) => Math.Abs(value) % 2 == 1;
    }
}

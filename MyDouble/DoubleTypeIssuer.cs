using System;

namespace MyDouble
{
    static class DoubleTypeIssuer
    {
        public static DoubleType Get(double value)
        {
            if (IsInt(value)) return DoubleType.Int;
            else if (IsHalfInt(value)) return DoubleType.HalfInt;
            else throw new Exception($"значение {value} не является ни целым ни полуцелым");
        }
        public static bool IsInt(double value) => value - (int)value == 0;
        public static bool IsHalfInt(double value) => value - (int)value == 0.5;
    }
}

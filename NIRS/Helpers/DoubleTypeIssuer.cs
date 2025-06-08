using MyDouble;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Helpers
{
    static class DoubleTypeIssuer
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInt(this double value) 
        {
            return Math.Abs(value - Math.Round(value)) == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsHalfInt(this double value)
        {
            return Math.Abs(value - Math.Round(value)) == 0.5;
        }
        //public static bool IsHalfIntbyte(double value)
        //{
        //    const long exponentMask = 0x7FF0000000000000;
        //    const long mantissaMask = 0x000FFFFFFFFFFFFF;
        //    const int exponentBias = 1023;

        //    long bits = BitConverter.DoubleToInt64Bits(value);
        //    long exponent = ((bits & exponentMask) >> 52) - exponentBias;

        //    // Если число слишком большое (дробная часть не влияет) или NaN/Infinity → не полуцелое
        //    if (exponent >= 52 || exponent < -1)
        //        return false;

        //    // Если экспонента = -1 (число в [0.5; 1)), проверяем мантиссу
        //    if (exponent == -1)
        //        return (bits & mantissaMask) == 0; // Мантисса должна быть нулевой (ровно 0.5)

        //    // Для других случаев (1.5, 2.5 и т. д.) проверяем последний бит мантиссы
        //    long mantissa = bits & mantissaMask;
        //    long lastBitMask = 1L << (51 - (int)exponent);
        //    return (mantissa & lastBitMask) != 0;
        //}
        //private struct DoubleComparer : IEqualityComparer<double>
        //{
        //    public bool Equals(double x, double y) => x == y;
        //    public int GetHashCode(double obj) => BitConverter.DoubleToInt64Bits(obj).GetHashCode();
        //}

        //private static readonly HashSet<double> _halfIntCache = new HashSet<double>(capacity: 1024, comparer: DoubleComparer.Default);

        //public static bool IsHalfInt(this double x)
        //{
        //    if (_halfIntCache.Contains(x)) return true;
        //    bool result = IsHalfIntdop(x);
        //    if (result) _halfIntCache.Add(x);
        //    return result;
        //}
        //public static bool IsHalfInt(this double value) => IsOdd(value / 0.5);
        //private static bool IsEven(double value) => Math.Abs(value) % 2 == 0;
        //private static bool IsOdd(double value) => Math.Abs(value) % 2 == 1;
    }
}

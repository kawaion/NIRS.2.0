﻿using MyDouble;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Helpers
{
    static class DoubleTypeIssuer
    {
        public static bool IsInt(this double value) => IsEven(value / 0.5);
        public static bool IsHalfInt(this double value) => IsOdd(value / 0.5);
        private static bool IsEven(double value) => Math.Abs(value) % 2 == 0;
        private static bool IsOdd(double value) => Math.Abs(value) % 2 == 1;
    }
}

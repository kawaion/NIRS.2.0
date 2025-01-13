using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.MyDouble_Folder
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

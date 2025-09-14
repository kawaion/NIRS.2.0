using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDouble
{
    class FractionalArithmetic
    {
        public (int, bool) Add(bool isFractional1, bool isFractional2)
        {
            if (isFractional1 != isFractional2)   // 0.5 + 0 = 0.5    или    0 + 0.5 = 0.5
                return (0, true);
            else if (isFractional1 == true && isFractional2 == true)   // 0.5 + 0.5 = 1.0
                return (1, false);
            else  // 0 + 0 = 0.0
                return (0, false);
        }
        public (int, bool) Minus(bool isFractional1, bool isFractional2)
        {
            if (isFractional1 == isFractional2)  // 0.5 - 0.5 = 0.0   или   0.0 - 0.0 = 0.0
                return (0, false);
            else if (isFractional1 == true && isFractional2 == false)  // 0.5 - 0.0 = 0.5
                return (0, true);
            else    // 0.0 - 0.5 = -0.5
                return (-1, true);
        }
    }
}

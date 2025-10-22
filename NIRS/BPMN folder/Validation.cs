using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.BPMN_folder
{
    static class Validation
    {
        public static double Validation01(double value) // метод скопирован
        {
            if (value > 1)
                value = 1;
            return value;
        }
    }
}

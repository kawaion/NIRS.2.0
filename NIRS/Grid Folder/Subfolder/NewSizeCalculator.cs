using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Grid_Folder.Subfolder
{
    class NewSizeCalculator
    {
        private const double GrowthFactor = 2; // Множитель роста
        public static int Calculate(int currentSize, int requiredIndex)
        {
            double newSize = currentSize;
            while (newSize <= requiredIndex)
                newSize *= GrowthFactor;
            return (int)newSize;
        }
    }
}

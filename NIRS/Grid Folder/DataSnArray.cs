using MyDouble;
using NIRS.Grid_Folder.Subfolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Grid_Folder
{
    class DataSnArray
    {
    
        private const int InitialCapacity = 256;

        private double[,] data;

        public int SizeP => data.GetLength(0);
        public int SizeN => data.GetLength(1);

        public DataSnArray(int countParams)
        {
            data = new double[countParams, InitialCapacity];
        }

        public double this[int p, int n]
        {
            get
            {
                return data[p, n];
            }
            set
            {
                TryResizeArray(n);
                data[p, n] = value;
            }
        }

        private void TryResizeArray(int n)
        {
            int newSizeN = n;
            if (n >= SizeN)
            {
                newSizeN = NewSizeCalculator.Calculate(SizeN, n);
                data = data.Resize2DArray(SizeP, newSizeN);
            }
        }
    }
}

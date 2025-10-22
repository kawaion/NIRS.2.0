using NIRS.Grid_Folder.Subfolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Grid_Folder
{
    class DataArray
    {
        private const int InitialCapacity = 128;

        private double[,,] data;

        public int SizeP => data.GetLength(0);
        public int SizeN => data.GetLength(1);
        public int SizeK => data.GetLength(2);

        public DataArray(int countParam)
        {
            data = new double[countParam, InitialCapacity, InitialCapacity];
        }

        public double this[int p, int n, int k]
        {
            get
            {
                return data[p, n, k];
            }
            set
            {
                TryResizeArray(n, k);
                data[p, n, k] = value;
            }
        }

        private void TryResizeArray(int n, int k)
        {
            bool isResize = false;
            int newSizeN = SizeN, newSizeK = SizeK;
            if (n >= SizeN)
            {
                isResize = true;
                newSizeN = NewSizeCalculator.Calculate(SizeN, n);
            }
            if (k >= SizeK)
            {
                isResize = true;
                newSizeK = NewSizeCalculator.Calculate(SizeK, k);
            }

            if (isResize)
                data = data.Resize3DArray(SizeP, newSizeN, newSizeK);
        }
    }
}

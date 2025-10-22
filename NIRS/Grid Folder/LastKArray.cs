using MyDouble;
using NIRS.Grid_Folder.Subfolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Grid_Folder
{
    class LastKArray
    {
        private const int InitialCapacity = 128;

        private LimitedDouble[,] data;

        public int SizeP => data.GetLength(0);
        public int SizeN => data.GetLength(1);

        public LastKArray (int constParam)
        {
            data = new LimitedDouble[constParam, InitialCapacity];
        }

        public LimitedDouble this[int p, int n]
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

        public bool IsNewLayer(int n)
        {
            return n >= SizeN;
        }

        private void TryResizeArray(int n)
        {
            if (n >= SizeN)
            {
                int newSizeN = NewSizeCalculator.Calculate(SizeN, n);
                data = data.Resize2DArray(SizeP, newSizeN);
            }
        }
    }
}

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
        LimitedDouble _defaultValue;

        public int SizeP => data.GetLength(0);
        public int SizeN => data.GetLength(1);

        public LastKArray (int constParam, LimitedDouble defaultValue)
        {
            data = new LimitedDouble[constParam, InitialCapacity];
            for (int i = 0; i < constParam; i++)
                for (int j = 0; j < InitialCapacity; j++)
                    data[i, j] = defaultValue.Copy();

            _defaultValue = defaultValue;
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
                int lastN = n;
                int newSizeN = NewSizeCalculator.Calculate(SizeN, n);
                data = data.Resize2DArray(SizeP, newSizeN);
                for (int i = 0; i < data.GetLength(0); i++)
                    for (int j = lastN; j < data.GetLength(1); j++)
                        data[i, j] = _defaultValue.Copy();
            }
        }
    }
}

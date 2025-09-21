using MyDouble;
using NIRS.Grid_Folder.Subfolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Grid_Folder
{
    class LastNArray
    {

        private LimitedDouble[] data;

        public int SizeP => data.GetLength(0);

        public LastNArray(int constParam)
        {
            data = new LimitedDouble[constParam];
        }

        public LimitedDouble this[int p]
        {
            get
            {
                return data[p];
            }
            set
            {
                data[p] = value;
            }
        }
    }
}

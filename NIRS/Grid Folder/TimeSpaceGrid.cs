using System;
using NIRS.MyDouble_Folder;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Grid_Folder
{
    class TimeSpaceGrid : Grid
    {
        public TimeSpaceGrid()
        {

        }
        private List<SubGrid> subGrid;
        public override SubGrid this[LimitedDouble n]
        {
            get
            {
                int index = ConvertNToIndex(n);
                return subGrid[index];
            }
            set
            {
                int index = ConvertNToIndex(n);
                subGrid[index] = value;
            }
        }
        private int ConvertNToIndex(LimitedDouble n) => (int)(n.Value * 2);
    }
}

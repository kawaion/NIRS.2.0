using MyDouble;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Grid_Folder
{
    class SpaceGrid : SubGrid
    {
        double n;

        List<GridCell> gridCells = new List<GridCell>();

        public override GridCell this[LimitedDouble k]
        {
            get
            {
                int index = ConvertNToIndex(k);
                if (subGrid[index] != null)
                    return subGrid[index];
                else throw new NullReferenceException();
            }
            set
            {
                int index = ConvertNToIndex(k);
                subGrid = AllocateMemorySubGridForTheIndex(subGrid, index);
                value.
                subGrid[index] = value;
            }
        }
    }
}

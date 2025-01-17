using MyDouble;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Grid_Folder
{
    class SpaceGrid : ISubGrid
    {
        double n;

        List<IGridCell> gridCells = new List<IGridCell>();

        public IGridCell this[LimitedDouble k]
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

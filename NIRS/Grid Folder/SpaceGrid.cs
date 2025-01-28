using MyDouble;
using System;
using System.Collections.Generic;
using NIRS.Memory_allocator;
using NIRS.Parameter_Type;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Grid_Folder
{
    class SpaceGrid : ISubGrid
    {
        private LimitedDouble n = null;
        public LimitedDouble N
        {
            get
            {
                return n;
            }
            set
            {
                if (n == null)
                    n = value;
                else throw new Exception("значение уже задано");
            }
        }

        List<IGridCell> gridCells = new List<IGridCell>();

        public IGridCell this[LimitedDouble k]
        {
            get
            {
                int index = ConvertNToIndex(k);
                if (gridCells[index] != null)
                    return gridCells[index];
                else throw new NullReferenceException();
            }
            set
            {
                int index = ConvertNToIndex(k);
                gridCells = AllocateMemorygridCellsForTheIndex(gridCells, index);
//value.
                gridCells[index] = value;
            }
        }

        private List<IGridCell> AllocateMemorygridCellsForTheIndex(List<IGridCell> gridCells, int index)
        {
            return gridCells.AllocateUpTo(index);
        }
        private int ConvertNToIndex(LimitedDouble k)
        {
            if (n.IsHalfInt() && k.IsInt())
                return (int)k.Value;
            if (n.IsInt() && k.IsHalfInt())
                return (int)(k.Value - 0.5);

            throw new Exception($"значение {n} {k} не подходит ни под один из типов");
        }
    }
}

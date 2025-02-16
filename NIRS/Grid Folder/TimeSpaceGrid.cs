
using MyDouble;
using System.Collections.Generic;
using NIRS.Memory_allocator;
using System;
using NIRS.Interfaces;

namespace NIRS.Grid_Folder
{
    class TimeSpaceGrid : IGrid
    {
        public TimeSpaceGrid()
        {
        }


        private List<ISubGrid> subGridPlus = new List<ISubGrid>();
        private List<ISubGrid> subGridMinus = new List<ISubGrid>();

        public ISubGrid this[LimitedDouble n]
        {
            get
            {
                (var index, var subGrid) = ChooseIndexAndSubGrid(n);

                if (subGrid[index] != null)
                    return subGrid[index];
                else throw new NullReferenceException();
            }
            set
            {
                (var index,var subGrid)= ChooseIndexAndSubGrid(n);

                subGrid = AllocateMemorySubGridForTheIndex(subGrid, index);
                subGrid[index] = value;
                subGrid[index].N = n;
            }
        }

        private (int index, List<ISubGrid> subGrid) ChooseIndexAndSubGrid(LimitedDouble n)
        {
            int index;
            List<ISubGrid> subGrid;

            if (n.Value < 0)
            {
                index = ConvertNToIndexMinus(n);
                subGrid = subGridMinus;
            }
            else
            {
                index = ConvertNToIndex(n);
                subGrid = subGridPlus;
            }

            return (index, subGrid);
        }

        private int ConvertNToIndex(LimitedDouble n) => (int)(n.Value * 2);
        private int ConvertNToIndexMinus(LimitedDouble n) => (int)(-n.Value * 2);
        private LimitedDouble ConvertIntToLimitedDouble(int n) => new LimitedDouble(n / 2.0);
        private List<ISubGrid> AllocateMemorySubGridForTheIndex(List<ISubGrid> subGrid, int index)
        {
            return subGrid.AllocateUpTo(index);
        }
    }
}

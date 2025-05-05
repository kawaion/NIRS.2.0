
using MyDouble;
using System.Collections.Generic;
using NIRS.Memory_allocator;
using System;
using NIRS.Interfaces;
using NIRS.RAM_folder;

namespace NIRS.Grid_Folder
{
    public class TimeSpaceGrid : IGrid
    {
        RAM<double, ISubGrid> ram;
        public TimeSpaceGrid()
        {
            ram = new RAM<double, ISubGrid>(4);
        }


        private List<ISubGrid> subGridPlus = new List<ISubGrid>();
        private List<ISubGrid> subGridMinus = new List<ISubGrid>();

        public ISubGrid this[LimitedDouble n]
        {
            get
            {
                if (ram.isContains(n.Value))
                    return ram.Get(n.Value);

                (var index, var subGrid) = ChooseIndexAndSubGrid(n);

                subGrid = AllocateMemorySubGridForTheIndex(subGrid, index, n);
                ram.Add(n.Value, subGrid[index]);
                return subGrid[index];
            }
            set
            {
                (var index,var subGrid)= ChooseIndexAndSubGrid(n);

                subGrid = AllocateMemorySubGridForTheIndex(subGrid, index, n);
                subGrid[index] = value;
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
        private int ConvertNToIndexMinus(LimitedDouble n) => (int)(-n.Value * 2)-1;
        private LimitedDouble ConvertIntToLimitedDouble(int n) => new LimitedDouble(n / 2.0);
        private List<ISubGrid> AllocateMemorySubGridForTheIndex(List<ISubGrid> subGrid, int index, LimitedDouble n)
        {
            subGrid.AllocateUpTo(index, GetNew);
            subGrid[index].n = n;
            return subGrid;
        }
        private SpaceGrid GetNew()
        {
            return new SpaceGrid();
        }

        public LimitedDouble MinN
        {
            get
            {
                if (subGridMinus.Count > 0) return subGridMinus[subGridMinus.Count - 1].n;
                else return subGridPlus[0].n;
            }
        }
        public LimitedDouble MaxN
        {
            get
            {
                if (subGridPlus.Count > 0) return subGridPlus[subGridPlus.Count - 1].n;
                else 
                {
                    if (subGridMinus[0].GetIsNull() == false) return subGridMinus[0].n; 
                    else return subGridMinus[1].n;
                }   
            }
        }
    }
}

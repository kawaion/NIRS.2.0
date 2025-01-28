
using MyDouble;
using System.Collections.Generic;
using NIRS.Memory_allocator;
using System;

namespace NIRS.Grid_Folder
{
    class TimeSpaceGrid : IGrid
    {
        private double _tau;
        private double _h;
        public TimeSpaceGrid(double tau, double h)
        {
            _tau = tau;
            _h = h;
        }


        private List<ISubGrid> subGrid;


        public ISubGrid this[LimitedDouble n]
        {
            get
            {
                int index = ConvertNToIndex(n);
                if (subGrid[index] != null)
                    return subGrid[index];
                else throw new NullReferenceException();
            }
            set
            {
                int index = ConvertNToIndex(n);
                subGrid = AllocateMemorySubGridForTheIndex(subGrid, index);
                subGrid[index] = value;
                subGrid[index].N = n;
            }
        }
        private int ConvertNToIndex(LimitedDouble n) => (int)(n.Value * 2);
        private List<ISubGrid> AllocateMemorySubGridForTheIndex(List<ISubGrid> subGrid, int index)
        {
            return subGrid.AllocateUpTo(index);
        }
    }
}

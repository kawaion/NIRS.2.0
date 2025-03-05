using MyDouble;
using System;
using System.Collections.Generic;
using NIRS.Memory_allocator;
using NIRS.Parameter_Type;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NIRS.Interfaces;
using NIRS.Parameter_names;

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

        List<IGridCell> gridCellsMinus = new List<IGridCell>();
        List<IGridCell> gridCellsPlus = new List<IGridCell>();

        public IGridCell this[LimitedDouble k]
        {
            get
            {
                (var index, var gridCells) = ChooseIndexAndgridCells(k);

                if (gridCells[index] != null)
                    return gridCells[index];
                else throw new NullReferenceException();
            }
            set
            {
                (var index, var gridCells) = ChooseIndexAndgridCells(k);

                gridCells = AllocateMemorygridCellsForTheIndex(gridCells, index);
                //value.
                gridCells[index] = value;
            }
        }

        public LimitedDouble LastIndex()
        {
            int lastIndex = gridCellsPlus.Count-1;  // провертить в тесте выдается ли последний индекс
            var value = ConvertIndexToN(lastIndex);
            return value;
        }
        public LimitedDouble LastIndex(PN pn)
        {
            var kLast = LastIndex();
            IGrid g = new TimeSpaceGrid();

            while (this[kLast][pn] == g.NULL)
                kLast -= 1;

            return kLast;
        }


        public IGridCell Last()
        {
            int lastI = ConvertNToIndex(LastIndex());
            return gridCellsPlus[lastI];
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
        private int ConvertNToIndexMinus(LimitedDouble k)
        {
            if (n.IsHalfInt() && k.IsInt())
                return (int)(-k.Value);
            if (n.IsInt() && k.IsHalfInt())
                return (int)(-k.Value - 0.5);

            throw new Exception($"значение {n} {k} не подходит ни под один из типов");
        }
        private LimitedDouble ConvertIndexToN(int value)
        {
            if (n.IsHalfInt())
                return new LimitedDouble(value);
            if (n.IsInt())
                return new LimitedDouble(value) + 0.5;

            throw new Exception();
        }
        private (int index, List<IGridCell> gridCells) ChooseIndexAndgridCells(LimitedDouble k)
        {
            int index;
            List<IGridCell> gridCells;

            if (k.Value < 0)
            {
                index = ConvertNToIndexMinus(n);
                gridCells = gridCellsMinus;
            }
            else
            {
                index = ConvertNToIndex(n);
                gridCells = gridCellsPlus;
            }

            return (index, gridCells);
        }


        public IGridCellProjectile sn { get; set; }
    }
}

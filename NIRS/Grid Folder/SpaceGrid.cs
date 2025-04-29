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
        private LimitedDouble N;
        public LimitedDouble n
        {
            get
            {
                return N;
            }
            set
            {
                if (isNull)
                {
                    N = value;
                    isNull = false;
                }
                else if (N == value)
                    return;
                else
                    throw new Exception("нельзя задать новое значение n");
            }
        }
        private bool isNull = true;

        public SpaceGrid()
        {
        }

        List<SpaceCellWithK> gridCellsMinus = new List<SpaceCellWithK>();
        List<SpaceCellWithK> gridCellsPlus = new List<SpaceCellWithK>();

        public SpaceCellWithK this[LimitedDouble k]
        {
            get
            {
                (var index, var gridCells) = ChooseIndexAndgridCells(k);

                gridCells = AllocateMemorygridCellsForTheIndex(gridCells, index, k);
                return gridCells[index];
            }
            set
            {
                (var index, var gridCells) = ChooseIndexAndgridCells(k);

                gridCells = AllocateMemorygridCellsForTheIndex(gridCells, index, k);
                gridCells[index] = value;
            }
        }

        private LimitedDouble LastIndex()
        {
            int lastIndex = gridCellsPlus.Count-1;  // провертить в тесте выдается ли последний индекс
            var value = ConvertIndexToN(lastIndex);
            return value;
        }
        public LimitedDouble LastIndex(PN pn)
        {
            var kLast = LastIndex();
            IGrid g = new TimeSpaceGrid();

            while (this[kLast].isNull(pn))
                kLast -= 1;

            return kLast;
        }
        public double Last(PN pn)
        {
            int lastI = ConvertKToIndex(LastIndex(pn));
            return gridCellsPlus[lastI][pn];
        }


        //private List<IGridCell> AllocateMemorygridCellsForTheIndex(List<IGridCell> gridCells, int index)
        //{
        //    return gridCells.AllocateUpTo(index,new SpaceCell());
        //}
        private List<SpaceCellWithK> AllocateMemorygridCellsForTheIndex(List<SpaceCellWithK> gridCells, int index, LimitedDouble k)
        {
            gridCells.AllocateUpTo(index, GetNew);
            gridCells[index].k = k;
            return gridCells;
        }
        private SpaceCellWithK GetNew()
        {
            return new SpaceCellWithK();
        }
        private int ConvertKToIndex(LimitedDouble k)
        {
            if (n.IsHalfInt() && k.IsInt())
                return (int)k.Value;
            if (n.IsInt() && k.IsHalfInt())
                return (int)(k.Value - 0.5);

            throw new Exception($"значение {n.Value} {k.Value} не подходит ни под один из типов");
        }
        private int ConvertKToIndexMinus(LimitedDouble k)
        {
            if (n.IsHalfInt() && k.IsInt())
                return (int)(-k.Value);
            if (n.IsInt() && k.IsHalfInt())
                return (int)(-k.Value - 0.5);

            throw new Exception($"значение {n.Value} {k.Value} не подходит ни под один из типов");
        }
        private LimitedDouble ConvertIndexToN(int value)
        {
            if (n.IsHalfInt())
                return new LimitedDouble(value);
            if (n.IsInt())
                return new LimitedDouble(value) + 0.5;

            throw new Exception();
        }
        private (int index, List<SpaceCellWithK> gridCells) ChooseIndexAndgridCells(LimitedDouble k)
        {
            int index;
            List<SpaceCellWithK> gridCells;

            if (k.Value < 0)
            {
                index = ConvertKToIndexMinus(k);
                gridCells = gridCellsMinus;
            }
            else
            {
                index = ConvertKToIndex(k);
                gridCells = gridCellsPlus;
            }

            return (index, gridCells);
        }


        public SpaceCellProjectile sn { get; set; } = new SpaceCellProjectile();
    }
}

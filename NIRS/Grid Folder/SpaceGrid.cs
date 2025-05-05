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
using NIRS.RAM_folder;

namespace NIRS.Grid_Folder
{
    class SpaceGrid : ISubGrid
    {
        private double N;
        public LimitedDouble n
        {
            get
            {
                return new LimitedDouble(N);
            }
            set
            {
                if (isNull)
                {
                    N = value.Value;
                    isNull = false;
                }
                else if (N == value.Value)
                    return;
                else
                    throw new Exception("нельзя задать новое значение n");
            }
        }
        private bool isNull = true;
        public bool GetIsNull() => isNull;

        RAM<double, SpaceCellWithK> ram;
        public SpaceGrid()
        {
            ram = new RAM<double, SpaceCellWithK>(4);
        }

        List<SpaceCellWithK> gridCellsMinus = new List<SpaceCellWithK>();
        List<SpaceCellWithK> gridCellsPlus = new List<SpaceCellWithK>();

        public SpaceCellWithK this[LimitedDouble k]
        {
            get
            {
                if (ram.isContains(k.Value))
                    return ram.Get(k.Value);

                (var index, var gridCells) = ChooseIndexAndgridCells(k);

                gridCells = AllocateMemorygridCellsForTheIndex(gridCells, index, k);
                ram.Add(k.Value, gridCells[index]);
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


        public LimitedDouble MinK
        {
            get
            {
                if (gridCellsMinus.Count > 0) return gridCellsMinus[gridCellsMinus.Count - 1].k;
                else return gridCellsPlus[0].k;
            }
        }
        public LimitedDouble MaxK(PN pn)
        {
            if (gridCellsPlus.Count > 0) return gridCellsPlus[gridCellsPlus.Count - 1].k;
            else
            {
                if (gridCellsMinus[0].isNull(pn) == false) return gridCellsMinus[0].k;
                else return gridCellsMinus[1].k;
            }
        }
    }
}

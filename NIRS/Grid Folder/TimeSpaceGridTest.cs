
using MyDouble;
using System.Collections.Generic;
using NIRS.Memory_allocator;
using System;
using NIRS.Interfaces;
using NIRS.RAM_folder;
using NIRS.Parameter_names;
using System.Linq;
using System.Reflection;
using NIRS.Helpers;
using System.Diagnostics;
using System.Text;

namespace NIRS.Grid_Folder
{
    [DebuggerDisplay("TimeSpaceGridTest: Params={countParams}, MaxNegN={maximumnNegativeN}, MaxNegK={maximumnNegativeK}")]
    public class TimeSpaceGridTest : IGrid
    {
        RAM<(PN, double), List<(double k, double value)>> ram;
        public TimeSpaceGridTest()
        {
            ram = new RAM<(PN, double), List<(double k, double value)>>(100);
            ramSn = new RAM<(PN, double), double>(15);
            InicialiseData();
            InicialiseDataSn();
        }
        private void InicialiseData()
        {
            for(int i = 0; i < countParams; i++)
            {
                data[i] = new List<(double n, List<(double k, double value)> layer)>();
            }
        }
        private void InicialiseDataSn()
        {
            for (int i = 0; i < countParamsSn; i++)
            {
                dataSn[i] = new List<(double n, double value)>();
            }
        }
        const int countParams = 13;
        List<(double n, List<(double k,double value)>layer )>[] data = new List<(double n, List<(double k, double value)> layer)>[countParams]; // количество переменных

        const int maximumnNegativeN = 1;
        const int maximumnNegativeK = 1;

        private const int number_dynamic_m = (int)PN.dynamic_m;
        private const int number_M = (int)PN.M;
        private const int number_v = (int)PN.v;
        private const int number_w = (int)PN.w;
        private const int number_r = (int)PN.r;
        private const int number_e = (int)PN.e;
        private const int number_eps = (int)PN.eps;
        private const int number_psi = (int)PN.psi;
        private const int number_z = (int)PN.z;
        private const int number_a = (int)PN.a;
        private const int number_p = (int)PN.p;
        private const int number_ro = (int)PN.ro;
        private const int number_m = (int)PN.m;

        public double this[PN pn, double n, double k]
        {
            get
            {
                var kIndex = ConvertToKIndex(k);

                //if (ram.isContains((pn, n)))
                //    return ram.Get((pn, n))[kIndex].value;//память

                var nIndex = ConvertToNIndex(n);
                if(pn == PN.One_minus_m)
                    return 1.02 - data[(int)PN.m][nIndex].layer [kIndex].value;
                else
                {
                    var cells = data[(int)pn][nIndex].layer;

                    //ram.Add((pn, n), cells);//память

                    return cells[kIndex].value;
                } 
            }
            set
            {
                var nIndex = ConvertToNIndex(n);
                var kIndex = ConvertToKIndex(k);
                var layers = data[(int)pn];
                layers = AllocateMemoryNLayersForTheIndex(layers, nIndex, n);
                var cells = layers[nIndex].layer;
                cells = AllocateMemoryCellsForTheIndex(cells, kIndex, k);
                var cellK = cells[kIndex];
                value = Validation(value);
                cellK.value = value;

                cells[kIndex] = cellK;
            }
        }
        private double Validation(double value)
        {
            if (Math.Abs(value) < 1e-6)
                return 0;
            return value;
        }
        private int ConvertToNIndex(double n)
        {
            if (n.IsInt())
                return (int)(n + maximumnNegativeN);
            if (n.IsHalfInt())
                return (int)(n - 0.5 + maximumnNegativeN);
            throw new Exception();
        }
        private int ConvertToKIndex(double k)
        {
            if(k.IsInt())
                return (int)(k + maximumnNegativeK);
            if(k.IsHalfInt())
                return (int)(k -0.5 + maximumnNegativeK);
            throw new Exception();
        }
        private List<(double n, List<(double k, double value)> layer)> AllocateMemoryNLayersForTheIndex(List<(double n, List<(double k, double value)> layer)> layers, int index, double n)
        {
            layers.AllocateUpTo(index, GetNewN);
            var layerN = layers[index];
            layerN.n = n;
            layers[index] = layerN;
            return layers;
        }

        double newN = -maximumnNegativeN - 1;
        private (double n, List<(double k, double layer)> layer) GetNewN()
        {
            var newlayer = new List<(double k, double layer)>();
            (double n, List<(double k, double layer)> layer) res = (n: newN, layer: newlayer);
            return res;
        }

        private List<(double k, double value)> AllocateMemoryCellsForTheIndex(List<(double k, double value)> cells, int index, double k)
        {
            cells.AllocateUpTo(index, GetNewK);
            var cellK = cells[index];
            cellK.k = k;
            cells[index] = cellK;
            return cells;
        }

        const double newK = -maximumnNegativeK - 1;
        const double newCellValue = 0;
        private (double k, double value) GetNewK()
        {
            (double k, double value) res = (k: newK, value: newCellValue);
            return res;
        }

        public double LastIndexK(PN pn,double n)
        {
            var nIndex = ConvertToNIndex(n);

            return data[(int)pn][nIndex].layer.Last().k;
        }
        public double LastIndexN(PN pn)
        {
            return data[(int)pn].Last().n;
        }







        const int countParamsSn = 15;
        List<(double n, double value)>[] dataSn = new List<(double n, double value)>[countParamsSn]; // количество переменных

        RAM<(PN, double), double> ramSn;

        private const int number_x = (int)PN.x;
        private const int number_vSn = (int)PN.vSn;

        public double GetSn(PN pn, double n)
        {
            if (ramSn.isContains((pn, n)))
                return ramSn.Get((pn, n));//память

            var nIndex = ConvertToNIndexSn(n);
            if (pn == PN.One_minus_m)
                return 1.02 - dataSn[(int)PN.m][nIndex].value;
            else
            {
                var value = dataSn[(int)pn][nIndex].value;

                ramSn.Add((pn, n), value);//память

                return value;
            }
        }
        public void SetSn(PN pn, double n, double value)
        {
            var nIndex = ConvertToNIndexSn(n);
            var layersSn = dataSn[(int)pn];
            layersSn = AllocateMemorySnForTheIndex(layersSn, nIndex, n);
            var cellSn = layersSn[nIndex];
            cellSn.value = value;

            layersSn[nIndex] = cellSn;
        }
        private List<(double n, double value)> AllocateMemorySnForTheIndex(List<(double n, double value)> layers, int index, double n)
        {
            layers.AllocateUpTo(index, GetNewNSn);
            var layerNSn = layers[index];
            layerNSn.n = n;

            layers[index] = layerNSn;
            return layers;
        }
        double newValue = 0;
        private (double n, double value) GetNewNSn()
        {
            return (newN, newValue);
        }
        private int ConvertToNIndexSn(double n)
        {
            return (int)((n + maximumnNegativeN)*2);
        }

        public double LastIndexNSn(PN pn)
        {
            throw new NotImplementedException();
        }






        public override string ToString()
        {
            return $"TimeSpaceGridTest: Params={countParams}, LastN={GetLastNValue()}, LastK={GetLastKValue()}";
        }

        // Метод для получения последнего значения N
        private string GetLastNValue()
        {
            try
            {
                var lastNValues = string.Join(", ", data.Select(d => d?.LastOrDefault().n.ToString() ?? "null"));
                return lastNValues;
            }
            catch
            {
                return "error";
            }
        }

        // Метод для получения последнего значения K
        private string GetLastKValue()
        {
            try
            {
                var lastKValues = string.Join(", ", data.Select(d =>
                    d?.LastOrDefault().layer?.LastOrDefault().k.ToString() ?? "null"));
                return lastKValues;
            }
            catch
            {
                return "error";
            }
        }

        // Добавим метод для вывода данных в читаемом формате
        public string GetDebugInfo()
        {
            var sb = new StringBuilder();
            sb.AppendLine("TimeSpaceGridTest Debug Info:");
            sb.AppendLine($"Parameters count: {countParams}");
            sb.AppendLine($"Sn Parameters count: {countParamsSn}");

            // Выводим информацию по основным данным
            sb.AppendLine("\nMain Data (n, k layers):");
            for (int i = 0; i < countParams; i++)
            {
                var paramName = ((PN)i).ToString();
                var layers = data[i];
                sb.AppendLine($"{paramName}: {layers?.Count ?? 0} layers");

                if (layers != null && layers.Count > 0)
                {
                    sb.AppendLine($"  Last n: {layers.Last().n}, " +
                                  $"K values: {layers.Last().layer?.Count ?? 0}");
                }
            }

            // Выводим информацию по Sn данным
            sb.AppendLine("\nSn Data:");
            for (int i = 0; i < countParamsSn; i++)
            {
                var paramName = ((PN)i).ToString();
                var values = dataSn[i];
                sb.AppendLine($"{paramName}: {values?.Count ?? 0} values");

                if (values != null && values.Count > 0)
                {
                    sb.AppendLine($"  Last n: {values.Last().n}, " +
                                $"Value: {values.Last().value}");
                }
            }

            return sb.ToString();
        }

        // Добавим метод для получения значения параметра в виде строки
        public string GetValueString(PN pn, double n, double k)
        {
            try
            {
                return $"{pn}[{n},{k}] = {this[pn, n, k]}";
            }
            catch (Exception ex)
            {
                return $"{pn}[{n},{k}] error: {ex.Message}";
            }
        }

        // Добавим метод для получения Sn значения в виде строки
        public string GetSnValueString(PN pn, double n)
        {
            try
            {
                return $"{pn}Sn[{n}] = {GetSn(pn, n)}";
            }
            catch (Exception ex)
            {
                return $"{pn}Sn[{n}] error: {ex.Message}";
            }
        }

        public double[,] GetFullData(int pn)
        {
            throw new NotImplementedException();
        }

        //RAM<double, ISubGrid> ram;
        //public TimeSpaceGrid()
        //{
        //    ram = new RAM<double, ISubGrid>(5);
        //}


        //private List<ISubGrid> subGridPlus = new List<ISubGrid>();
        //private List<ISubGrid> subGridMinus = new List<ISubGrid>();

        //public ISubGrid this[LimitedDouble n]
        //{
        //    get
        //    {
        //        if (ram.isContains(n.Value))
        //            return ram.Get(n.Value);

        //        (var index, var subGrid) = ChooseIndexAndSubGrid(n);

        //        subGrid = AllocateMemorySubGridForTheIndex(subGrid, index, n);
        //        ram.Add(n.Value, subGrid[index]);
        //        return subGrid[index];
        //    }
        //    set
        //    {
        //        (var index,var subGrid)= ChooseIndexAndSubGrid(n);

        //        subGrid = AllocateMemorySubGridForTheIndex(subGrid, index, n);
        //        subGrid[index] = value;
        //    }
        //}

        //private (int index, List<ISubGrid> subGrid) ChooseIndexAndSubGrid(LimitedDouble n)
        //{
        //    int index;
        //    List<ISubGrid> subGrid;

        //    if (n.Value < 0)
        //    {
        //        index = ConvertNToIndexMinus(n);
        //        subGrid = subGridMinus;
        //    }
        //    else
        //    {
        //        index = ConvertNToIndex(n);
        //        subGrid = subGridPlus;
        //    }

        //    return (index, subGrid);
        //}

        //private int ConvertNToIndex(LimitedDouble n) => (int)(n.Value * 2);
        //private int ConvertNToIndexMinus(LimitedDouble n) => (int)(-n.Value * 2)-1;
        //private LimitedDouble ConvertIntToLimitedDouble(int n) => new LimitedDouble(n / 2.0);
        //private List<ISubGrid> AllocateMemorySubGridForTheIndex(List<ISubGrid> subGrid, int index, LimitedDouble n)
        //{
        //    subGrid.AllocateUpTo(index, GetNew);
        //    subGrid[index].n = n;
        //    return subGrid;
        //}
        //private SpaceGrid GetNew()
        //{
        //    return new SpaceGrid();
        //}

        //public LimitedDouble MinN
        //{
        //    get
        //    {
        //        if (subGridMinus.Count > 0) return subGridMinus[subGridMinus.Count - 1].n;
        //        else return subGridPlus[0].n;
        //    }
        //}
        //public LimitedDouble MaxN
        //{
        //    get
        //    {
        //        if (subGridPlus.Count > 0) return subGridPlus[subGridPlus.Count - 1].n;
        //        else 
        //        {
        //            if (subGridMinus[0].GetIsNull() == false) return subGridMinus[0].n; 
        //            else return subGridMinus[1].n;
        //        }   
        //    }
        //}
    }
}

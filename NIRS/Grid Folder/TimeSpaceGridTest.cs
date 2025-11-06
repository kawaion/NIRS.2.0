
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
using System.Runtime.CompilerServices;

namespace NIRS.Grid_Folder
{
    public class TimeSpaceGridTest : IGrid
    {
        RAM<(PN, double), List<(double k, double value)>> ram;

        Dictionary<LimitedDouble, int> dictKConverter = new Dictionary<LimitedDouble, int>();
        Dictionary<LimitedDouble, int> dictNConverter = new Dictionary<LimitedDouble, int>();
        public TimeSpaceGridTest()
        {
            ram = new RAM<(PN, double), List<(double k, double value)>>(100);
            ramSn = new RAM<(PN, double), double>(15);
            InicialiseData();
            InicialiseDataSn();
        }
        private void InicialiseData()
        {
            for (int i = 0; i < countParams; i++)
            {
                data[i] = new List<NLayer>();
            }
        }
        private void InicialiseDataSn()
        {
            for (int i = 0; i < countParamsSn; i++)
            {
                dataSn[i] = new List<NLayerSn>();
            }
        }
        const int countParams = 13;
        List<NLayer>[] data = new List<NLayer>[countParams]; // количество переменных

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
        private const int number_ro = (int)PN.rho;
        private const int number_m = (int)PN.m;

        public double this[PN pn, LimitedDouble n, LimitedDouble k]
        {
            get
            {

                if (pn == PN.One_minus_m)
                    return 1.02 - this[PN.m, n, k];

                var kIndex = ConvertToKIndex(k);

                //if (ram.isContains((pn, n)))
                //    return ram.Get((pn, n))[kIndex].value;//память

                var nIndex = ConvertToNIndex(n);

                    //ram.Add((pn, n), cells);//память

                return data[(int)pn][nIndex][kIndex].value;
            }
            set
            {
                //if (double.IsNaN(value))
                //{
                //    int c = 0;
                //}
                var nIndex = ConvertToNIndex(n);
                var kIndex = ConvertToKIndex(k);
                var dataPN = data[(int)pn];
                dataPN = AllocateMemoryNLayersForTheIndex(dataPN, nIndex, n);

                var klayers = dataPN[nIndex].kLayers;
                klayers = AllocateMemoryCellsForTheIndex(klayers, kIndex, k);
                klayers[kIndex].value = value; 
            }
        }

        private double ValidationZero(double value)
        {
            if (Math.Abs(value) < 1e-6)
                return 0;
            return value;
        }
        private int ConvertToNIndex(LimitedDouble n)
        {
            if (dictNConverter.TryGetValue(n, out int res))
                return res;
            else
            {
                var newRes = (n + maximumnNegativeN).GetInt();
                dictNConverter.Add(n, newRes);
                return newRes;
            }
        }
        private int ConvertToKIndex(LimitedDouble k)
        {
            if (dictKConverter.TryGetValue(k, out int res))
                return res;
            else
            {
                var newRes = (k + maximumnNegativeK).GetInt();
                dictKConverter.Add(k, newRes);
                return newRes;
            }
        }
        private List<NLayer> AllocateMemoryNLayersForTheIndex(List<NLayer>dataPN, int index, LimitedDouble n)
        {
            dataPN.AllocateUpTo(index, GetNewN);
            dataPN[index].n = n;
            return dataPN;
        }

        LimitedDouble newN = new LimitedDouble(-maximumnNegativeN - 1);
        private NLayer GetNewN()
        {
            return new NLayer();
        }

        private List<KLayer> AllocateMemoryCellsForTheIndex(List<KLayer> cells, int index, LimitedDouble k)
        {
            cells.AllocateUpTo(index, GetNewK);
            cells[index].k = k;
            return cells;
        }

        LimitedDouble newK = new LimitedDouble(-maximumnNegativeK - 1);
        const double newCellValue = 0;
        private KLayer GetNewK()
        {
            return new KLayer();
        }

        public LimitedDouble LastIndexK(PN pn, LimitedDouble n)
        {
            var nIndex = ConvertToNIndex(n);

            return data[(int)pn][nIndex].kLayers.Last().k;
        }
        public LimitedDouble LastIndexN(PN pn)
        {
            return data[(int)pn].Last().n;
        }







        const int countParamsSn = 15;
        List<NLayerSn>[] dataSn = new List<NLayerSn>[countParamsSn]; // количество переменных

        RAM<(PN, double), double> ramSn;

        private const int number_x = (int)PN.x;
        private const int number_vSn = (int)PN.vSn;

        public double GetSn(PN pn, LimitedDouble n)
        {
            //if (ramSn.isContains((pn, n)))
            //    return ramSn.Get((pn, n));//память

            
            if (pn == PN.One_minus_m)
                return 1.02 - GetSn(PN.m, n);
            if (pn == PN.v || pn == PN.w)
                return GetSn(PN.vSn, n);

            var nIndex = ConvertToNIndexSn(n, pn);

            var value = dataSn[(int)pn][nIndex].value;

                //ramSn.Add((pn, n), value);//память

            return value;
        }
        public void SetSn(PN pn, LimitedDouble n, double value)
        {
            var nIndex = ConvertToNIndexSn(n, pn);
            var dataSnPN = dataSn[(int)pn];
            dataSnPN = AllocateMemorySnForTheIndex(dataSnPN, nIndex, n);
            dataSnPN[nIndex].value = value;
        }
        private List<NLayerSn> AllocateMemorySnForTheIndex(List<NLayerSn> layers, int index, LimitedDouble n)
        {
            layers.AllocateUpTo(index, GetNewNSn);
            layers[index].n = n;
            return layers;
        }
        double newValue = 0;
        private NLayerSn GetNewNSn()
        {
            return new NLayerSn();
        }
        private int ConvertToNIndexSn(LimitedDouble n, PN pn)
        {
            if (pn == PN.x)
                return ((n + maximumnNegativeN) * 2).GetInt();
            else
                return ConvertToNIndex(n);
        }

        public LimitedDouble LastIndexNSn(PN pn)
        {
            return dataSn[(int)pn].Last().n;
        }






        //public override string ToString()
        //{
        //    return $"TimeSpaceGridTest: Params={countParams}, LastN={GetLastNValue()}, LastK={GetLastKValue()}";
        //}

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
        //private string GetLastKValue()
        //{
        //    try
        //    {
        //        var lastKValues = string.Join(", ", data.Select(d =>
        //            d?.LastOrDefault().layer?.LastOrDefault().k.ToString() ?? "null"));
        //        return lastKValues;
        //    }
        //    catch
        //    {
        //        return "error";
        //    }
        //}

        // Добавим метод для вывода данных в читаемом формате
        //public string GetDebugInfo()
        //{
        //    var sb = new StringBuilder();
        //    sb.AppendLine("TimeSpaceGridTest Debug Info:");
        //    sb.AppendLine($"Parameters count: {countParams}");
        //    sb.AppendLine($"Sn Parameters count: {countParamsSn}");

        //    // Выводим информацию по основным данным
        //    sb.AppendLine("\nMain Data (n, k layers):");
        //    for (int i = 0; i < countParams; i++)
        //    {
        //        var paramName = ((PN)i).ToString();
        //        var layers = data[i];
        //        sb.AppendLine($"{paramName}: {layers?.Count ?? 0} layers");

        //        if (layers != null && layers.Count > 0)
        //        {
        //            sb.AppendLine($"  Last n: {layers.Last().n}, " +
        //                          $"K values: {layers.Last().layer?.Count ?? 0}");
        //        }
        //    }

        //    // Выводим информацию по Sn данным
        //    sb.AppendLine("\nSn Data:");
        //    for (int i = 0; i < countParamsSn; i++)
        //    {
        //        var paramName = ((PN)i).ToString();
        //        var values = dataSn[i];
        //        sb.AppendLine($"{paramName}: {values?.Count ?? 0} values");

        //        if (values != null && values.Count > 0)
        //        {
        //            sb.AppendLine($"  Last n: {values.Last().n}, " +
        //                        $"Value: {values.Last().value}");
        //        }
        //    }

        //    return sb.ToString();
        //}

        // Добавим метод для получения значения параметра в виде строки
        public string GetValueString(PN pn, LimitedDouble n, LimitedDouble k)
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
        public string GetSnValueString(PN pn, LimitedDouble n)
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

        public LimitedDouble LastIndexK(LimitedDouble n)
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

    public class NLayer
    {
        public LimitedDouble n;
        public List<KLayer> kLayers = new List<KLayer>(); 
        public NLayer()
        {
            
        }
        public KLayer this[int k]
        {
            get
            {
                return kLayers[k];
            }
        }
    }
    public class KLayer
    {
        public LimitedDouble k;
        public double value;
        public KLayer()
        {

        }
    }
    public class NLayerSn
    {
        public LimitedDouble n;
        public double value;
        public NLayerSn()
        {

        }
    }
}

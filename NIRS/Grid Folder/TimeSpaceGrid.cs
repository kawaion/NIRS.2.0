using NIRS.Helpers;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NIRS.Grid_Folder
{
    public class TimeSpaceGrid : IGrid
    {
        private const int InitialCapacity = 256; // Начальный размер массива
        private const double GrowthFactor = 2; // Множитель роста

        public double[,,] data; // [paramIndex, nIndex, kIndex]
        //private DynamicBlock3DArray<double> data;
        private double[,] currentKSize; // [paramIndex, nIndex]
        private double[] currentNSize;  // [paramIndex]

        const int countParams = 13;
        const int maximumnNegativeN = 1;
        const int maximumnNegativeK = 1;

        public TimeSpaceGrid()
        {
            InitializeData();
            InitializeDataSn();
        }
        public double[,] GetFullData(int pn)
        {
            double[,] datapn = new double[data.GetLength(1),data.GetLength(2)];
            for (int i = 0; i < data.GetLength(1); i++)
                for(int j = 0; j < data.GetLength(2); j++)
                {
                    datapn[i,j] = data[pn,i,j];
                }
            return datapn;
        }
        private void InitializeData()
        {
            //data = new DynamicBlock3DArray<double>(countParams, 512, 512);
            data = new double[countParams, InitialCapacity, InitialCapacity];
            currentNSize = new double[countParams];
            currentKSize = new double[countParams, InitialCapacity];
        }
        public double this[PN pn, double n, double k]
        {
            get
            {
                if (pn == PN.One_minus_m)
                    return 1.02 - this[PN.m, n, k];

                var paramIndex = (int)pn;
                var nIndex = ConvertToNIndex(n);
                var kIndex = ConvertToKIndex(k);

                return data[paramIndex, nIndex, kIndex];
            }
            set
            {
                if (double.IsNaN(value))
                {
                    int с = 0;
                }
                if (double.IsInfinity(value))
                {
                    int с = 0;
                }
                var paramIndex = (int)pn;
                var nIndex = ConvertToNIndex(n);
                var kIndex = ConvertToKIndex(k);
                
                EnsureKSizeCapacity(paramIndex, nIndex);

                EnsureCapacity(paramIndex, nIndex, kIndex);
                data[paramIndex, nIndex, kIndex] = Validation(value);

                // Обновляем currentNSize
                if (nIndex > currentNSize[paramIndex])
                    currentNSize[paramIndex] = n;

                // Обновляем currentKSize
                if (kIndex > currentKSize[paramIndex, nIndex])
                    currentKSize[paramIndex, nIndex] = k;
                    
            }
        }
        private void EnsureKSizeCapacity(int paramIndex, int nIndex)
        {
            if (nIndex >= currentKSize.GetLength(1))
            {
                int newSize = (int)(currentKSize.GetLength(1) * GrowthFactor);
                if (newSize <= nIndex)
                    newSize = nIndex + 1;

                var newCurrentKSize = new double[countParams, newSize];

                // Копируем существующие данные
                for (int p = 0; p < countParams; p++)
                {
                    for (int n = 0; n < currentKSize.GetLength(1); n++)
                    {
                        newCurrentKSize[p, n] = currentKSize[p, n];
                    }
                }

                currentKSize = newCurrentKSize;
            }
        }
        private void EnsureCapacity(int paramIndex, int nIndex, int kIndex)
        {
            // Проверяем необходимость расширения по n
            if (nIndex >= data.GetLength(1))
            {
                var newNSize = CalculateNewSize(data.GetLength(1), nIndex);
                ResizeArray(paramIndex, newNSize, data.GetLength(2));
            }

            // Проверяем необходимость расширения по k
            if (kIndex >= data.GetLength(2))
            {
                var newKSize = CalculateNewSize(data.GetLength(2), kIndex);
                ResizeArray(paramIndex, data.GetLength(1), newKSize);
            }
        }

        private int CalculateNewSize(int currentSize, int requiredIndex)
        {
            int newSize = currentSize;
            while (newSize <= requiredIndex)
            {
                newSize = (int)(newSize * GrowthFactor);
            }
            return Math.Max(newSize, InitialCapacity);
        }

        private void ResizeArray(int paramIndex, int newNSize, int newKSize)
        {
            var newData = new double[data.GetLength(0), newNSize, newKSize];
            var newCurrentKSize = new double[countParams, newNSize];

            // Копируем данные
            for (int p = 0; p < data.GetLength(0); p++)
            {
                for (int n = 0; n < data.GetLength(1); n++)
                {
                    Array.Copy(data,
                             p * data.GetLength(1) * data.GetLength(2) + n * data.GetLength(2),
                             newData,
                             p * newNSize * newKSize + n * newKSize,
                             Math.Min(data.GetLength(2), newKSize));

                    if (n < currentKSize.GetLength(1))
                        newCurrentKSize[p, n] = currentKSize[p, n];
                }
            }

            data = newData;
            currentKSize = newCurrentKSize;
        }
        //private void ResizeArray(int paramIndex, int newNSize, int newKSize)
        //{
        //    var newCurrentKSize = new double[countParams, newNSize];

        //    // Копируем данные
        //    for (int p = 0; p < countParams; p++)
        //    {
        //        for (int n = 0; n < data.GetLength(1); n++)
        //        {
        //            if (n < currentKSize.GetLength(1))
        //                newCurrentKSize[p, n] = currentKSize[p, n];
        //        }
        //    }

        //    currentKSize = newCurrentKSize;
        //}

        // Остальные методы остаются без изменений
        private double Validation(double value) => value;//Math.Abs(value) < 1e-6 ? 0 : value;

        private int ConvertToNIndex(double n)
        {
            return (int)(n + maximumnNegativeN);
        }

        private int ConvertToKIndex(double k)
        {
            return (int)(k + maximumnNegativeK);
        }

        public double LastIndexK(PN pn, double n)
        {
            var paramIndex = (int)pn;
            var nIndex = ConvertToNIndex(n);
            return currentKSize[paramIndex, nIndex];
        }

        public double LastIndexN(PN pn)
        {
            var paramIndex = (int)pn;
            return currentNSize[paramIndex];
        }





        const int countParamsSn = 15;
        private double[,] dataSn; // [paramIndex, nIndex]
        private double[] currentNSizeSn; // Текущий размер для каждого параметра

        private void InitializeDataSn()
        {
            dataSn = new double[countParamsSn, InitialCapacity];
            currentNSizeSn = new double[countParamsSn];
        }

        public double GetSn(PN pn, double n)
        {
            if (pn == PN.One_minus_m)
                return 1.02 - GetSn(PN.m, n);

            var paramIndex = (int)pn;
            var nIndex = ConvertToNIndexSn(n);
            return dataSn[paramIndex, nIndex];
        }

        public void SetSn(PN pn, double n, double value)
        {
            if (double.IsNaN(value))
            {
                int с = 0;
            }
            if (double.IsInfinity(value))
            {
                int с = 0;
            }
            var paramIndex = (int)pn;
            var nIndex = ConvertToNIndexSn(n);
            EnsureCapacitySn(paramIndex, nIndex);
            dataSn[paramIndex, nIndex] = Validation(value);
            currentNSizeSn[paramIndex] = n;
        }
        private void EnsureCapacitySn(int paramIndex, int nIndex)
        {
            // Проверяем необходимость расширения по n
            if (nIndex >= dataSn.GetLength(1))
            {
                var newNSize = CalculateNewSize(dataSn.GetLength(1), nIndex);
                ResizeSnArrays(newNSize);
            }
        }

        private void ResizeSnArrays(int newSize)
        {
            // Создаем новые массивы
            var newDataSn = new double[dataSn.GetLength(0), newSize];

            // Копируем данные
            for (int p = 0; p < dataSn.GetLength(0); p++)
            {
                Array.Copy(dataSn,
                         p * dataSn.GetLength(1),
                         newDataSn,
                         p * newSize,
                         Math.Min(dataSn.GetLength(1), newSize));
            }

            // Обновляем ссылки
            dataSn = newDataSn;
        }
        private int ConvertToNIndexSn(double n)
        {
            return (int)((n + maximumnNegativeN) * 2);
        }
        public double LastIndexNSn(PN pn)
        {
            var paramIndex = (int)pn;
            return currentNSizeSn[paramIndex];
        }
    }
}

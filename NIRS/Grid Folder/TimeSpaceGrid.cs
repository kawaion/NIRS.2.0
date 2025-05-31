using NIRS.Helpers;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;

namespace NIRS.Grid_Folder
{
    public class TimeSpaceGrid : IGrid
    {
        private const int InitialCapacity = 4; // Начальный размер массива
        private const float GrowthFactor = 2f; // Множитель роста

        private double[,,] data; // [paramIndex, nIndex, kIndex]
        private int[,] currentKSize; // [paramIndex, nIndex]
        private int[] currentNSize;  // [paramIndex]

        const int countParams = 13;
        const int maximumnNegativeN = 1;
        const int maximumnNegativeK = 1;

        public TimeSpaceGrid()
        {
            data = new double[countParams, InitialCapacity, InitialCapacity];
            currentNSize = new int[countParams];
            currentKSize = new int[countParams, InitialCapacity];
        }

        public double this[PN pn, double n, double k]
        {
            get
            {
                if (pn == PN.One_minus_m)
                    return 1 - this[PN.m, n, k];

                var paramIndex = (int)pn;
                var nIndex = ConvertToNIndex(n);
                var kIndex = ConvertToKIndex(k);

                EnsureCapacity(paramIndex, nIndex, kIndex);
                return data[paramIndex, nIndex, kIndex];
            }
            set
            {
                var paramIndex = (int)pn;
                var nIndex = ConvertToNIndex(n);
                var kIndex = ConvertToKIndex(k);

                EnsureCapacity(paramIndex, nIndex, kIndex);
                data[paramIndex, nIndex, kIndex] = Validation(value);

                // Обновляем currentKSize
                if (kIndex > currentKSize[paramIndex, nIndex])
                    currentKSize[paramIndex, nIndex] = kIndex;
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

            // Обновляем currentNSize
            if (nIndex > currentNSize[paramIndex])
                currentNSize[paramIndex] = nIndex;
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
            var newCurrentKSize = new int[countParams, newNSize];

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

        // Остальные методы остаются без изменений
        private double Validation(double value) => Math.Abs(value) < 1e-6 ? 0 : value;

        private int ConvertToNIndex(double n)
        {
            if (n.IsInt()) return (int)(n + maximumnNegativeN);
            if (n.IsHalfInt()) return (int)(n - 0.5 + maximumnNegativeN);
            throw new Exception($"Invalid n value: {n}");
        }

        private int ConvertToKIndex(double k)
        {
            if (k.IsInt()) return (int)(k + maximumnNegativeK);
            if (k.IsHalfInt()) return (int)(k - 0.5 + maximumnNegativeK);
            throw new Exception($"Invalid k value: {k}");
        }

        public double LastIndexK(PN pn, double n)
        {
            var paramIndex = (int)pn;
            var nIndex = ConvertToNIndex(n);
            return currentKSize[paramIndex, nIndex] - maximumnNegativeK;
        }

        public double LastIndexN(PN pn)
        {
            var paramIndex = (int)pn;
            return currentNSize[paramIndex] - maximumnNegativeN;
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
                return 1 - dataSn[(int)PN.m][nIndex].value;
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
            return (int)((n + maximumnNegativeN) * 2);
        }
    }
}

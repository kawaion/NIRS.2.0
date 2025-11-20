using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Helpers
{
    public static class Array2DConverter
    {
        public static double[,] ConvertTo2DArray(this List<List<double>> list)
        {
            if (list == null || list.Count == 0)
                return new double[0, 0];

            // Находим максимальную длину внутренних списков
            int maxRows = list.Count;
            int maxCols = list.Max(innerList => innerList?.Count ?? 0);

            double[,] result = new double[maxRows, maxCols];

            for (int i = 0; i < maxRows; i++)
            {
                if (list[i] != null)
                {
                    for (int j = 0; j < maxCols; j++)
                    {
                        // Если элемент существует - берем его, иначе 0
                        result[i, j] = j < list[i].Count ? list[i][j] : 0;
                    }
                }
                else
                {
                    // Если вся строка null - заполняем нулями
                    for (int j = 0; j < maxCols; j++)
                    {
                        result[i, j] = 0;
                    }
                }
            }

            return result;
        }
    }
}

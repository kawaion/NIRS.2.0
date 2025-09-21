using MyDouble;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Grid_Folder.Subfolder
{
    static class CapasityEnsurerForArray
    {
        public static T[,,] Resize3DArray<T>(this T[,,] original, int newX, int newY, int newZ)
        {
            T[,,] newArray = new T[newX, newY, newZ];

            int xLimit = Math.Min(original.GetLength(0), newX);
            int yLimit = Math.Min(original.GetLength(1), newY);
            int zLimit = Math.Min(original.GetLength(2), newZ);

            for (int x = 0; x < xLimit; x++)
            {
                for (int y = 0; y < yLimit; y++)
                {
                    for (int z = 0; z < zLimit; z++)
                    {
                        newArray[x, y, z] = original[x, y, z];
                    }
                }
            }

            return newArray;
        }

        public static T[,] Resize2DArray<T>(this T[,] original, int newX, int newY)
        {
            T[,] newArray = new T[newX, newY];

            int xLimit = Math.Min(original.GetLength(0), newX);
            int yLimit = Math.Min(original.GetLength(1), newY);

            for (int x = 0; x < xLimit; x++)
            {
                for (int y = 0; y < yLimit; y++)
                {
                    newArray[x, y] = original[x, y];

                }
            }

            return newArray;
        }





        //private void EnsureKSizeCapacity(int paramIndex, int nIndex, LimitedDouble[,] currentKSize)
        //{
        //    if (nIndex >= currentKSize.GetLength(1))
        //    {
        //        int newSize = (int)(currentKSize.GetLength(1) * GrowthFactor);
        //        if (newSize <= nIndex)
        //            newSize = nIndex + 1;

        //        var newCurrentKSize = new double[countParams, newSize];

        //        // Копируем существующие данные
        //        for (int p = 0; p < countParams; p++)
        //        {
        //            for (int n = 0; n < currentKSize.GetLength(1); n++)
        //            {
        //                newCurrentKSize[p, n] = currentKSize[p, n];
        //            }
        //        }

        //        currentKSize = newCurrentKSize;
        //    }
        //}
    }
}

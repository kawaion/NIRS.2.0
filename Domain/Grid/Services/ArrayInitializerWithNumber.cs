using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Grid.Services;

internal static class ArrayInitializerWithNumber
{
    public static double[,] Initialize(double[,] slice, double value)
    {
        int rows = slice.GetLength(0);
        int cols = slice.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                slice[i, j] = value;
            }
        }
        return slice;
    }

    public static double[] Initialize(double[] array, double value)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = value;
        }
        return array;
    }
}

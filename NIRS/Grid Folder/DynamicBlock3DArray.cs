using System.Collections.Generic;
using System;

public class DynamicBlock3DArray<T> where T : struct
{
    private readonly int blockSize;
    private readonly Dictionary<(int, int, int), T[,,]> blocks = new Dictionary<(int, int, int), T[,,]>();
    private readonly int fixedXSize;

    public DynamicBlock3DArray(int fixedXSize, int initialBlockSizeY = 16, int initialBlockSizeZ = 16)
    {
        this.fixedXSize = fixedXSize;
        this.blockSize = initialBlockSizeY; // Используем одинаковый размер для Y и Z
    }

    private (int by, int bz) GetBlockCoords(int y, int z)
    {
        return (y / blockSize, z / blockSize);
    }

    private (int ly, int lz) GetLocalCoords(int y, int z)
    {
        return (y % blockSize, z % blockSize);
    }

    public T this[int x, int y, int z]
    {
        get
        {
            if (x < 0 || x >= fixedXSize || y < 0 || z < 0)
                return default;

            var (by, bz) = GetBlockCoords(y, z);
            if (!blocks.TryGetValue((x, by, bz), out var block))
                return default;

            var (ly, lz) = GetLocalCoords(y, z);
            return block[0, ly, lz]; // X фиксирован в рамках блока
        }
        set
        {
            if (x < 0 || x >= fixedXSize || y < 0 || z < 0)
                return;

            var (by, bz) = GetBlockCoords(y, z);
            var blockKey = (x, by, bz);

            if (!blocks.TryGetValue(blockKey, out var block))
            {
                block = new T[1, blockSize, blockSize]; // X размер = 1, так как он фиксирован
                blocks[blockKey] = block;
            }

            var (ly, lz) = GetLocalCoords(y, z);
            block[0, ly, lz] = value;
        }
    }
    private int GetMaxY()
    {
        int max = 0;
        foreach (var key in blocks.Keys)
        {
            max = Math.Max(max, key.Item2 * blockSize + blockSize - 1);
        }
        return max;
    }

    private int GetMaxZ()
    {
        int max = 0;
        foreach (var key in blocks.Keys)
        {
            max = Math.Max(max, key.Item3 * blockSize + blockSize - 1);
        }
        return max;
    }
    public int GetLength(int dimension)
        {
            switch(dimension )
            {
                case 0: return fixedXSize;                         // Фиксированный размер по X
                case 1: return GetMaxY() + 1;                           // Текущий размер по Y
                case 2: return GetMaxZ() + 1;                            // Текущий размер по Z
                default: throw new IndexOutOfRangeException("Dimension must be 0, 1 or 2");
            };
        }
}

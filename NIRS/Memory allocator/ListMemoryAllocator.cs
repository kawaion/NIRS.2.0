using MyDouble;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Memory_allocator
{
    public static class ListMemoryAllocator
    {
        public static List<T> AllocateUpTo<T>(this List<T> list, int index) where T: class
        {
            while (list.Count <= index)
                list.Add(null);

            return list;
        }
        public static List<T> AllocateUpTo<T>(this List<T> list, int index, GetNew<T> getNew) where T : class
        {
            while (list.Count <= index)
                list.Add(getNew());

            return list;
        }
        public static List<(LimitedDouble n, List<(LimitedDouble k, double layer)> layer)> AllocateUpTo(this List<(LimitedDouble n, List<(LimitedDouble k, double layer)> layer)> list, int index, GetNew<(LimitedDouble n, List<(LimitedDouble k, double layer)> layer)> getNew)
        {
            while (list.Count <= index)
                list.Add(getNew());

            return list;
        }
        public static List<(LimitedDouble k, double cell)> AllocateUpTo(this List<(LimitedDouble k, double cell)> list, int index, GetNew<(LimitedDouble k, double cell)> getNew)
        {
            while (list.Count <= index)
                list.Add(getNew());

            return list;
        }
    }
    public delegate T GetNew<T>();
}

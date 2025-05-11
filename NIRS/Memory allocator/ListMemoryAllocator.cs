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
        public static List<(double n, List<(double k, double layer)> layer)> AllocateUpTo(this List<(double n, List<(double k, double layer)> layer)> list, int index, GetNew<(double n, List<(double k, double layer)> layer)> getNew)
        {
            while (list.Count <= index)
                list.Add(getNew());

            return list;
        }
        public static List<(double k, double cell)> AllocateUpTo(this List<(double k, double cell)> list, int index, GetNew<(double k, double cell)> getNew)
        {
            while (list.Count <= index)
                list.Add(getNew());

            return list;
        }
    }
    public delegate T GetNew<T>();
}

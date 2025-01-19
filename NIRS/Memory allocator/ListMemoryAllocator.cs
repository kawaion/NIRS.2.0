using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}

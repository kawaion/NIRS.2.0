using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.RAM_folder
{
    class RAM<K,T>
    {
        Dictionary<K, T> memory = new Dictionary<K, T>();
        Queue<K> queue = new Queue<K>();

        int _maxsize;
        public RAM(int maxSize)
        {
            _maxsize = maxSize;
        }
        public bool isContains(K key)
        {
            return memory.ContainsKey(key);
        } 
        public T Get(K key)
        {
            return memory[key];
        }

        public void Add(K key,T value)
        {
            memory.Add(key, value);
            queue.Enqueue(key);

            if (memory.Count > _maxsize)
            {
                var mostRecentKey = queue.Dequeue();
                memory.Remove(mostRecentKey);
            }
        }
    }
}

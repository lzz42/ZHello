using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHello.Algorithm.ALCache
{
    public interface IDataCache<K,V>
    {
        public void SetCapacity(int capacity);
        public void SetValue(K k, V v);
        public V GetValue(K k);
    }

    public abstract class DataCache<K, V> : IDataCache<K, V>
    {
        public abstract V GetValue(K k);
        public abstract void SetValue(K k, V v);
        public abstract void SetCapacity(int capacity);
        protected abstract bool IsFull();
        protected abstract void Clear();
    }
}

using System.Collections;
using System.Collections.Generic;

namespace ProxyGen.Helper
{
    public class SingleEnumerable<T> : IEnumerable<T>
    {
        private List<T> _enumerable;

        public SingleEnumerable(T item)
        {
            _enumerable = new List<T>();
            _enumerable.Add(item);
        }


        public IEnumerator<T> GetEnumerator()
        {
            return _enumerable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
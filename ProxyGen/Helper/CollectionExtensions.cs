using System;
using System.Collections.Generic;

namespace ProxyGen.Helper
{
    public static class CollectionExtensions
    {
        public static void ForEach<T>(this IList<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }
    }
}
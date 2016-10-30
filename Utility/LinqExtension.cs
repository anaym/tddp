using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility
{
    public static class LinqExtension
    {
        public static T MinOrDefault<T>(this IEnumerable<T> seq, Func<T, int> keyExtractor)
        {
            var minKey = int.MaxValue;
            var data = default(T);
            foreach (var item in seq)
            {
                var currentKey = keyExtractor(item);
                if (currentKey < minKey)
                {
                    minKey = currentKey;
                    data = item;
                }
            }
            return data;
        }

        public static Dictionary<TK, TV> ToDictionary<TK, TV>(this IEnumerable<KeyValuePair<TK, TV>> seq)
        {
            return seq.ToDictionary(p => p.Key, p => p.Value);
        }
    }
}
using System;
using System.Collections.Generic;

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
    }
}
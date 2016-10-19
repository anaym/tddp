using System;
using System.Collections.Generic;

namespace TagsCloudVisualization.Extensions
{
    public static class LinqExtension
    {
        public static T MinOrDefault<T>(this IEnumerable<T> seq, Func<T, int> keyExtractor)
        {
            var min = int.MaxValue;
            var data = default(T);
            foreach (var item in seq)
            {
                var now = keyExtractor(item);
                if (now < min)
                {
                    min = now;
                    data = item;
                }
            }
            return data;
        }
    }
}
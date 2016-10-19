using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization.Extensions
{
    public static class DictionaryExtension
    {
        public static Dictionary<TK, TV> ToDictionary<TK, TV>(this IEnumerable<KeyValuePair<TK, TV>> seq)
        {
            return seq.ToDictionary(item => item.Key, item => item.Value);
        }
    }
}

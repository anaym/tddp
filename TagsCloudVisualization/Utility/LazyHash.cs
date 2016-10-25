using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization.Utility
{
    public static class LazyHash
    {
        public static int GetHashCode(Object a, Object b)
        {
            unchecked
            {
                return (a.GetHashCode() << 16) ^ b.GetHashCode();
            }
        }
    }
}

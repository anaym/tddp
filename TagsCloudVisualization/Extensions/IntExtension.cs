using System;

namespace TagsCloudVisualization.Extensions
{
    public static class IntExtension
    {
        public static bool IsInRange(this int num, int a, int b)
        {
            return num <= Math.Max(a, b) && num >= Math.Min(a, b);
        }
    }
}
namespace TagsCloudVisualization.Utility
{
    public static class LazyHash
    {
        public static int GetHashCode<TA, TB>(TA a, TB b) => GetHashCode(a.GetHashCode(), b.GetHashCode());

        // CR (krait): А это теперь зачем?
        public static int GetHashCode(int a, int b)
        {
            unchecked
            {
                return (a << 16) ^ b;
            }
        }
    }
}

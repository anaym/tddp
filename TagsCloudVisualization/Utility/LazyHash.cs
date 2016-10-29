namespace TagsCloudVisualization.Utility
{
    public static class LazyHash
    {
        public static int GetHashCode<TA, TB>(TA a, TB b)
        {
            unchecked
            {
                return (a.GetHashCode() << 16) ^ b.GetHashCode();
            }
        }
    }
}

using System;

namespace Utility.Geometry
{
    public struct ParallelSegment : IEquatable<ParallelSegment>
    {
        public readonly int Left;
        public readonly int Right;

        public ParallelSegment(int left, int right)
        {
            this.Left = Math.Min(left, right);
            this.Right = Math.Max(left, right);
        }

        public bool Contains(int n, bool includeBorders = true)
        {
            return includeBorders ? (n >= Left && n <= Right) : (n > Left && n < Right);
        }

        public bool IsIntersected(ParallelSegment other, bool includeBorder = true)
        {
            if (Equals(other))
                return true;
            
            var thisContainsPointsOfOther = Contains(other.Left, includeBorder) || Contains(other.Right, includeBorder);
            var otherContainsPointsOfThis = other.Contains(Left, includeBorder) || other.Contains(Right, includeBorder);
            return thisContainsPointsOfOther || otherContainsPointsOfThis;
        }

        public override int GetHashCode() => LazyHash.GetHashCode(Left, Right);
        public bool Equals(ParallelSegment other) => Left == other.Left && Right == other.Right;
        public override bool Equals(object obj) => obj is ParallelSegment && Equals((ParallelSegment)obj);
        public override string ToString() => $"[{Left}, {Right}]";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization.Geometry
{
    public class ParallelSegment : IEquatable<ParallelSegment>
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
            var a = Contains(other.Left, includeBorder) || Contains(other.Right, includeBorder);
            var b = other.Contains(Left, includeBorder) || other.Contains(Right, includeBorder);
            return Equals(other) || a || b;
        }

        public override int GetHashCode() => Left ^ Right;
        public bool Equals(ParallelSegment other) => other != null && Left == other.Left && Right == other.Right;
        public override bool Equals(object obj) => Equals(obj as ParallelSegment);
        public override string ToString() => $"[{Left}, {Right}]";
    }
}

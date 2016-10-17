using System;

namespace TagsCloudVisualization
{
    public class Vector : IEquatable<Vector>
    {
        public readonly int X;
        public readonly int Y;

        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Vector other) => other != null && (other.X == X && other.Y == Y);
        public override int GetHashCode() => X ^ Y;
        public override bool Equals(object obj) => (obj as Vector)?.Equals(this) ?? false;
    }
}
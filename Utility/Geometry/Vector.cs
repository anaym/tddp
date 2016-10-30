using System;

namespace Utility.Geometry
{
    public struct Vector : IEquatable<Vector>
    {
        public readonly int X;
        public readonly int Y;
        
        public static readonly Vector Zero = new Vector(0, 0);

        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int Norm => (int)Math.Sqrt(X * X + Y * Y);
        public int DistanceTo(Vector other) => (this - other).Norm;
        
        #region Operators

        public static Vector operator +(Vector left, Vector right) => new Vector(left.X + right.X, left.Y + right.Y);
        public static Vector operator *(Vector left, int right) => new Vector(left.X * right, left.Y * right);
        public static Vector operator /(Vector left, int right) => new Vector(left.X / right, left.Y / right);
        public static Vector operator -(Vector left) => new Vector(-left.X, -left.Y);
        public static Vector operator -(Vector left, Vector right) => left + -right;

        #endregion

        public bool Equals(Vector other) => other.X == X && other.Y == Y;
        public override int GetHashCode() => LazyHash.GetHashCode(X, Y);
        public override bool Equals(object obj) => obj is Vector && ((Vector) obj).Equals(this);
        public override string ToString() => $"({X}, {Y})";
    }
}
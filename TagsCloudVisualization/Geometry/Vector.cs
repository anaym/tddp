using System;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class Vector : IEquatable<Vector>
    {
        public readonly int X;
        public readonly int Y;

        public static Vector Zero => new Vector(0, 0);

        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int Norm => (int)Math.Sqrt(X*X + Y*Y);
        public int DistanceTo(Vector other) => Sub(other).Norm;

        public Vector Add(Vector other)
        {
            return new Vector(X + other.X, Y + other.Y);
        }
        public Vector Sub(Vector other) => Add(other.Mul(-1));
        public Vector Mul(int k) => new Vector(k*X, k*Y);
        public Vector Div(int k) => new Vector(X/k, Y/k);
        public int ScalarMul(Vector other) => other.X*X + other.Y*Y;

        public bool Equals(Vector other) => other != null && (other.X == X && other.Y == Y);
        public override int GetHashCode() => X ^ Y;
        public override bool Equals(object obj) => (obj as Vector)?.Equals(this) ?? false;
        public override string ToString() => $"({X}, {Y})";
    }
}
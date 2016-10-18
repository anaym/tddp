using System;

namespace TagsCloudVisualization.Geometry
{
    public class Size : IEquatable<Size>, IComparable<Size>
    {
        public readonly int Width;
        public readonly int Height;

        public static Size Empty => new Size(0, 0);

        public Size(int width, int height)
        {
            if (width < 0 || height < 0)
                throw new ArgumentException("Width and height must be a non negative!");
            Width = width;
            Height = height;
        }

        public int Square => Width*Height;

        public int CompareTo(Size other)
        {
            if (other == null)
                return 1;
            var wd = Width.CompareTo(other.Width);
            return wd != 0 ? wd : Height.CompareTo(other.Height);
        }
        public bool Equals(Size other) => other != null && (Width == other.Width && Height == other.Height);
        public override int GetHashCode() => Width ^ Height;
        public override bool Equals(object obj) => (obj as Size)?.Equals(this) ?? false;
        public override string ToString() => $"({Width}, {Height})";

    }
}
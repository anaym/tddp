using System;

namespace TagsCloudVisualization.Geometry
{
    // CR (krait): Стоит сделать структурой. См. комментарий к ParallelSegment.

    public class Size : IEquatable<Size>, IComparable<Size>
    {
        public readonly int Width;
        public readonly int Height;

        // CR (krait): Зачем каждый раз пересоздавать одинаковый Size?
        public static Size Empty => new Size(0, 0);

        public Size(int width, int height)
        {
            if (width < 0 || height < 0)
                throw new ArgumentException("Width and height must be non negative!");
            Width = width;
            Height = height;
        }

        // CR (krait): Площадь по-английски называется Area.
        public int Square => Width * Height;

        public int CompareTo(Size other)
        {
            if (other == null)
                return 1;
            var wd = Width.CompareTo(other.Width);
            return wd != 0 ? wd : Height.CompareTo(other.Height);
        }
        public bool Equals(Size other) => other != null && (Width == other.Width && Height == other.Height);
        // CR (krait): Плохой хеш: будет одинаковым у (w, h) и (h, w).
        public override int GetHashCode() => Width ^ Height;
        public override bool Equals(object obj) => (obj as Size)?.Equals(this) ?? false;
        public override string ToString() => $"({Width}, {Height})";

    }
}
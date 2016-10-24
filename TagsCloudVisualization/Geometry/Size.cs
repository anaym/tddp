using System;

namespace TagsCloudVisualization.Geometry
{
    // !CR (krait): Стоит сделать структурой. См. комментарий к ParallelSegment.

    public struct Size : IEquatable<Size>, IComparable<Size>
    {
        public readonly int Width;
        public readonly int Height;

        // !CR (krait): Зачем каждый раз пересоздавать одинаковый Size?
        public static readonly Size Empty = new Size(0, 0);

        public Size(int width, int height)
        {
            if (width < 0 || height < 0)
                throw new ArgumentException("Width and height must be non negative!");
            Width = width;
            Height = height;
        }

        // !CR (krait): Площадь по-английски называется Area.
        public int Area => Width * Height;

        public int CompareTo(Size other)
        {
            var wd = Width.CompareTo(other.Width);
            return wd != 0 ? wd : Height.CompareTo(other.Height);
        }
        public bool Equals(Size other) => Width == other.Width && Height == other.Height;
        // !CR (krait): Плохой хеш: будет одинаковым у (w, h) и (h, w).
        public override int GetHashCode() => (-Width) ^ Height;
        public override bool Equals(object obj) => obj is Size && ((Size)obj).Equals(this);
        public override string ToString() => $"({Width}, {Height})";

    }
}
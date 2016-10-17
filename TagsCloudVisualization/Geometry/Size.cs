﻿using System;

namespace TagsCloudVisualization
{
    public class Size : IEquatable<Size>, IComparable<Size>
    {
        public readonly int Width;
        public readonly int Height;

        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }

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
    }
}
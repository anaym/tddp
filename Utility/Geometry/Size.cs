using System;

namespace Utility.Geometry
{
    public struct Size : IEquatable<Size>
    {
        public readonly int Width;
        public readonly int Height;
        
        public static readonly Size Empty = new Size(0, 0);

        public Size(int width, int height)
        {
            if (width < 0 || height < 0)
                throw new ArgumentException("Width and height must be non negative!");
            Width = width;
            Height = height;
        }
        
        public int Area => Width * Height;

        public bool Equals(Size other) => Width == other.Width && Height == other.Height;
        public override int GetHashCode() => LazyHash.GetHashCode(Width, Height);
        public override bool Equals(object obj) => obj is Size && ((Size)obj).Equals(this);
        public override string ToString() => $"({Width}, {Height})";
    }
}
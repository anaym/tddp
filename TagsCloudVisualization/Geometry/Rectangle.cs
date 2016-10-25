using NUnit.Framework;
using TagsCloudVisualization.Utility;

namespace TagsCloudVisualization.Geometry
{
    // !CR (krait): Стоит сделать структурой. См. комментарий к ParallelSegment.

    public struct Rectangle
    {
        public readonly Size Size;
        public readonly Vector RightTop;
        public Vector RightBottom => new Vector(Right, Bottom);
        public Vector LeftTop => new Vector(Left, Top);
        public Vector LeftBottom => new Vector(Left, Bottom);
        public Vector Centre => RightTop - Size.ToVector()/2;

        public static readonly Rectangle Empty = new Rectangle(Vector.Zero, Size.Empty);

        public static Rectangle FromRightTop(Vector rightTop, Size size) => new Rectangle(rightTop, size);
        public static Rectangle FromRightBottom(Vector rightBottom, Size size) => new Rectangle(rightBottom + new Vector(0, size.Height), size);
        public static Rectangle FromLeftTop(Vector leftTop, Size size) => new Rectangle(leftTop + new Vector(size.Width, 0), size);
        public static Rectangle FromLeftBottom(Vector leftBottom, Size size) => new Rectangle(leftBottom + size.ToVector(), size);
        public static Rectangle FromCentre(Vector centre, Size size) => new Rectangle(centre + size.ToVector()/2, size);

        public Rectangle(int right, int top, int left, int bottom) : this(new Vector(right, top), new Vector(left, bottom))
        { }

        public Rectangle(Vector rightTop, Vector leftBottom) : this(rightTop, rightTop.ToSize(leftBottom))
        { }

        public Rectangle(Vector rightTop, Size size)
        {
            RightTop = rightTop;
            Size = size;
        }

        public int Top => RightTop.Y;
        public int Right => RightTop.X;
        public int Bottom => Top - Size.Height;
        public int Left => Right - Size.Width;

        public ParallelSegment XProjection => new ParallelSegment(Left, Right);
        public ParallelSegment YProjection => new ParallelSegment(Bottom, Top);

        public bool IsIntersected(Rectangle other, bool includeContur=true)
        {
            return XProjection.IsIntersected(other.XProjection, includeContur) &&
                   YProjection.IsIntersected(other.YProjection, includeContur);
        }

        public bool Contains(Vector other, bool include)
        {
            return new ParallelSegment(Left, Right).Contains(other.X, include) &&
                   new ParallelSegment(Bottom, Top).Contains(other.Y, include);
        }

        public bool Contains(Rectangle other, bool include)
        {
            return Contains(LeftTop, include) && Contains(RightBottom, include);
        }

        public bool Equals(Rectangle other) => Size.Equals(other.Size) && Centre.Equals(other.Centre);
        // !CR (krait): Почему тут не учитывается позиция?
        public override int GetHashCode() => LazyHash.GetHashCode(LeftTop, Size);
        public override bool Equals(object obj) => obj is Rectangle && Equals((Rectangle)obj);
        public override string ToString() => $"{{{Size} on {Centre}: RT={RightTop}, LB={LeftBottom}}}";
    }
}
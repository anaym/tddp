using System;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization.Geometry
{
    public class Rectangle : IEquatable<Rectangle>
    {
        public readonly Size Size;
        public Vector Centre { get; set; }

        public static Rectangle FromRightTop(Vector rightTop, Size size) => new Rectangle(rightTop, rightTop.Add(size.ToVector()));

        public Rectangle(Vector rightTop, Vector leftBottom) : this(rightTop.ToSize(leftBottom), rightTop)
        {
            RightTop = rightTop;
        }

        public Rectangle(Size size, Vector centre)
        {
            Size = size;
            Centre = centre;
            if (size == null)
                throw new ArgumentNullException(nameof(size));
            if (centre == null)
                throw new ArgumentNullException(nameof(centre));
        }

        public Rectangle(Size size) : this(size, new Vector(0, 0))
        { }

        public Vector RightTop
        {
            get
            {
                return new Vector(Centre.X + Size.Width/2, Centre.Y + Size.Height/2);
            }
            set
            {
                Centre = new Vector(value.X - Size.Width/2, value.Y - Size.Height/2);
            }
        }
        public Vector LeftBottom
        {
            get
            {
                var wa = Size.Width%2;
                var ha = Size.Height%2;
                return new Vector(Centre.X - Size.Width / 2 - wa, Centre.Y - Size.Height / 2 - ha);
            }
            set
            {
                var wa = Size.Width % 2;
                var ha = Size.Height % 2;
                Centre = new Vector(value.X + Size.Width / 2 + wa, value.Y + Size.Height / 2 + ha);
            }
        }

        public bool IsIntersected(Rectangle other, bool includeContur=true)
        {
            return this.IsIntersectedNonCommutative(other, includeContur) || other.IsIntersectedNonCommutative(this, includeContur);
        }

        private bool IsIntersectedNonCommutative(Rectangle other, bool includeContur = true)
        {
            var xIntersected = other.RightTop.X.IsInRange(RightTop.X, LeftBottom.X, includeContur) || other.LeftBottom.X.IsInRange(RightTop.X, LeftBottom.X, includeContur);
            var yIntersected = other.RightTop.Y.IsInRange(RightTop.Y, LeftBottom.Y, includeContur) || other.LeftBottom.Y.IsInRange(RightTop.Y, LeftBottom.Y, includeContur);
            return xIntersected && yIntersected;
        }

        public bool Equals(Rectangle other) => other != null && Size.Equals(other.Size) && Centre.Equals(other.Centre);
        public override int GetHashCode() => Size.GetHashCode();
        public override bool Equals(object obj) => Equals(obj as Rectangle);
        public override string ToString() => $"{{{Size} on {Centre}: LT={RightTop}, RB={LeftBottom}}}";
    }
}
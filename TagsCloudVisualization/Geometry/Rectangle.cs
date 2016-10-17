using System;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization.Geometry
{
    public class Rectangle : IEquatable<Rectangle>
    {
        public readonly Size Size;
        public Vector Centre { get; set; }

        public Rectangle(Vector leftTop, Vector rightBottom)
            : this(new Size(Math.Abs(leftTop.X - rightBottom.X), Math.Abs(leftTop.Y - rightBottom.Y)), leftTop)
        {
            LeftTop = leftTop;
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

        public Vector LeftTop
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
        public Vector RightBottom
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
            var xIntersected = other.LeftTop.X.IsInRange(LeftTop.X, RightBottom.X, includeContur) || other.RightBottom.X.IsInRange(LeftTop.X, RightBottom.X, includeContur);
            var yIntersected = other.LeftTop.Y.IsInRange(LeftTop.Y, RightBottom.Y, includeContur) || other.RightBottom.Y.IsInRange(LeftTop.Y, RightBottom.Y, includeContur);
            return xIntersected && yIntersected;
        }

        public bool Equals(Rectangle other) => other != null && Size.Equals(other.Size) && Centre.Equals(other.Centre);
        public override int GetHashCode() => Size.GetHashCode();
        public override bool Equals(object obj) => Equals(obj as Rectangle);
        public override string ToString() => $"{{{Size} on {Centre}: LT={LeftTop}, RB={RightBottom}}}";
    }
}
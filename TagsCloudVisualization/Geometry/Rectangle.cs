using System;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization.Geometry
{
    public class Rectangle
    {
        public readonly Size Size;
        public Vector Centre { get; set; }

        public static Rectangle FromRightTop(Vector rightTop, Size size) => new Rectangle(rightTop, rightTop.Sub(size.ToVector()));
        public static Rectangle FromLeftBottom(Vector leftBottom, Size size) => new Rectangle(leftBottom.Add(size.ToVector()), leftBottom);

        public Rectangle(Vector rightUp, Vector leftBottom) : this(rightUp.ToSize(leftBottom), rightUp)
        {
            RightUp = rightUp;
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

        public Vector RightUp
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
        public Vector RightBottom => new Vector(Right, Down);
        public Vector LeftUp => new Vector(Left, Up);

        public int Up => Centre.Y + Size.Height/2;
        public int Right => Centre.X + Size.Width/2;
        public int Down => Centre.Y - Size.Height/2 - Size.Height%2;
        public int Left => Centre.X - Size.Width/2 - Size.Width%2;

        public bool IsIntersected(Rectangle other, bool includeContur=true)
        {
            var f = new Segment(Left, Right).IsIntersected(new Segment(other.Left, other.Right), includeContur) &&
                new Segment(Down, Up).IsIntersected(new Segment(other.Down, other.Up), includeContur);

            var s = Contains(other.Centre, true) || other.Contains(Centre, true);
            return f || s;

            return this.IsIntersectedNonCommutative(other, includeContur) || other.IsIntersectedNonCommutative(this, includeContur);
        }

        public bool Contains(Vector other, bool include)
        {
            return new Segment(Left, Right).Contains(other.X, include) &&
                   new Segment(Down, Up).Contains(other.Y, include);
        }

        private bool IsIntersectedNonCommutative(Rectangle other, bool includeContur = true)
        {
            var xIntersected = other.Left.IsInRange(Left, Right, includeContur) || other.Right.IsInRange(Left, Right, includeContur);
            var yIntersected = other.Up.IsInRange(Up, Down, includeContur) || other.Down.IsInRange(Up, Down, includeContur);
            return xIntersected && yIntersected;
        }/*

        public bool Equals(Rectangle other) => other != null && Size.Equals(other.Size) && Centre.Equals(other.Centre);
        public override int GetHashCode() => Size.GetHashCode();
        public override bool Equals(object obj) => Equals(obj as Rectangle);*/
        public override string ToString() => $"{{{Size} on {Centre}: RT={RightUp}, LB={LeftBottom}}}";
    }
}
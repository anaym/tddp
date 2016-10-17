using System;

namespace TagsCloudVisualization
{
    public class Rectangle : IEquatable<Rectangle>
    {
        public readonly Size Size;
        public Vector Centre { get; set; }
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

        public bool Equals(Rectangle other) => other != null && Size.Equals(other.Size) && Centre.Equals(other.Centre);
        public override int GetHashCode() => Size.GetHashCode();
        public override bool Equals(object obj) => Equals(obj as Rectangle);
    }
}
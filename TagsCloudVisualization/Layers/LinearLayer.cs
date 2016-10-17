using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Geometry;

namespace TagsCloudVisualization.Layers
{
    public class LinearLayer : ILayer
    {
        public readonly Direction ToMove;
        public readonly Direction ToCentre;
        private int remaingLength;

        public LinearLayer(Vector start, Direction toMove, Direction toCentre, int maxLength)
        {
            if (toCentre.ToVector().ScalarMul(toMove.ToVector()) != 0)
                throw new ArgumentException("Not ortogonal directions");

            this.ToMove = toMove;
            this.ToCentre = toCentre;
            this.remaingLength = maxLength;
            CurrentPosition = start;
        }

        public Vector CurrentPosition { get; private set; }
        public bool IsCompleted => remaingLength > 0;

        public Rectangle Put(Size size)
        {
            var rect = Rectangle.FromRightTop(CurrentPosition, size);
            var delta = ToMove.ToVector().ScalarMul(size.ToVector());
            remaingLength -= delta;
            CurrentPosition = CurrentPosition.Add(ToMove.ToVector().Mul(delta));
            return rect;
        }
    }

    [TestFixture]
    public class LinearLayer_Should
    {
        private Size[] sizesExample;

        [SetUp]
        public void SetUp()
        {
            sizesExample = new[] {new Size(10, 20), new Size(12, 324), new Size(15, 67)};
        }


        [Test]
        public void PuttedRectanglesOnOneLine(Direction)
        {
            var layer = new LinearLayer(Vector.Zero, direction, int.MaxValue);
            var buffer = sizesExample.Select(s => layer.Put(s)).ToList();
            return buffer.Select(extractAlignment).Distinct().Count();
        }

        private int CountOfRectanglesNotOnLine(Direction direction, Func<Rectangle, int> extractAlignment)
        {
            
        }
    }
}

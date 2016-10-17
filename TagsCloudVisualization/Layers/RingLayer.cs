using System;
using NUnit.Framework;
using TagsCloudVisualization.Geometry;

namespace TagsCloudVisualization.Layers
{
    public class RingLayer : ILayer
    {
        public readonly int Capacity;
        public Rectangle OutsideRectangle { get; private set; }
        public int RemainingSpotsCount { get; private set; }
        public Vector CurrentPosition { get; private set; }

        public bool IsCompleted => RemainingSpotsCount == 0;

        public static RingLayer FirstRingLayer(Vector pos) => new RingLayer(1, new Rectangle(Size.Empty));
        public static RingLayer FromPreviewsLayer(RingLayer prev) => new RingLayer(prev.Capacity + 4, prev.OutsideRectangle);

        public RingLayer(int capacity, Rectangle prevOutsideRectangle)
        {
            Capacity = capacity;
            RemainingSpotsCount = capacity;
            OutsideRectangle = prevOutsideRectangle;
            CurrentPosition = prevOutsideRectangle.RightTop;
        }

        public Rectangle Put(Size size)
        {
            if (size == null)
                throw new ArgumentNullException();
            if (IsCompleted)
                throw new Exception("Layer is completed");
            RemainingSpotsCount--;
            var rect = new Rectangle(size, CurrentPosition);
        }
    }

    [TestFixture]
    public class RingLayer_Should
    {
        private RingLayer ringLayer;

        [SetUp]
        public void SetUp()
        {
            ringLayer = RingLayer.FirstRingLayer(Vector.Zero);
        }


        [Test]
        public void ThrowException_WhenPullAfterComplete()
        {
            ringLayer.Put(Size.Empty);
            Assert.Throws<Exception>(() => ringLayer.Put(Size.Empty));
        }

        [Test]
        public void ThrowArgumentNullException_WhenPullNull()
        {
            Assert.Throws<ArgumentNullException>(() => ringLayer.Put(null));
        }
    }
}

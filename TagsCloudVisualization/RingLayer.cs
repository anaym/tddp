using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TagsCloudVisualization.Geometry;

namespace TagsCloudVisualization
{
    public class RingLayer
    {
        public readonly int Capacity;
        public Rectangle OutsideRectangle { get; private set; }
        public int RemainingSpotsCount { get; private set; }
        public Vector CurrentPosition { get; private set; }

        public bool IsCompleted => RemainingSpotsCount == 0;

        public RingLayer(int capacity, Rectangle prevOutsideRectangle)
        {
            Capacity = capacity;
            RemainingSpotsCount = capacity;
            OutsideRectangle = prevOutsideRectangle;
            CurrentPosition = prevOutsideRectangle.LeftTop;
        }

        public Rectangle Deploy(Size size)
        {
            if (size == null)
                throw new ArgumentNullException();
            if (IsCompleted)
                throw new Exception("Layer is completed");
            var rect = new Rectangle(size, CurrentPosition);
        }
    }

    [TestFixture]
    public class RingLayer_Should
    {
        [Test]
        public void DoSomething_WhenSomething()
        {

        }
    }
}

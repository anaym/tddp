using System;
using System.Collections.Generic;
using System.Linq;
using TagsCloudVisualization.Geometry;

namespace TagsCloudVisualization
{
    public class TagCloud
    {
        private readonly ICircularCloudLayouter layouter;
        private readonly Dictionary<Rectangle, string> rectangleToTag;

        public readonly Size MinCharSize;
        public readonly Func<int, int> ValueToHeight;
        public readonly double HeightPerWidth;

        public TagCloud(ICircularCloudLayouter layouter, Size minCharSize, Func<int, int> valueToHeight)
        {
            this.layouter = layouter;
            MinCharSize = minCharSize;
            ValueToHeight = valueToHeight;
            rectangleToTag = new Dictionary<Rectangle, string>();
            if (minCharSize.Width == 0 || minCharSize.Height == 0)
                throw new ArgumentException("Bad min char size: width and height must be non zero");
            if (valueToHeight == null)
                throw new ArgumentNullException(nameof(valueToHeight));
            if (layouter == null)
                throw new ArgumentNullException(nameof(layouter));
            HeightPerWidth = 1.0*minCharSize.Height/minCharSize.Width;
        }

        public void PutNextTag(string tag, int value)
        {
            var rect = layouter.PutNextRectangle(GetSize(tag, value));
            rectangleToTag.Add(rect, tag);
        }

        public void PutManyTags(IReadOnlyDictionary<string, int> tags)
        {
            foreach (var source in tags.OrderByDescending(t => t.Value))
            {
                PutNextTag(source.Key, source.Value);
            }
        }

        public IEnumerable<Rectangle> Rectangles => layouter.Rectangles;
        public IEnumerable<KeyValuePair<Rectangle, string>> Tags => rectangleToTag;
        public Rectangle TagsTangentialRectangle => Tags.Select(t => t.Key).TangentialRectangle();
        public Rectangle LayoutTangentialRectangle => Rectangles.TangentialRectangle();

        private Size GetSize(string tag, int value)
        {
            var height = ValueToHeight(value) + MinCharSize.Height;
            var width = height*tag.Length/HeightPerWidth + MinCharSize.Width;
            return new Size((int) width, height);
        }
    }
}
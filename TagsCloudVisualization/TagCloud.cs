using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Geometry;
using TagsCloudVisualization.Geometry.Extensions;
using Rectangle = TagsCloudVisualization.Geometry.Rectangle;
using Size = TagsCloudVisualization.Geometry.Size;

namespace TagsCloudVisualization
{
    public class TagCloud
    {
        private readonly ICircularCloudLayouter layouter;
        private readonly Dictionary<Rectangle, string> rectangleToTag;
        private readonly Func<KeyValuePair<string, int>, Size> valueToSize;
        private readonly int maxValue;
        private readonly int minValue;

        // CR (krait): Почему AsMapping? Я не понял.
        public static TagCloud AsMapping(ICircularCloudLayouter layouter, int minHeight, int maxHeight, int maxValue, int minValue)
        {
            // CR (krait): А если minValue > maxValue?
            if (maxValue == minValue)
                throw new ArgumentException($"{nameof(maxValue)} can`t be equal to {nameof(minValue)}");
            Func<int, int> valToHeight = v => (int) (1.0 * (v - minValue) / (maxValue - minValue) * (maxHeight - minHeight) + minHeight);
            var drawer = Graphics.FromImage(new Bitmap(1, 1));
            return new TagCloud(layouter, pair => drawer.MeasureString(pair.Key, new Font(FontFamily.GenericMonospace, valToHeight(pair.Value))).ToGeometrySize());
        }

        public TagCloud(ICircularCloudLayouter layouter, Func<KeyValuePair<String, int>, Size> valueToSize) : this(layouter, valueToSize, int.MaxValue, 0)
        { }

        protected TagCloud(ICircularCloudLayouter layouter, Func<KeyValuePair<String, int>, Size> valueToSize, int maxValue, int minValue)
        {
            if (maxValue < 0)
                throw new ArgumentException(nameof(maxValue));
            if (minValue < 0)
                throw new ArgumentException(nameof(minValue));
            this.maxValue = maxValue;
            this.minValue = minValue;
            this.layouter = layouter;
            this.valueToSize = valueToSize;
            if (valueToSize == null)
                throw new ArgumentNullException(nameof(valueToSize));
            if (layouter == null)
                throw new ArgumentNullException(nameof(layouter));
            rectangleToTag = new Dictionary<Rectangle, string>();
        }

        // CR (krait): Жесть название, чёрт ногу сломит.
        private static Size Value2HeightToValue2Size(int value, string tag, Func<int, int> valueToHeight, Size minCharSize)
        {
            var height = valueToHeight(value) + minCharSize.Height;
            var width = 1.0 * height * tag.Length / minCharSize.Height * minCharSize.Width + minCharSize.Width;
            return new Size((int)width, height);
        }

        public TagCloud(ICircularCloudLayouter layouter, Size minCharSize, Func<int, int> valueToHeight) 
            : this(layouter, pair => Value2HeightToValue2Size(pair.Value, pair.Key, valueToHeight, minCharSize))
        {
            this.layouter = layouter;
            if (minCharSize.Width == 0 || minCharSize.Height == 0)
                throw new ArgumentException("Bad min char size: width and height must be non zero");
            if (valueToHeight == null)
                throw new ArgumentNullException(nameof(valueToHeight));
        }

        public void PutNextTag(string tag, int value)
        {
            if (value > maxValue || value < minValue)
                throw new ArgumentException($"{nameof(value)} out of range [{nameof(minValue)}, {nameof(maxValue)}]");
            var rect = layouter.PutNextRectangle(valueToSize(new KeyValuePair<string, int>(tag, value)));
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
        public Rectangle TagsCoveringRectangle => Tags.Select(t => t.Key).CoveringRectangle();
        public Rectangle LayoutCoveringRectangle => Rectangles.CoveringRectangle();
    }
}
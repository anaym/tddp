using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Geometry;
using Rectangle = TagsCloudVisualization.Geometry.Rectangle;
using Size = TagsCloudVisualization.Geometry.Size;

namespace TagsCloudVisualization
{
    public class TagCloudToBitmapConverter
    {
        private readonly CircularCloudLayouter layouter;
        private readonly Dictionary<Rectangle, string> rectangleToTag;

        public Brush TextBrush;
        public bool ShowRectangles;

        private StringFormat stringFormat;

        public TagCloudToBitmapConverter(IReadOnlyDictionary<string, int> data, Size minCharSize, Size maxCharSize)
        {
            layouter = new CircularCloudLayouter();
            rectangleToTag = new Dictionary<Rectangle, string>();

            TextBrush = new SolidBrush(Color.Red);
            ShowRectangles = false;
            stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center
            };

            FillData(data, minCharSize, maxCharSize);
        }

        private void FillData(IReadOnlyDictionary<string, int> data, Size minCharSize, Size maxCharSize)
        {
            var delta = data.Max(p => p.Value) - data.Min(p => p.Value);
            var sizeDelta = maxCharSize.ToVector().Sub(minCharSize.ToVector());
            var xStep = sizeDelta.X * 1.0 / delta;
            var yStep = sizeDelta.Y * 1.0 / delta;
            foreach (var pair in data.OrderByDescending(p => p.Value))
            {
                var size = new Size(((int)(xStep * pair.Value * 0.5) + minCharSize.Width) * pair.Key.Length, (int)(yStep * pair.Value) + minCharSize.Height);
                var rect = layouter.PutNextRectangle(size);
                rectangleToTag.Add(rect, pair.Key);
            }
        }

        public Bitmap ToBitmap()
        {
            var tangentialRectangle = layouter.GetRectangles().TangentialRectangle();
            var bitmap = new Bitmap(tangentialRectangle.Size.Width, tangentialRectangle.Size.Height);
            var graphics = Graphics.FromImage(bitmap);
            foreach (var rectangle in layouter.GetRectangles())
            {
                var rectF = rectangle.ToRectangleF(tangentialRectangle );
                if (ShowRectangles) graphics.FillRectangle(new SolidBrush(rectangle.ToColor()), rectF);
                var tag = rectangleToTag[rectangle];

                graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                var goodFont = FindFont(graphics, tag, rectF.Size, new Font(FontFamily.GenericMonospace, 128));
                graphics.DrawString(tag, goodFont, TextBrush, rectF, stringFormat);
            }
            return bitmap;
        }

        private static Font FindFont(Graphics g, string str, SizeF room, Font preferedFont)
        {
            SizeF realSize = g.MeasureString(str, preferedFont);
            float heightScaleRatio = room.Height / realSize.Height;
            float widthScaleRatio = room.Width / realSize.Width;
            float scaleRatio = (heightScaleRatio < widthScaleRatio) ? heightScaleRatio : widthScaleRatio;
            float scaleFontSize = preferedFont.Size * scaleRatio;
            return new Font(preferedFont.FontFamily, scaleFontSize);
        }
    }
}

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using TagsCloudVisualization.Geometry;
using Rectangle = TagsCloudVisualization.Geometry.Rectangle;
using Size = TagsCloudVisualization.Geometry.Size;

namespace TagsCloudVisualization
{
    public class CloudRenderer
    {
        private readonly ICircularCloudLayouter layouter;
        private readonly IDictionary<Rectangle, string> rectangleToTag;

        public Brush TextBrush;
        public bool ShowRectangles;
        public StringFormat StringFormat;
        public Size MinCharSize;
        public Size MaxCharSize;

        public CloudRenderer(ICircularCloudLayouter layouter)
        {
            this.layouter = layouter;
            rectangleToTag = new Dictionary<Rectangle, string>();

            TextBrush = new SolidBrush(Color.Red);
            ShowRectangles = false;
            StringFormat = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center
            };
            MinCharSize = new Size(16, (int)(16*1.35));
            MaxCharSize = new Size(64, (int)(64*1.35));
        }

        public void PutTags(IReadOnlyDictionary<string, int> tags)
        {
            var delta = tags.Max(p => p.Value) - tags.Min(p => p.Value);
            var sizeDelta = MaxCharSize.ToVector().Sub(MinCharSize.ToVector());
            var xStep = sizeDelta.X * 1.0 / delta;
            var yStep = sizeDelta.Y * 1.0 / delta;
            foreach (var pair in tags.OrderByDescending(p => p.Value))
            {
                var size = new Size(((int) (xStep*pair.Value*0.5) + MinCharSize.Width)*pair.Key.Length,
                                     (int) (yStep*pair.Value) + MinCharSize.Height);
                var rect = layouter.PutNextRectangle(size);
                rectangleToTag.Add(rect, pair.Key);
            }
        }

        public Rectangle VisualizeRectangle => layouter.GetRectangles().TangentialRectangle();

        public void Render(Graphics graphics)
        {
            var tangentialRectangle = VisualizeRectangle;
            foreach (var rectangle in layouter.GetRectangles())
            {
                var rectF = rectangle.ToRectangleF(tangentialRectangle);
                if (ShowRectangles || !rectangleToTag.ContainsKey(rectangle))
                    graphics.FillRectangle(new SolidBrush(rectangle.ToColor()), rectF);
                if (rectangleToTag.ContainsKey(rectangle))
                {
                    var tag = rectangleToTag[rectangle];

                    graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                    var goodFont = FindFont(graphics, tag, rectF.Size, new Font(FontFamily.GenericMonospace, 128));
                    graphics.DrawString(tag, goodFont, TextBrush, rectF, StringFormat);
                }
            }
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
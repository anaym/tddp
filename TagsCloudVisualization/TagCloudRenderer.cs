using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using TagsCloudVisualization.Geometry;
using TagsCloudVisualization.Geometry.Extensions;

namespace TagsCloudVisualization
{
    public class TagCloudRenderer
    {
        private readonly List<Brush> textBrushes;
        private readonly bool showRectangles;
        private readonly StringFormat stringFormat;

        public void AddColor(Color color) => textBrushes.Add(new SolidBrush(color));
        public void AddManyColor(params Color[] colors) => textBrushes.AddRange(colors.Select(c => new SolidBrush(c)));

        public TagCloudRenderer(bool showRectangles = false)
        {
            textBrushes = new List<Brush> {new SolidBrush(Color.DarkRed)};
            this.showRectangles = showRectangles;
            stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center
            };
        }

        public Bitmap RenderToBitmap(TagCloud tags)
        {
            var size = (showRectangles ? tags.LayoutCoveringRectangle : tags.TagsCoveringRectangle).Size;
            var bitmap = new Bitmap(size.Width, size.Height);
            Render(Graphics.FromImage(bitmap), tags);
            return bitmap;
        }

        public void Render(Graphics graphics, TagCloud tagCloud)
        {
            var transform = new VectorCoordinateSystemConverter(showRectangles ? tagCloud.LayoutCoveringRectangle : tagCloud.TagsCoveringRectangle);
            if (showRectangles)
            {
                foreach (var rectangle in tagCloud.Rectangles)
                {
                    var rectF = transform.Transform(rectangle);
                    graphics.FillRectangle(new SolidBrush(rectangle.Size.ToColor()), rectF);
                    graphics.DrawRectangle(new Pen(Color.GreenYellow), rectF.X, rectF.Y, rectF.Width, rectF.Height);

                }
            }
            var rnd = new Random();
            foreach (var tag in tagCloud.Tags)
            {
                var rectF = transform.Transform(tag.Key);
                graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                var goodFont = FindFont(graphics, tag.Value, rectF.Size, new Font(FontFamily.GenericMonospace, 128));
                var textBrush = textBrushes[rnd.Next(textBrushes.Count)];
                graphics.DrawString(tag.Value, goodFont, textBrush, rectF, stringFormat);
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
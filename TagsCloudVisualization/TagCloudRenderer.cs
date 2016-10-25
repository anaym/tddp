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
        // !CR (krait): Изменяемые публичные поля выглядят неаккуратно. Лучше принять всё это в конструкторе.

        public readonly Brush TextBrush;
        public readonly bool ShowRectangles;
        public readonly StringFormat StringFormat;

        public TagCloudRenderer(bool showRectangles = false, Brush textBrush = null)
        {
            TextBrush = textBrush ?? new SolidBrush(Color.Red);
            ShowRectangles = showRectangles;
            StringFormat = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center
            };
        }

        public Bitmap RenderToBitmap(TagCloud tags)
        {
            var size = (ShowRectangles ? tags.LayoutTangentialRectangle : tags.TagsTangentialRectangle).Size;
            var bitmap = new Bitmap(size.Width, size.Height);
            Render(Graphics.FromImage(bitmap), tags);
            return bitmap;
        }

        public void Render(Graphics graphics, TagCloud tagCloud)
        {
            var transform = new VectorTransform(ShowRectangles ? tagCloud.LayoutTangentialRectangle : tagCloud.TagsTangentialRectangle);
            if (ShowRectangles)
            {
                foreach (var rectangle in tagCloud.Rectangles)
                {
                    var rectF = transform.Transform(rectangle);
                    graphics.FillRectangle(new SolidBrush(rectangle.Size.ToColor()), rectF);
                    graphics.DrawRectangle(new Pen(Color.GreenYellow), rectF.X, rectF.Y, rectF.Width, rectF.Height);

                }
            }
            foreach (var tag in tagCloud.Tags)
            {
                var rectF = transform.Transform(tag.Key);
                graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                var goodFont = FindFont(graphics, tag.Value, rectF.Size, new Font(FontFamily.GenericMonospace, 128));
                graphics.DrawString(tag.Value, goodFont, TextBrush, rectF, StringFormat);
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
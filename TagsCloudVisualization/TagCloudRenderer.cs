using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using TagsCloudVisualization.Geometry;

namespace TagsCloudVisualization
{
    public class TagCloudRenderer
    {
        // CR (krait): Изменяемые публичные поля выглядят неаккуратно. Лучше принять всё это в конструкторе.

        public Brush TextBrush;
        public bool ShowRectangles;
        public StringFormat StringFormat;

        public TagCloudRenderer()
        {
            TextBrush = new SolidBrush(Color.Red);
            ShowRectangles = false;
            StringFormat = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center
            };
        }

        public Bitmap RenderToBitmap(TagCloud tags)
        {
            var size = tags.TangentialRectangle.Size;
            var bitmap = new Bitmap(size.Width, size.Height);
            Render(Graphics.FromImage(bitmap), tags);
            return bitmap;
        }

        public void Render(Graphics graphics, TagCloud tagCloud)
        {
            var tangentialRectangle = tagCloud.TangentialRectangle;
            foreach (var tag in tagCloud.Tags)
            {
                var rectF = tag.Key.ToRectangleF(tangentialRectangle);
                if (ShowRectangles)
                {
                    graphics.FillRectangle(new SolidBrush(tag.Key.ToColor()), rectF);
                    graphics.DrawRectangle(new Pen(Color.GreenYellow), rectF.X, rectF.Y, rectF.Width, rectF.Height);
                }
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
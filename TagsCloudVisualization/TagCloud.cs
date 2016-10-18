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
using TagsCloudVisualization.CircularLayouter;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Geometry;
using Rectangle = TagsCloudVisualization.Geometry.Rectangle;
using Size = TagsCloudVisualization.Geometry.Size;

namespace TagsCloudVisualization
{
    public class TagCloud
    {
        private readonly double heightPerWidth;
        private CircularCloudLayouter layouter;
        private Dictionary<Rectangle, string> rectangleToTag;

        public TagCloud(IReadOnlyDictionary<string, int> data, Size minCharSize, Size maxCharSize)
        {
            layouter = new CircularCloudLayouter();
            rectangleToTag = new Dictionary<Rectangle, string>();

            var delta = data.Max(p => p.Value) - data.Min(p => p.Value);
            var sizeDelta = maxCharSize.ToVector().Sub(minCharSize.ToVector());
            var xStep = sizeDelta.X*1.0/delta;
            var yStep = sizeDelta.Y*1.0/delta;
            foreach (var pair in data.OrderByDescending(p => p.Value))
            {
                var size = new Size(((int)(xStep * pair.Value) + minCharSize.Width) * pair.Key.Length, (int)(yStep * pair.Value) + minCharSize.Height);
                var rect = layouter.PutNextRectangle(size);
                rectangleToTag.Add(rect, pair.Key);
            }
        }

        public Bitmap ToBitmap(bool fillRectangles = false)
        {
            var outer = layouter.GetRectangles().OutRectangle();
            Console.WriteLine(outer);
            var bitmap = new Bitmap(outer.Size.Width, outer.Size.Height);
            var graphics = Graphics.FromImage(bitmap);
            var brush = new SolidBrush(Color.Red);
            var check = new List<Rectangle>();
            var n = 0;

            var a = layouter.rectangles[0];
            var b = layouter.rectangles[1];
            var i = a.IsIntersected(b, false);

            foreach (var rectangle in layouter.GetRectangles())
            {
                
                var rectF = rectangle.ToRectangleF(outer);
                n++;
                Console.WriteLine(rectangle + ">>>" + rectF);
                if (fillRectangles) graphics.FillRectangle(new SolidBrush(rectangle.ToColor()), rectF);
                var tag = rectangleToTag[rectangle];
                check.Add(rectF.ToRectangle());
                try
                {
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Near;
                    stringFormat.LineAlignment = StringAlignment.Center;

                    graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                    var goodFont = FindFont(graphics, tag, rectF.Size, new Font(FontFamily.GenericMonospace,16));
                    graphics.DrawString(tag, goodFont, brush, rectF, stringFormat);

                }
                catch (Exception)
                {
                    Console.WriteLine("DRAW ERROR");
                }
            }
            graphics.DrawLine(new Pen(Color.AliceBlue), 0, outer.Size.Height/2, outer.Size.Width, outer.Size.Height / 2);
            graphics.DrawLine(new Pen(Color.AliceBlue), outer.Size.Width/2, 0, outer.Size.Width/2, outer.Size.Height);
            Console.WriteLine(layouter.GetRectangles().Count(r => layouter.GetRectangles().Any(o => o.IsIntersected(r, false))));
            var bad = check.Where(r => check.Any(o => o.IsIntersected(r, false))).ToList();
            foreach (var rectangle in bad)
            {
                Console.WriteLine(rectangle);
            }
            return bitmap;
        }

        private Font FindFont(Graphics g, string longString, SizeF room, Font preferedFont)
        {
            //you should perform some scale functions!!!
            SizeF RealSize = g.MeasureString(longString, preferedFont);
            float HeightScaleRatio = room.Height / RealSize.Height;
            float WidthScaleRatio = room.Width / RealSize.Width;
            float ScaleRatio = (HeightScaleRatio < WidthScaleRatio) ? ScaleRatio = HeightScaleRatio : ScaleRatio = WidthScaleRatio;
            float ScaleFontSize = preferedFont.Size * ScaleRatio;
            return new Font(preferedFont.FontFamily, ScaleFontSize);
        }
    }

    public static class RectangleCastExtension
    {
        public static Rectangle ToRectangle(this RectangleF rect)
        {
            return new Rectangle(new Vector((int)rect.Left, (int)rect.Bottom), new Vector((int)rect.Right, (int)rect.Top));
        }

        public static RectangleF ToRectangleF(this Rectangle rectangle, Rectangle outter)
        {
            var a = rectangle.LeftUp.ToPointF(outter);
            var b = rectangle.RightBottom.ToPointF(outter);
            return new RectangleF(a.X, a.Y, rectangle.Size.Width, rectangle.Size.Height);
        }

        public static SizeF ToSizeF(this Size size) => new SizeF(size.Width, size.Height);
        public static PointF ToPointF(this Vector vector, Rectangle outter)
        {
            var tr = new TransformCoordinates(outter).Transform(vector);
            return new PointF(tr.X, tr.Y);
        }

        public static Color ToColor(this object obj) => Color.FromArgb(50, Color.FromArgb(obj.GetHashCode()));
    }

    public class TransformCoordinates
    {
        private readonly Rectangle inRectangle;

        public TransformCoordinates(Rectangle inRectangle)
        {
            this.inRectangle = inRectangle;
        }

        public Vector Transform(Vector inVector)
        {
            var x = inVector.X - inRectangle.Left;
            var y = inVector.Y - inRectangle.Down;
            var yi = -y;
            var yr = inRectangle.Size.Height + yi;
            return new Vector(x, yr);
        }
    }
}

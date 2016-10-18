using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization.Geometry
{
    public class Segment
    {
        public readonly int Left;
        public readonly int Right;

        public Segment(int left, int right)
        {
            this.Left = left;
            this.Right = right;
        }

        public bool Contains(int n, bool include)
        {
            return n.IsInRange(Left, Right, include);
        }

        public bool IsIntersected(Segment other, bool include)
        {
            var a = Contains(other.Left, include) || Contains(other.Right, include);
            var b = other.Contains(Left, include) || other.Contains(Right, include);
            return a || b;
        }
    }
}

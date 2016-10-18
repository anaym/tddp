using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization.CircularLayouter
{
    public class Bucket : IEnumerable<Vector>
    {
        private readonly Func<Vector, bool> detector;
        private Queue<Vector> data;

        public bool CanAdd(Vector vector)
        {
            return detector(vector);
        }

        public bool Add(Vector vector)
        {
            if (!CanAdd(vector)) return false;
            data.Enqueue(vector);
            return true;
        }

        public Bucket(Func<Vector, bool> detector)
        {
            this.detector = detector;
            data = new Queue<Vector>();
        }

        public IEnumerator<Vector> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

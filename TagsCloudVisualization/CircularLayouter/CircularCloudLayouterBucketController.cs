using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization.CircularLayouter
{
    class CircularCloudLayouterBucketController
    {
        private readonly Vector centre;
        private readonly List<Bucket> buckets;
        private int bp;

        public IEnumerable<Vector> Data => buckets[bp];

        public CircularCloudLayouterBucketController(Vector centre)
        {
            this.centre = centre;
            buckets = new List<Bucket>();
            bp = 0;

            AddBucket(new Bucket(p => p.X >= 0 && p.Y >= 0));
            AddBucket(new Bucket(p => p.X <=0 && p.Y >= 0));
            AddBucket(new Bucket(p => p.X <= 0 && p.Y <= 0));
            AddBucket(new Bucket(p => p.X >= 0 && p.Y <= 0));
        }

        public void Add(Vector point)
        {
            foreach (var bucket in buckets)
            {
                if (bucket.Add(point))
                    return;
            }
            throw new Exception("Bucket is not founded");
        }

        public void AddMany(params Vector[] points)
        {
            foreach (var vector in points)
            {
                Add(vector);
            }
        }

        protected void AddBucket(Bucket bucket)
        {
            buckets.Add(bucket);
        }

        public void Swap()
        {
            bp = (bp + 1)%buckets.Count;
        }
    }
}

using System.Collections.Generic;

namespace ImageUtilities 
{
    public class Cluster
    {
        public Vector centroid;

        public List<Vector> vectors;

        public Cluster(Vector _centroid)
        {
            centroid = _centroid;
            vectors = new List<Vector>();
            vectors.Add(centroid);
        }

        public void Add(Vector vec)
        {
            vectors.Add(vec);
        }

        public int Count()
        {
            return vectors.Count;
        }

        public void ReplaceCentroid(Vector newCentroid)
        {
            // remove the current centroid from the vectors
            vectors.RemoveAll(v => v == centroid);
            vectors.Add(newCentroid);

            centroid = newCentroid;   
        }

        public bool IsVectorNearCentroid(Vector vec, double lastClosestDistance)
        {
            var distance = vec.Distance(centroid);

            if ((distance * 2) < lastClosestDistance)
            {
                return true;
            }

            return false;
        }
    }
}
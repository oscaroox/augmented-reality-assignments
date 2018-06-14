namespace ImageUtilities 
{
    public class Cluster
    {
        private Vector centroid;

        private List<Vector> vectors;

        public Cluster(Vector _centroid, List<Vector> _vectors)
        {
            centroid = _centroid;
            vectors = _vectors;
        }
    }
}
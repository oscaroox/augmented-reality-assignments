using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageUtilities
{

    public class KMeans
    {
        private int numClusters;

        private Random rand = new Random();

        private Vector[,] vectors;

        private int vWidth = 0;
        private int vHeight = 0;
        private int maxIterations = 0;

        private List<Vector> centroids;
        private List<List<Vector>> clusters;

        public KMeans(Vector[,] _vectors, int _numClusters, int _maxIterations)
        {
            vectors = _vectors;
            numClusters = _numClusters;
            maxIterations = _maxIterations;

            centroids = new List<Vector>();
            clusters = new List<List<Vector>>();

            vWidth = vectors.GetLength(0);
            vHeight = vectors.GetLength(1);
         
        }

        public Color[,] Start()
        {
     
            generateCentroids();


            iterate((x,y) =>
            {
                var curr = vectors[x, y];

                var calcDistance = Double.PositiveInfinity;
                Vector closestCentroid = centroids[0];

                // loop through each centroid and check 
                // if this vector is closest to a centroid
                foreach (var centroid in centroids)
                {
                    var distance = curr.Distance(centroid);

                    if ((distance * 2) < calcDistance)
                    {
                        calcDistance = (distance * 2);
                        closestCentroid = centroid;
                    }
             
                }

                // get the cluster closes to the centroid
                List<Vector> cluster = clusters.Select(v => v)
                    .First(c => c.Any(v => v == closestCentroid));

                cluster.Add(curr);
                int count = cluster.Count();
                Vector currentCentroid = cluster[0];
                var newCentroid = new Vector(x, y, 0, 0, 0);

                foreach(var vector in cluster)
                {
                    newCentroid = newCentroid.Sum(vector);
                }

                newCentroid = new Vector(x, y, newCentroid.R / count, newCentroid.G / count, newCentroid.B / count);

                cluster.RemoveAt(0);
                cluster.Insert(0, newCentroid);

                for (int i = 0; i < centroids.Count(); i++)
                {
                    if (centroids[i] == currentCentroid)
                    {
                        centroids[i] = newCentroid;
                        break;
                    }
                }

            });

            foreach (List<Vector> cluster in clusters)
            {
                Vector centroid = cluster[0];
                for (int i = 1; i < cluster.Count(); i++)
                {
                    cluster[i].R = centroid.R;
                    cluster[i].G = centroid.G;
                    cluster[i].B = centroid.B;
                }
            }

            Color[,] output = new Color[vWidth, vHeight];
            foreach (List<Vector> cluster in clusters)
            {
                for (int i = 0; i < cluster.Count(); i++)
                {
                    Vector vector = cluster[i];
                    output[vector.x, vector.y] = Color.FromArgb((int)vector.R, (int)vector.G, (int)vector.B);
                }
            }

            return output;

        }

        private void generateCentroids()
        {
            for(var i = 0; i < numClusters; i++)
            {
                // get a random starting point for the centroid
                var centroid = vectors[rand.Next(0, vWidth), rand.Next(0, vHeight)];

                var cluster = new List<Vector>();

                // add the centroid to the cluster
                cluster.Add(centroid);

                // keep track of the cluster
                clusters.Add(cluster);

                // finally keep track of the centroid
                centroids.Add(centroid);
            }
        }

        private void iterate(Action<int,int> iter)
        {
            for (var i = 0; i < maxIterations; i++)
            {
                for (var x = 0; x < vWidth; x++)

                {
                    for (var y =0; y < vHeight; y++)
                    {
                        iter(x, y);
                    }
                }
            }
        }


    }
}

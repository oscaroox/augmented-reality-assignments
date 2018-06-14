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

        private List<Cluster> clusters;

        public KMeans(Vector[,] _vectors, int _numClusters, int _maxIterations)
        {
            vectors = _vectors;
            numClusters = _numClusters;
            maxIterations = _maxIterations;

            clusters = new List<Cluster>();

            vWidth = vectors.GetLength(0);
            vHeight = vectors.GetLength(1);
         
        }

        public Color[,] Start()
        {
     
            generateCentroids();


            iterate((x,y) =>
            {
                var currentVector = vectors[x, y];

                var lastNearestDistance = Double.PositiveInfinity;

                Cluster nearestCluster = clusters.First();

                foreach (var cluster in clusters)
                {
                    if(cluster.IsVectorNearCentroid(currentVector, lastNearestDistance))
                    {
                        lastNearestDistance = (lastNearestDistance * 2);
                        nearestCluster = cluster; 
                    }
                }
                
                // add vector to nearest cluster
                nearestCluster.Add(currentVector);

                var count = nearestCluster.Count();

                // create a new vector to replace the old centroid position
                var newCentroid = new Vector(x, y, 0, 0, 0);
                 
                foreach(var vector in nearestCluster.vectors)
                {
                    newCentroid.Sum(vector);
                }
                
                // calculate new position of the centroid
                newCentroid.Product(count);
                
                
                nearestCluster.ReplaceCentroid(newCentroid);

            });

            Color[,] output = new Color[vWidth, vHeight];

            foreach (Cluster cluster in clusters)
            {
                var centroid = cluster.centroid;
                var vectors = cluster.vectors;

                foreach (Vector vector in vectors)
                {
                    vector.R = centroid.R;
                    vector.G = centroid.G;
                    vector.B = centroid.B;

                    output[vector.x, vector.y] = vector.FromRGB();
                }

            }

            return output;
        }

        private void generateCentroids()
        {
            for(var i = 0; i < numClusters; i++)
            {
                // get a random starting point for the centroid
                // and add to new cluster
                var cluster = new Cluster(vectors[rand.Next(0, vWidth), rand.Next(0, vHeight)]);

                clusters.Add(cluster);
            }
        }

        private void iterate(Action<int,int> iter)
        {
            for (var i = 0; i < maxIterations; i++)
            {
                for (var x = 0; x < vWidth; x++)

                {
                    for (var y = 0; y < vHeight; y++)
                    {
                        iter(x, y);
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace ImageUtilities
{
    
    public class Vector
    {
        public double R, G, B;
        public int x, y;

        public Vector(int _x, int _y, double _r, double _g, double _b)
        {
            x = _x;
            y = _y;
            R = _r;
            G = _g;
            B = _b;
        }

        public void Sum(Vector other)
        {
            R = R + other.R;
            G = G + other.G;
            B = B + other.B;
        }

        public double Length()
        {
            return Math.Sqrt((R * 2) + (G * 2) + (B * 2));
        }

        public void Product(int sc)
        {
            R = R / sc;
            G = G / sc;
            B = B / sc;
        }

        public Color FromRGB()
        {
            return Color.FromArgb((int)R, (int)G, (int)B);
        }

        public double Distance(Vector other)
        {
            var rp = (R - other.R) * 2;
            var gp = (G - other.G) * 2;
            var bp = (B - other.B) * 2;

            return Math.Sqrt(rp + gp + bp);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Vector vec = (Vector)obj;

            return (R == vec.R) && (G == vec.G) && (B == vec.B);
        }


        public static Vector[,] FromColor(Color[,] im)
        {
            var width = im.GetLength(0);
            var height = im.GetLength(1);

            var res = new Vector[width, height];
      
            for (var x = 0; x < im.GetLength(0); x++)
            {
                for (var y = 0; y < im.GetLength(1); y++)
                {
                    var color = im[x, y];
                    res[x, y] = new Vector(x, y, color.R, color.G, color.B);
                }
            }

            return res;
        }
    }
}

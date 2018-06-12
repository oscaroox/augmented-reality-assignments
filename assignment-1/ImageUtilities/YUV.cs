using System;

namespace ImageUtilities
{
    public class YUV
    {
        public int y { get; set; }
        public int u { get; set; }
        public int v { get; set; }

        public YUV(int _y, int _u, int _v)
        {
            y = _y;
            u = _u;
            v = _v;
        }

        private int Clamp(int x)
        {
            if (x > 255)
            {
                return 255;
            }
            else if (x < 0)
            {
                return 0;
            }
            else
            {
                return x;
            }
        }

        public void ToGreyScale()
        {
            u = 0;
            v = 0;
        }

        public RGB ToRGB()
        {
            var fy = Convert.ToDouble(y);
            var fu = Convert.ToDouble(u);
            var fv = Convert.ToDouble(v);

            var r = Convert.ToInt32(fy + 1.140 * fv);

            var g = Convert.ToInt32(fy - .395 * fu - .581 * fv);

            var b = Convert.ToInt32(fy + 2.032 * fu);

            return new RGB(Clamp(r), Clamp(g), Clamp(b));
        }
    }
}
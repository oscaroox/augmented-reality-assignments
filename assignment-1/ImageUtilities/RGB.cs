using System;

namespace ImageUtilities
{
    public class RGB
    {
        public int r { get; set; }
        public int g { get; set; }
        public int b { get; set; }

        public RGB(int _r, int _g, int _b)
        {
            r = _r;
            g = _g;
            b = _b;
        }

        public void ToGreyScale()
        {
            var avg = (r + g + b) / 3;

            r = avg;
            g = avg;
            b = avg;
        }

        public YUV ToYUV()
        {
            var fr = Convert.ToDouble(r);
            var fg = Convert.ToDouble(g);
            var fb = Convert.ToDouble(b);

            var y = Convert.ToInt32(.299 * fr + .587 * fg + .114 * fb);

            var u = Convert.ToInt32(-.147 * fr - .289 * fg + .436 * fb);

            var v = Convert.ToInt32(.615 * fr - .515 * fg - .100 * fb);

            return new YUV(y, u, v);
        }
    }
}
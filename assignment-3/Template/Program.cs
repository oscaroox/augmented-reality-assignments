using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ImageUtilities;


namespace Template
{
  class Program
  {
    static void Main(string[] args)
    {
      //string path = args[0]; // right click template -> properties -> Debug -> command line arguments -> "../../../images/tulips.png"
      string path = @"../../../images/baboon.png";
      Color[,] im = ImageViewer.LoadImage(path);

      var mv = Vector.FromColor(im);

      var km = new KMeans(mv, 400, 3);
      var output = km.Start();

      ImageViewer.DrawImage(output);
      ImageViewer.DrawImagePair(im, output);
    }
  }
}

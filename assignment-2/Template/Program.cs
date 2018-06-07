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
      string path = @"../../../images/boy.bmp";
      string forest = @"../../../images/forest.jpg";

      Color[,] im1 = ImageViewer.LoadImage(path);
      Color[,] im2 = ImageViewer.LoadImage(forest);

      var nResize = ImageViewer.NearestNeighbourInterpolation(im1, 1.5);
      var bResize = ImageViewer.BilinearInterpolation(im2, 1.5);

      // first draw nearest neighbour interpolation
      ImageViewer.DrawImage(nResize);
      ImageViewer.DrawImagePair(im1, nResize);

      // then draw nearest neighbour interpolation
      ImageViewer.DrawImage(bResize);
      ImageViewer.DrawImagePair(im2, bResize);
      
    }
  }
}

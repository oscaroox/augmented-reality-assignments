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
      Color[,] im = ImageViewer.LoadImage(path);

      var resize = ImageViewer.bilinearInterpolation(im, 1.2);

      ImageViewer.DrawImage(resize);
      ImageViewer.DrawImagePair(im, resize);
    }
  }
}

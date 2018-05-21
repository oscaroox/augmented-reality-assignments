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
      string path = @"../../../images/forest.jpg";
      Color[,] im = ImageViewer.LoadImage(path);
        
      // First display rgb to yuv conversion
      var imgFromRGBToYUVAndBack = ImageViewer.FromRGB2YUVAndBack(im);
      var imgWithGreyScale = ImageViewer.FromRGB2YUVWithGreyScale(im);

      ImageViewer.DrawImagePair(im, imgFromRGBToYUVAndBack);

      // Then displayblack and white filter with rgb and yuv
      ImageViewer.DrawImagePair(im, imgWithGreyScale);

    }
  }
}

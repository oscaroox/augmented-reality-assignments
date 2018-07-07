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
      string path = @"../../../images/omni1.jpg";
      Color[,] im = ImageViewer.LoadImage(path);
      ImageViewer.DrawImage(im);

      var newim = ImageViewer.unwarp(im, 330, 240, 16, 236);
       
      ImageViewer.DrawImage(newim);
    }
  }
}

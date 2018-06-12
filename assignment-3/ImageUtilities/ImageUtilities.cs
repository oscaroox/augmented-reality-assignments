using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace ImageUtilities
{
    public class ImageViewer
    {

    /// <summary>
    /// Converts a matrix of colors into a bitmap
    /// </summary>
    private static Bitmap Colorm2bitmap(Color[,] im)
    {
      Bitmap im_bitmap = new Bitmap(im.GetLength(0), im.GetLength(1));
      for (int i = 0; i < im.GetLength(0); i++)
      {
        for (int j = 0; j < im.GetLength(1); j++)
        {
          im_bitmap.SetPixel(i, j, im[i, j]);
        }
      }
      return im_bitmap;
    }

    /// <summary>
    /// Converts a bitmap into a matrix of colors
    /// </summary>
    private static Color[,] Bitmap2colorm(Bitmap im_bitmap)
    {
      Color[,] im = new Color[im_bitmap.Width, im_bitmap.Height];

      for (int i = 0; i < im_bitmap.Width; i++)
      {
        for (int j = 0; j < im_bitmap.Height; j++)
        {
          im[i, j] = im_bitmap.GetPixel(i, j);
        }
      }
      return im;
    }

    /// <summary>
    /// Load an image from file and returns a matrix of Color
    /// </summary>
    /// <param name="path">File path relative to the executable</param>
    public static Color[,] LoadImage(string path)
    {
      Bitmap inputBitmap = new Bitmap(path);
      Color[,] inputImage = Bitmap2colorm(inputBitmap);
      return inputImage;
    }

    /// <summary>
    /// Draw a single image in a window
    /// </summary>
    /// <param name="inputImage">Color matrix representing the input image</param>
    public static void DrawImage(Color[,] inputImage)
    {
      int width = inputImage.GetLength(0);
      int height = inputImage.GetLength(1);

      Bitmap inputBitmap = Colorm2bitmap(inputImage);

      PictureBox inputPicture = new PictureBox {
        Image = inputBitmap,
        Width = width,
        Height = height
    };

      //if width of image bigger than screen, then adapt it. Needs to be fixed so that scale factor calculation also depends on the height
      int sw = Screen.PrimaryScreen.WorkingArea.Width;
      int sh = Screen.PrimaryScreen.WorkingArea.Height;
      inputPicture.SizeMode = PictureBoxSizeMode.Zoom;
      if (inputBitmap.Width > sw || inputBitmap.Height > sh)
      {
        int scaleFactor = (int)Math.Ceiling((float)inputBitmap.Width / (float)sw);
        inputPicture.Size = new Size(inputBitmap.Width / scaleFactor, inputBitmap.Height / scaleFactor);
      }

      Form imageWindow = new Form
      {
        Width = width,
        Height = height
      };
      imageWindow.Controls.Add(inputPicture);
      imageWindow.ShowDialog();
    }

    /// <summary>
    /// Draw a pair of images in a window. The images are scaled down if their size is too big.
    /// </summary>
    /// <param name="inputImage">Color matrix representing the input image</param>
    /// <param name="outputImage">Color matrix representing the output image</param>
    public static void DrawImagePair(Color[,] inputImage, Color[,] outputImage)
    {

      Bitmap inputBitmap = Colorm2bitmap(inputImage);
      Bitmap outputBitmap = Colorm2bitmap(outputImage);

      PictureBox inputPicture = new PictureBox {
        Width = inputBitmap.Width,
        Height = inputBitmap.Height,
        Image = inputBitmap
      };
      PictureBox outputPicture = new PictureBox {
        Width = outputBitmap.Width,
        Height = outputBitmap.Height,
        Image = outputBitmap
      };

      inputPicture.SizeMode = outputPicture.SizeMode = PictureBoxSizeMode.Zoom;

      //if width of image bigger than screen, then adapt it. Needs to be fixed so that scale factor calculation also depends on the height
      int sw = Screen.PrimaryScreen.WorkingArea.Width;
      int sh = Screen.PrimaryScreen.WorkingArea.Height;
      if (inputBitmap.Width + outputBitmap.Width > sw || Math.Max(inputBitmap.Height, outputBitmap.Height) > sh)
      {
        int scaleFactor = (int)Math.Ceiling((float)(inputBitmap.Width + outputBitmap.Width) / (float)sw);
        inputPicture.Size = new Size(inputBitmap.Width / scaleFactor, inputBitmap.Height / scaleFactor);
        outputPicture.Size = new Size(outputBitmap.Width / scaleFactor, outputBitmap.Height / scaleFactor);
      }

      Form imageWindow = new Form {
        Width = inputPicture.Width + outputPicture.Width,
        Height = Math.Max(inputPicture.Height, outputPicture.Height)
      };
      outputPicture.Location = new Point(inputPicture.Width,0);
      imageWindow.Controls.Add(inputPicture);
      imageWindow.Controls.Add(outputPicture);
      imageWindow.ShowDialog();
    }

    }
 
}
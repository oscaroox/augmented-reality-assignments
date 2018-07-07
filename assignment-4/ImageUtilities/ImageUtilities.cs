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

    public static Color[,] unwarp(Color[,] im, int cx, int cy, int rin, int rout)
    {

            int width = (int)Math.Round(2 * Math.PI * rout);
            int height = rout - rin;

            Bitmap res = new Bitmap(width, height);
  
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    
                    double polarO = -2.0 * Math.PI * ((double)x / (double)width);
                    double polarR = rin + (1.0 - ((double)y / (double)height)) * (rout - rin);


                    double trueX = polarR * Math.Cos(polarO) + cx;
                    double trueY = polarR * Math.Sin(polarO) + cy;

                    if (trueX + 2 < im.GetLength(0) && trueY + 2 < im.GetLength(1))
                    {
                        int floorX = (int)(Math.Round(trueX));
                        int floorY = (int)(Math.Round(trueY));

                        double deltaX = 0;
                        double deltaY = 0;

                        if (trueX > floorX)
                        {
                            deltaX = trueX - floorX;
                        }
                        else if (trueX < floorX)
                        {
                            deltaX = floorX - trueX;
                        }

                        if (trueY > floorY)
                        {
                            deltaY = trueY - floorY;
                        }
                        else if (trueY < floorY)
                        {
                            deltaY = floorY - trueY;
                        }


                        int r1 = floorX + 1;
                        int c1 = floorY + 1;

                        double xr = 1 - deltaX;
                        double xc = 1 - deltaY;


                        var clrTopLeft = im[floorX, floorY];
                        var clrTopRight = im[r1, floorY];
                        var clrBottomLeft = im[floorX, c1];
                        var clrBottomRight = im[r1, c1];

                        var jr = (clrTopLeft.R * xr * xc) +
                                (clrTopRight.R * deltaX * xc) +
                                (clrBottomLeft.R * xr * deltaY) +
                                (clrBottomRight.R * deltaX * deltaY);

                        var jg = (clrTopLeft.G * xr * xc) +
                                (clrTopRight.G * deltaX * xc) +
                                (clrBottomLeft.G * xr * deltaY) +
                                (clrBottomRight.G * deltaX * deltaY);

                        var jb = (clrTopLeft.B * xr * xc) +
                                (clrTopRight.B * deltaX * xc) +
                                (clrBottomLeft.B * xr * deltaY) +
                                (clrBottomRight.B * deltaX * deltaY);

                        res.SetPixel(x, y, Color.FromArgb(
                            Clamp(jr),
                            Clamp(jg),
                            Clamp(jb)
                        ));
                    }
                    else
                    {
                        res.SetPixel(x, y, Color.FromArgb(255, 0, 0, 0));
                    }



                }
            }

            return Bitmap2colorm(res);
    }

    private static int Clamp(double v)
    {
            var pixel = Convert.ToInt32(v);
            if (pixel < 0) return 0;
            if (pixel > 255) return 255;
            return pixel;
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
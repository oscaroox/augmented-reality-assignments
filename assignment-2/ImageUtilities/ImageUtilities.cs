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

    public static Color[,] NearestNeighbourInterpolation(Color[,] im, double scaleFactor)
    {
            var sourceWidth = im.GetLength(0);
            var sourceHeight = im.GetLength(1);

            var sr = (int)(sourceWidth * scaleFactor);
            var sc = (int)(sourceHeight * scaleFactor);

            var bitmap = new Bitmap(sr, sc);

            for(var x = 0; x < sr; x++)
            {
                for(var y = 0; y < sc; y++)
                {
                    var rf = x / scaleFactor;
                    var cf = y / scaleFactor;

                    var r = (int)Math.Floor(rf);
                    var c = (int)Math.Floor(cf);

                    if (r >= sourceWidth) r = sourceWidth - 1;
                    if (c >= sourceHeight) c = sourceHeight - 1;

                    var co = im[r, c];

                    bitmap.SetPixel(x, y, Color.FromArgb(
                        Convert.ToInt32(co.A),
                        Convert.ToInt32(co.R),
                        Convert.ToInt32(co.G),
                        Convert.ToInt32(co.B)
                       ));
                    
                }
            }

            return Bitmap2colorm(bitmap);
    }


     public static Color[,] BilinearInterpolation(Color[,] im, double scaleFactor)
    {
            var sourceWidth = im.GetLength(0);
            var sourceHeight = im.GetLength(1);


            var sr = (int)(sourceWidth * scaleFactor);
            var sc = (int)(sourceHeight * scaleFactor);
               
            Bitmap bitmap = new Bitmap(sr, sc);

            for (var x = 0; x < sr; x++)
            {
                for (var y = 0; y < sc; y++)
                {
                    var rf = x / scaleFactor;
                    var cf = y / scaleFactor;

                    var r = (Int32)Math.Floor(rf);
                    var c = (Int32)Math.Floor(cf);

                    var delta_r = rf - r;
                    var delta_c = cf - c;

                    var xr = 1 - delta_r;
                    var xc = 1 - delta_c;

                    var r1 = r + 1;
                    var c1 = c + 1;

                    if (r >= sourceWidth) r = sourceWidth - 1;
                    if (c >= sourceHeight) c = sourceHeight - 1;

                    if (r1 >= sourceWidth)
                    {
                        r1 = r;
                    }

                    if (c1 >= sourceHeight)
                    {
                        c1 = c;
                    }

                    var co1 = im[r, c];
                    var co2 = im[r1, c];
                    var co3 = im[r, c1];
                    var co4 = im[r1, c1];

                    var ja = (co1.A * xr * xc) +
                            (co2.A * delta_r * xc) +
                            (co3.A * xr * delta_c) +
                            (co4.A * delta_r * delta_c);

                    var jr = (co1.R * xr * xc) +
                            (co2.R * delta_r * xc) +
                            (co3.R * xr * delta_c) +
                            (co4.R * delta_r * delta_c);

                    var jg = (co1.G * xr * xc) +
                            (co2.G * delta_r * xc) +
                            (co3.G * xr * delta_c) +
                            (co4.G * delta_r * delta_c);

                    var jb = (co1.B * xr * xc) +
                            (co2.B * delta_r * xc) +
                            (co3.B * xr * delta_c) +
                            (co4.B * delta_r * delta_c);


                    bitmap.SetPixel(x, y, Color.FromArgb(
                        Convert.ToInt32(ja),
                        Convert.ToInt32(jr),
                        Convert.ToInt32(jg),
                        Convert.ToInt32(jb)
                    ));

                }
            }

            return Bitmap2colorm(bitmap);
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
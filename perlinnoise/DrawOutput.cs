using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
namespace perlinnoise
{
    public partial class DrawOutput : Form
    {
        
        public DrawOutput()
        {
            InitializeComponent();
        }
        public void ToBitmap(double[,] smoothnoise)
        {
            Graphics graphics = pictureBox1.CreateGraphics();
            int boxsizeX = Convert.ToInt32(pictureBox1.Width / smoothnoise.GetLength(0)),boxsizeY = Convert.ToInt32(pictureBox1.Height/smoothnoise.GetLength(1));
            Console.WriteLine("boxsizeX,Y are {0},{1} respectively. press enter to continue", boxsizeX, boxsizeY);
 //           Console.ReadLine();
            double color;
            pictureBox1.Show();
            Bitmap bitmap= new Bitmap(pictureBox1.Width,pictureBox1.Height,graphics);
            
            for (int x=0;x<smoothnoise.GetLength(0);x++)
            {
                for (int y=0;y<smoothnoise.GetLength(1);y++)
                {
                    //for each box in the bitmap
                    color = (255 * smoothnoise[x, y]);
                    Console.WriteLine("color value = {0}", color);
                    
                    for (int pxX=0;pxX<boxsizeX;pxX++)
                    {
                        for (int pxY=0;pxY<boxsizeY;pxY++)
                        {
                            Console.WriteLine("pxX and pxY are:{0},{1}", pxX, pxY);
                            
                            //for each pixel in each box, set its color to var color
                            bitmap.SetPixel(((x*boxsizeX)+pxX), ((y*boxsizeY)+pxY), Color.FromArgb(255, Convert.ToInt32(color), Convert.ToInt32(color), Convert.ToInt32(color)));
                            Console.WriteLine("pixel set was: {0},{1}", (x * boxsizeX) + pxX, (y * boxsizeY) + pxY);
                            pictureBox1.Image = bitmap;
                            pictureBox1.Refresh();
                            Debug.WriteLine("grayscale color at [{0},{1}] is {2}", ((x*boxsizeX)+pxX), ((y*boxsizeY)+pxY), color);
                        }
                    }
                }
            }
            bitmap.Save((Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)) + "\\noiseimg.png", System.Drawing.Imaging.ImageFormat.Png);
            Console.WriteLine("Saved file. check image. press enter to generate interpolated image");
//            Console.ReadLine();
            //now for interpolation
            double x0, x1, x2,x3, internoise;
            for (int pointX = 0; pointX < (bitmap.Width) - 1; pointX++)
            {
                for (int pointY = 0; pointY < (bitmap.Height) - 1; pointY++)
                {
                    x0 = bitmap.GetPixel(pointX, pointY).R;
                    x1 = bitmap.GetPixel(pointX + 1, pointY).R;
                    x2 = bitmap.GetPixel(pointX, pointY + 1).R;
                    x3 = bitmap.GetPixel(pointX+1, pointY+1).R;
                    internoise = (x0 + x1 + x2+x3) / 4;
                    Console.WriteLine("interpolated noise at {0},{1} is {2}", pointX, pointY, internoise);
                    bitmap.SetPixel(pointX, pointY, Color.FromArgb(255, Convert.ToInt32(internoise), Convert.ToInt32(internoise), Convert.ToInt32(internoise)));
                    smoothnoise[pointX, pointY] = bitmap.GetPixel(pointX, pointY).R;
                }
            }
            bitmap.Save((Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)) + "\\internoiseimg.png", System.Drawing.Imaging.ImageFormat.Png);
            Console.WriteLine("Interpolation complete. File saved on Desktop. Press enter to leave");
            
        }
        public double[,] NoiseGradient(double[,] smoothnoise)
        {//add bias into the map, central mountains and outside square of water
            double centerdist,multiplier;
            for (int x=0;x<smoothnoise.GetLength(0);x++)
            {
                for (int y=0;y<smoothnoise.GetLength(1);y++)
                {
                    centerdist = Math.Sqrt(Convert.ToDouble(((((smoothnoise.GetLength(0))/2)-x)^2)+ (((smoothnoise.GetLength(1))/2) - y) ^ 2));
                    multiplier = centerdist / Math.Sqrt(((smoothnoise.GetLength(0) ^ 2) + (smoothnoise.GetLength(1) ^ 2)));
                    smoothnoise[x, y] *= multiplier;
                }
            }
            return smoothnoise;
        }
        public void TerrainMapper(double[,] smoothnoise)
        {
            Console.WriteLine("Generating Color map");
            Bitmap bitmap = new Bitmap(smoothnoise.GetLength(0), smoothnoise.GetLength(1));
            for (int x=0;x<smoothnoise.GetLength(0);x++)
            {
                for (int y=0;y<smoothnoise.GetLength(1);y++)
                {
                    double noise = smoothnoise[x, y];
                    Console.WriteLine("noise value at {0},{1} is {2}", x, y, noise);
                    if (noise < 0)
                    {
                        noise = 0;
                    }
                    else if (noise>255)
                    {
                        noise = 255;
                    }
                    bitmap.SetPixel(x, y, Color.FromArgb(255, Convert.ToInt32(noise), Convert.ToInt32(noise), Convert.ToInt32(noise)));
                }
            }
            Color color = new Color();
            for (int a=0;a<bitmap.Width;a++)
            {
                for(int b=0;b<bitmap.Height;b++)
                {                    
                    if (bitmap.GetPixel(a,b).R >=(0.8*255))
                    {
                        bitmap.SetPixel(a, b, Color.SlateGray);
                        color = Color.SlateGray;
                    }
                    else if (bitmap.GetPixel(a,b).R>=(0.6*255)&& bitmap.GetPixel(a, b).R<(0.8*255))
                    {
                        bitmap.SetPixel(a, b, Color.Olive);
                        color = Color.Olive;
                    }
                    else if (bitmap.GetPixel(a, b).R >= (0.4*255) && bitmap.GetPixel(a, b).R < (0.6*255))
                    {
                        bitmap.SetPixel(a, b, Color.Green);
                        color = Color.Green;
                    }
                    else if (bitmap.GetPixel(a, b).R >= (0.2*255) && bitmap.GetPixel(a, b).R < (0.4*255))
                    {
                        bitmap.SetPixel(a, b, Color.SandyBrown);
                        color = Color.SandyBrown;
                    }
                    else
                    {
                        bitmap.SetPixel(a, b, Color.Blue);
                        color = Color.Blue;
                    }
                    Console.WriteLine("Color at {0},{1} is {2}", a, b, color);
                }
            }
            bitmap.Save((Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)) + "\\colornoiseimg.png", System.Drawing.Imaging.ImageFormat.Png);
            Console.WriteLine("Colored map saved. Press enter to leave");
        }

        
    }
}

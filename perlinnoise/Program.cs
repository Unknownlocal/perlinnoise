using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
namespace perlinnoise
{
    class Program
    {
        public double[,] smoothnoise= new double[64,64];
        public int freq=64, octave=1;
        
        public void WhiteNoise()
        {
            Random random = new Random();
            //for (int f=1;f<=64;f/=2)
            //{
                //freq = f;                
                for (int x=0;x<smoothnoise.GetLength(0);x+=freq)
                {
                    for (int y=0;y<smoothnoise.GetLength(1);y+=freq)
                    {
                        smoothnoise[x, y] += random.NextDouble();
                    }
                }
                octave += 1;
            //}
        }
        public void Draw()
        {
            PictureBox pictureBox = new PictureBox();
            pictureBox.Width = 600;pictureBox.Height = 800;
            pictureBox.Show();
            pictureBox.CreateControl(); 
            Graphics g = pictureBox.CreateGraphics();
            Bitmap bitmap = new Bitmap(pictureBox.Width, pictureBox.Height, g);
            for(int x=0;x<bitmap.Width;x++)
            {
                for(int y=0;y<bitmap.Height;y++)
                {
                    
                }
            }
                    
        }            
        static void Main(string[] args)
        {
        }
    }
}

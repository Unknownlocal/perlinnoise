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
        public double[,] smoothnoise= new double[512,512];
        public int freq=1, octave=1;
        public double currentnoise=0;
        public void WhiteNoise()
        {
            Random random = new Random();
            //for (int f=1;f<=64;f/=2)
            //{
                //freq = f;             //for everysqaure generate a noise value   
                //for each octave in the 64x64 array, generate one noise value and add it on. as freq--, make each ovtace have the same randvalue added to it
            while (freq <= smoothnoise.GetLength(0)||freq <= smoothnoise.GetLength(1))
            {
                for (int i = 0; i < (smoothnoise.GetLength(0) / freq); i++)
                {
                    for (int j = 0; j < (smoothnoise.GetLength(1) / freq); j++)
                    {
                        //for each block.
                        currentnoise = random.NextDouble();
                        for (int x = 0; x < freq; x++)
                        {
                            for (int y = 0; y < freq; y++)
                            {
                                smoothnoise[((i * freq) + x), ((j * freq) + y)] += currentnoise;
                            }
                        }
                    }
                }
                freq *= 2;
                octave += 1;
                
            }
            for(int x=0;x<smoothnoise.GetLength(0);x++)
            {
                for(int y=0;y<smoothnoise.GetLength(1);y++)
                {
                    smoothnoise[x, y] /= octave;
                }
            }
            //interpolation and standardization
            /*for (int c=0;c<smoothnoise.GetLength(0);c++)
            {
                for (int d=0;d<smoothnoise.GetLength(1);d++)
                {
                    
                    if (c < -1+smoothnoise.GetLength(0)&&d<-1+smoothnoise.GetLength(1))
                    {
                        smoothnoise[c, d] = ((smoothnoise[c + 1, d] + smoothnoise[c, d + 1] + smoothnoise[c + 1, d + 1] + smoothnoise[c, d]) / 4)/octave;
                    }
                    else
                    {
                        smoothnoise[c, d] /= octave;
                    }
                }
            }*/
            /*for (int a = 0; a < smoothnoise.GetLength(0); a++)
            {
                for (int b = 0; b < smoothnoise.GetLength(1); b++)
                {

                   // Console.WriteLine("Noise Value at [{0},{1}] is: {2}", a, b, smoothnoise[a, b]);
                }
            }*/

            //}
        }
        
        static void Main(string[] args)
        {
            Program program = new Program();
            DrawOutput draw = new DrawOutput();
            program.WhiteNoise();
            Console.WriteLine("whitenoise complete.");
            draw.ToBitmap(program.smoothnoise);
            Console.WriteLine("smoothnoise complete");
            //draw.TerrainMapper(draw.NoiseGradient(program.smoothnoise));
            draw.ColorMap(program.smoothnoise);
            Console.WriteLine("ColorMap complete");
            Console.ReadLine();
        }
    }
}

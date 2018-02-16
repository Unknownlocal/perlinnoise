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
        public int freq=1, octave=1,ctr=1;
        public double[,] noise1 = new double[64,64];
        public double[,] noise2 = new double[64,64];
        public double[,] fractal = new double[64,64];
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
                        currentnoise = (random.NextDouble()*ctr)/10;
                        if (currentnoise > 1)
                        {
                            currentnoise = 1;
                        }
                        else if (currentnoise < 0)
                        {
                            currentnoise = 0;
                        }
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
                ctr += ctr;
            }
            for(int x=0;x<smoothnoise.GetLength(0);x++)
            {
                for(int y=0;y<smoothnoise.GetLength(1);y++)
                {
                    smoothnoise[x, y] /= octave;
                }
            }
        }
       /* public double[,] fractalnoise()
        {
            double[,] noise = new double[smoothnoise.GetLength(0),smoothnoise.GetLength(1)];
            Random random = new Random();
            int f = 1, oct = 1;
            double noisenow = 0,xdist,ydist,centerdist,c1=0;
            while (f <= noise.GetLength(0) || f <= noise.GetLength(1))
            {
                for (int i = 0; i < (noise.GetLength(0) / f); i++)
                {
                    for (int j = 0; j < (noise.GetLength(1) / f); j++)
                    {
                        //for each block.
                        noisenow = (random.NextDouble() * ctr) / 10;
                        if (noisenow > 1)
                        {
                            noisenow = 1;
                        }
                        else if (noisenow < 0)
                        {
                            noisenow = 0;
                        }
                        for (int x = 0; x < f; x++)
                        {
                            for (int y = 0; y < f; y++)
                            {
                                noise[((i * f) + x), ((j * f) + y)] += noisenow;
                            }
                        }
                    }
                }
                f *= 2;
                oct += 1;
            }
            for (int px=0;px<noise.GetLength(0);px++)
            {
                for (int py=0;py<noise.GetLength(1);py++)
                {
                    xdist = (((1+noise.GetLength(0))/2) - px);
                    ydist = (((noise.GetLength(1)+1)/2) - py);
                    xdist *= xdist;ydist *= ydist;
                    centerdist = xdist + ydist;
                    centerdist = Math.Sqrt(centerdist);
                    if (c1 < centerdist)
                    {
                        c1 = centerdist;
                    }
                    Console.WriteLine("centerdist of {0},{1} is {2}", px, py, centerdist);
                }
            }
            //now that base values are generated, time to scale noise
            Console.WriteLine("Largest distance in array is {0}", c1);
            return noise;
        }*/
        public void FractalNoiseGen()
        {
            Random rand = new Random();
            Random random = new Random();
            double centerdist,xdist,ydist;
            int freq=1, octave=1;
            double current1, current2;
            while (freq <= noise1.GetLength(0)||freq<=noise1.GetLength(1))
            {
                for(int x=0;x<noise1.GetLength(0)/freq;x++)
                {
                    for(int y=0;y<noise1.GetLength(1)/freq;y++)
                    {
                        current1 = rand.NextDouble();
                        current2 = random.NextDouble();
                        if (current1>1)
                        {
                            current1 = 1;
                        }
                        else if (current1 < 0)
                        {
                            current1 = 0;
                        }
                        if (current2 > 1)
                        {
                            current2 = 1;
                        }
                        else if (current2 < 0)
                        {
                            current2 = 0;
                        }
                        for (int i=0;i<freq;i++)
                        {
                            for (int j=0;j<freq;j++)
                            {
                                noise1[(x * freq) + i, (y * freq) + j] += current1;
                                noise2[(x * freq) + i, (y * freq) + j] += current2;
                            }
                        }
                    }
                }
                freq *= 2;octave += 1;
            }
            for (int xx=0;xx<noise1.GetLength(0);xx++)
            {
                for (int yy=0;yy<noise1.GetLength(1);yy++)
                {
                    noise1[xx, yy] /= octave;noise2[xx, yy] /= octave;
                }
            }
            // now to add the 2 arrays together
            for (int px=0;px<noise2.GetLength(0);px++)
            {
                for (int py=0;py<noise2.GetLength(1);py++)
                {
                    xdist = px - ((noise1.GetLength(0) + 1) / 2);
                    ydist = py - ((noise1.GetLength(1) + 1) / 2);
                    centerdist = Math.Pow(xdist, 2) + Math.Pow(ydist, 2);
                    centerdist = Math.Sqrt(centerdist);
                    fractal[px, py] = 0.1*Math.Sqrt(centerdist)*(noise1[px, py] + noise2[px, py]);
                    Console.WriteLine("value of fractalnoise gird at [{0},{1}] is {2}, made with centerdist = {3}", px, py, fractal[px, py], centerdist);
                    //check values are as 0<x<1
                    if (fractal[px,py]>1)
                    {
                        fractal[px, py] = 1;
                    }
                    else if (fractal[px,py]<0)
                    {
                        fractal[px, py] = 0;
                    }
                }
            }
            
        }
        static void Main(string[] args)
        {
            Program program = new Program();
            DrawOutput draw = new DrawOutput();
            
            /*program.WhiteNoise();
            Console.WriteLine("whitenoise complete.");*/
            // program.fractalnoise();
            program.FractalNoiseGen();
            Console.WriteLine("fractal noise complete");
            draw.ToBitmap(program.fractal);
            Console.WriteLine("smoothnoise complete");
            draw.ColorMap(program.fractal);
            /*//draw.TerrainMapper(draw.NoiseGradient(program.smoothnoise));
            draw.ColorMap(program.smoothnoise);*/
            Console.WriteLine("ColorMap complete");/*
            draw.Landmakerftw(program.smoothnoise, draw.output);
            Console.WriteLine("applied corrections to map");
            draw.DiffChecker(draw.output, draw.edit);*/
            Console.ReadLine();
        }
    }
}

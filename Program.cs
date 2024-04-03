using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace нулевой_порядок_2_переменные          //Nelder-Mead
{
    internal class Program
    {
        static double e = 0.000001;
        static double alpha = 0.9;
        static double beta = 0.6;
        static double gamma = 2;
        static double f(double[] x)
        {
            double f = x[0] + 2 * x[1] + 4 * Math.Sqrt(1 + x[0] * x[0] + x[1] * x[1]);
            return f;
        }
        static double[] mirror(double[] xc, double[] xh)
        {
            double[] xr = { (1+alpha)*xc[0]-alpha*xh[0], (1 + alpha) * xc[1] - alpha*xh[1] };
            return xr;
        }
        static double[] stretch(double[] xc, double[] xr)
        { 
            double[] xe = { (1-gamma)*xc[0]+gamma*xr[0], (1-gamma) * xc[1] + gamma*xr[1] };
            return xe;
        }
        static double[] compress(double[] xc, double[] xr)
        {
            double[] xk = { (1-beta)*xc[0]+beta*xr[0], (1-beta) * xc[1] + beta*xr[1] };
            return xk;
        }
        static double[] reduction(double[] x, double[] xl)
        {
            double[] xreduced = { (x[0] - xl[0]) / 2 + xl[0], (x[1] - xl[1]) / 2 + xl[1]};
            return xreduced;
        }
        static void Main(string[] args)
        {
            int counter = 0;
            double[] x0 = { 1, 1 };
            double[] x1 = { -1, 0 };
            double[] x2 = { -1, -1 };
            while (Math.Sqrt((Math.Pow((f(x0) - (f(x0) + f(x1) + f(x2)) / 3),2)
                + Math.Pow((f(x1) - (f(x0) + f(x1) + f(x2)) / 3), 2) +
                Math.Pow((f(x2) - (f(x0) + f(x1) + f(x2)) / 3), 2)) /3) >=e)
            {
                counter++;                      //how many cycles will it take?
                double f0 = f(x0);
                double f1 = f(x1);
                double f2 = f(x2);
                double[] fprep = new[] { f0, f1, f2 };
                Array.Sort(fprep);
                double fh = fprep[2];
                double fl = fprep[0];
                double[] xmid = new double[2];
                double[] xh = new double[2];
                double[] xl = new double[2];
                double[] xg = new double[2];    //finding xc, xl, xg, xh
                if (fh == f2)                   //unoptimised, but works ;)
                {
                    xmid[0] = (x0[0] + x1[0]) / 2;
                    xmid[1] = (x0[1] + x1[1]) / 2;
                    xh = x2;
                    if (fl == f0)
                    {
                        xl = x0;
                        xg = x1;
                    }
                    else
                    {
                        xl = x1;
                        xg = x0;
                    }
                }
                else if (fh == f1)
                {
                    xmid[0] = (x0[0] + x2[0]) / 2;
                    xmid[1] = (x0[1] + x2[1]) / 2;
                    xh = x1;
                    if (fl == f0)
                    {
                        xl = x0;
                        xg = x2;
                    }
                    else
                    {
                        xl = x2;
                        xg = x0;
                    }
                }
                else
                {
                    xmid[0] = (x2[0] + x1[0]) / 2;
                    xmid[1] = (x2[1] + x1[1]) / 2;
                    xh = x0;
                    if (fl == f1)
                    {
                        xl = x1;
                        xg = x2;
                    }
                    else
                    {
                        xl = x2;
                        xg = x1;
                    }
                }
                //functions calls
                double[] xr = mirror(xmid, xh);                 //отражение\reflection
                double fr = f(xr);
                if (fr < fl)
                {
                    double[] xe = stretch(xmid, xr);            //растяжение\stretching
                    double fe = f(xe);
                    if ( fr >= fe)
                    {
                        if (fr < fh)
                        {
                            x0 = xl;
                            x1 = xg;
                            x2 = xe;
                        }
                        else
                        {
                            double[] xk = compress(xmid, xr);   //сжатие\compressing
                            double fk = f(xk);
                            if (fk > fh)
                            {
                                x2 = xl;
                                x1 = reduction(xg, xl);         //редукция\reduction
                                x0 = reduction(xh, xl);
                            }
                            else 
                            {
                                x0 = xl;
                                x1 = xg;
                                x2 = xk; 
                            }
                        }
                        
                    }
                    else
                    {
                        x0 = xl;
                        x1 = xg;
                        x2 = xr;
                    }
                }
                else
                {
                    x0 = xl;
                    x1 = xg;
                    x2 = xr;
                }
            }
            Console.WriteLine("Координаты 1 точки:\nx1 = " + x0[0].ToString() + "\nx2 = " + x0[1].ToString() + "\nЗначение функции в точке: " + f(x0).ToString());
            Console.WriteLine("\nКоординаты 2 точки:\nx1 = " + x1[0].ToString() + "\nx2 = " + x1[1].ToString() + "\nЗначение функции в точке: " + f(x0).ToString());
            Console.WriteLine("\nКоординаты 3 точки:\nx1 = " + x2[0].ToString() + "\nx2 = " + x2[1].ToString()  + "\nЗначение функции в точке: " + f(x0).ToString() + "\nКоличество циклов: " + counter.ToString());
            Console.Write("Press any key to exit..");
            Console.ReadKey();
        }
    }
}
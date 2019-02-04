using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrix_CSharp_DLL;

namespace Matrix_CSharp
{

    public class Matrix
    {
        int n { get; set; }
        public double time { get; set; }
        MatrixElement matrix_element_func;

        public Matrix(int n, MatrixElement m_e)
        {
            this.n = n;
            this.matrix_element_func = m_e;
        }

        public bool Solve(double[] rh, ref double[] res)
        {
            double[] row = new double[n];
            double[] column = new double[n];
            double[] x = new double[n];
            double[] y = new double[n];
            double[] x_buff = new double[n];
            double[] y_buff = new double[n];
            double[] buff = new double[n];
            double F, G, R, S, T;

            //create row and column
            for (int i = 0; i < n; i++)
            {
                row[i] = matrix_element_func(n, i, true);
                column[i] = matrix_element_func(n, i, false);
                
            }

            var sw = System.Diagnostics.Stopwatch.StartNew();

            x[0] = 1 / row[0];
            y[0] = 1 / row[0];

            for (int j = 1; j < n; j++)
            {
                F = 0; G = 0;

                for (int i = 0; i < j; i++)
                {   
                    //вычисляем коэффициенты F и G
                    F += column[j - i] * x[i];
                    G += row[i + 1] * y[i];
                }

                R = 1 / (1 - F * G);
                S = -R * F;
                T = -R * G;

                y[j] = y[j - 1] * R;        //вычисляем отдельно первые и последние коэффициенты 
                x[j] = y[j - 1] * S;        //для оптимизации цикла
                for (int i = j - 1; i > 0; --i)
                {
                    y[i] = x[i] * T + y[i - 1] * R;
                    x[i] = x[i] * R + y[i - 1] * S;
                }
                y[0] = x[0] * T;
                x[0] = x[0] * R;
            }
            
            for (int i = 0; i < n; ++i)
            {
                buff[i] = 0;
                for (int j = 0; j < n; ++j)
                {
                    int num = i - j;
                    if (num < 0)
                        buff[i] += x[n + num] * rh[j];
                }
            }

            for (int i = 0; i < n; ++i)
            {
                x_buff[i] = 0;
                for (int j = 0; j < n; ++j)
                {
                    int num = i - j;
                    if (num > 0)
                        x_buff[i] += y[num - 1] * buff[j];
                }
            }

            for (int i = 0; i < n; ++i)
            {
                buff[i] = 0;
                for (int j = 0; j < n; ++j)
                {
                    int num = i - j;
                    if (num <= 0)
                        buff[i] += y[n + num - 1] * rh[j];
                }
            }

            for (int i = 0; i < n; ++i)
            {
                y_buff[i] = 0;
                for (int j = 0; j < n; ++j)
                {
                    int num = i - j;
                    if (num >= 0)
                        y_buff[i] += x[num] * buff[j];
                }
            }

            for (int i = 0; i < n; ++i)
            {
                res[i] = (y_buff[i] - x_buff[i]) / x[0];
            }

            sw.Stop();
            time = sw.ElapsedMilliseconds / 1000.0;

            return true;
        }

        ~Matrix() { }
    };
}

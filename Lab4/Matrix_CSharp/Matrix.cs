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
        double time { get; set; }
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

            //create row and column
            for (int i = 0; i < n; i++)
            {
                row[i] = matrix_element_func(n, i, true);
                column[i] = matrix_element_func(n, i, false);
            }

            double[] x = new double[n];
            double[] y = new double[n];
            double[] x_buff = new double[n];
            double[] y_buff = new double[n];

            double  F, G, R, S, T;
            x_buff[0] = 1 / row[0];
            y_buff[0] = 1 / row[0];
            
            //Вывод матрицы
            /*for (int j = 0; j < n; j++)
            {
                for (int i = j; i >= 0; i--)
                {
                    System.Console.Write(column[i] + " ");     //ПРОВЕРИТЬ!!
                }
                for (int i = j+1; i < n; i++)
                {
                    System.Console.Write(row[i] + " ");
                }
                System.Console.Write("\n");
            }
            */

            for (int j = 1; j < n; j++)
            {
                F = 0; G = 0;

                for (int i = 0; i < j; i++)
                {   
                    //вычисляем коэффициенты F и G
                    F += column[j - i] * x_buff[i];
                    G += row[i + 1] * y_buff[i];
                }

                
                R = 1 / (1 - F * G);
                S = -R * F;
                T = -R * G;

                x[0] = x_buff[0] * R;        //вычисляем отдельно первые и последние коэффициенты 
                x[j] = y_buff[j - 1] * S;    //для оптимизации цикла

                y[0] = x_buff[0] * T;
                y[j] = y_buff[j - 1] * R;

                for (int d = 1; d < j; d++)
                {
                    x[d] = x_buff[d] * R + y_buff[d - 1] * S;
                    y[d] = x_buff[d] * T + y_buff[d - 1] * R;
                }

                for (int d = 0; d <= j; d++)
                {
                    x_buff[d] = x[d];
                    y_buff[d] = y[d];
                }
            }

            //находим решение res[]
            for (int j = 0; j < n; j++)
            {
                res[j] = 0;
                for (int i = n - j - 1; i < n - 1; i++)
                {
                    //System.Console.Write(y[i] + " ");
                    res[j] += y[i];
                }

                for (int i = 0; i < n - j; i++)
                {
                    //System.Console.Write(x[i] + " ");
                    res[j] += x[i];
                }

                res[j] *= rh[j];
            }
            return true;
        }

        ~Matrix() { }
    };
}

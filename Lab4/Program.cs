using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Matrix_CSharp;
using Matrix_CSharp_DLL;

namespace Lab4
{


    class Program
    {
        [System.Runtime.InteropServices.DllImport("..\\..\\..\\DEBUG\\Matrix_CPP_DLL", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GlobalFunction(
        int n,
        double[] row,
        double[] column,
        double[] rh,
        double[] answ,
        ref double time
        );

        public static void multiply(double[,] A, ref double[] x)
        {
            System.Console.WriteLine("Проверим правильность вектора правой части ( А * х = rh ? ):");
            double[] buffer = new double[4];
            for (int j = 0; j < 4; j++)
            {
                double buff = 0;
                for(int i = 0; i < 4; i++)
                {
                    buff += A[j,i] * x[i];
                }
                buffer[j] = buff;
            }
            for (int j = 0; j < 4; j++)
                x[j] = buffer[j];
        }

        public static void Test()
        {
            System.Console.WriteLine("Работа функции Test()");

            double MatrixCoef(int n, int j, bool b)
            {
                if (b)
                {
                    if (j == 1)
                        return 0;
                    else
                        return n - j;
                }
                return n - j;
            }

            Matrix mtr = new Matrix(4, MatrixCoef);

            double[,] A = new double[4, 4] { { 4, 0, 2, 1 }, { 3, 4, 0, 2 }, { 2, 3, 4, 0 }, { 1, 2, 3, 4 } };
            double[] res = new double[4];
            double[] rh = new double[4];
            double[] row = new double[4];
            double[] column = new double[4];

            for (int j = 0; j < 4; j++)
            {
                row[j] = MatrixCoef(4, j, true);
                column[j] = MatrixCoef(4, j, false);
                rh[j] = 1;
            }

            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    System.Console.Write(A[j,i] + " ");
                }
                System.Console.Write("   " + rh[j]);
                System.Console.Write("\n");
            }
        

            mtr.Solve(rh, ref res);
            System.Console.Write("Вектор решений на C#: [{0}, {1}, {2}, {3}]\n\n", res[0], res[1], res[2], res[3]);

            //Solve on C++
            double t_cpp = 0;
            GlobalFunction(4, row, column, rh, res, ref t_cpp);
            System.Console.Write("Вектор решений на C++: [{0}, {1}, {2}, {3}]\n\n", res[0], res[1], res[2], res[3]);

            multiply(A, ref res);
            System.Console.Write("Вектор правой части: [{0}, {1}, {2}, {3}]\n\n", res[0], res[1], res[2], res[3]);
        }

        static void Main(string[] args)
        {
            Test();

            TestTime test_time = new TestTime();
            //Имя файла для загрузки и сохранения
            string filename = "people.dat";
            TestTime.Load(filename, ref test_time);

            while (true)
            {
                try
                {
                    //Вводим порядок матрицы
                    Console.WriteLine("Введите порядок матрицы или 'end' для выхода: ");
                    string str = Console.ReadLine();
                    if (str == "end")
                    {
                        break;
                    }
                    else
                    {
                        int a = Convert.ToInt32(str);

                        TestTimeItem test_time_item = new TestTimeItem();

                        test_time_item.calculateCoef(a);
                        test_time.Add(test_time_item);
                    }
                }
                catch(System.IO.IOException ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Необходимо ввеcти число или 'end'\n");
                }
            }

            TestTime.Save(filename, test_time);
            Console.Write(test_time);

            /*foreach (var ass in AppDomain.CurrentDomain.GetAssemblies())
                foreach (var type in ass.GetTypes())
                    if (type.IsSubclassOf(typeof(Exception)))
                        Console.WriteLine(type);
            */
            System.Threading.Thread.Sleep(5000);
        }

       
    }
}

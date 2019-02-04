using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrix_CSharp;
using Matrix_CSharp_DLL;

namespace Lab4
{

    class Program
    {
        public static void Test()
        {
            Matrix mtr = new Matrix(4, MatrixCoef);

            double MatrixCoef(int n, int j, bool b)
            {
                if (b)
                {
                    if (j == 0)
                    {
                        return 4;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else
                {
                    if (j == 0)
                    {
                        return 4;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }

            double[] res = new double[4];
            double[] rh = new double[4];
            rh[0] = rh[1] = rh[2] = rh[3] = 1;

            mtr.Solve(rh, ref res);
            System.Console.Write("Функция Test()\nВектор решений: [{0}, {1}, {2}, {3}]\n\n", res[0], res[1], res[2], res[3]);
        }

        static void Main(string[] args)
        {
            /*Функция решает систему 4х4 вида:  [4, 1, 1, 1]
                                                [1, 4, 1, 1]
                                                [1, 1, 4, 1]
                                                [1, 1, 1, 4]
            с единичным вектором правой части
            */
            //Test();


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

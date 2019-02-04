using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Matrix_CSharp;
using System.Runtime.Serialization;

namespace Matrix_CSharp_DLL
{

    public delegate double MatrixElement(int n, int j, bool row);

    [Serializable]
    public class TestTimeItem
    {
        [DllImport("..\\..\\..\\DEBUG\\Matrix_CPP_DLL", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GlobalFunction(
        int n,
        double[] row,
        double[] column,
        double[] rh,
        double[] answ,
        ref double time
    );
        public int n { get; set; }
        public double time_csharp { get; set; }
        public double time_cpp { get; set; }
        public double coef { get; set; }

        //delegate for matrix C#
        public double calculateMatrixCoef(int n, int j, bool b)
        {
            return n - j;
        }

        public void calculateCoef(int a)
        {
            this.n = a;
            double[] rh = new double[n],
               row = new double[n],
               column = new double[n],
               answ = new double[n];

            //create rh[]
            for (int i = 0; i < n; i++)
            {
                row[i] = calculateMatrixCoef(n, i, true);
                column[i] = calculateMatrixCoef(n, i, false);
                rh[i] = i+1;
            }

            //Solve on C++
            double t_cpp = 0;
            GlobalFunction(n, row, column, rh, answ, ref t_cpp);
            Console.WriteLine("затрачено времени на C++:{0}", t_cpp);
            time_cpp = t_cpp;

            //Solve on C#
            Matrix mtr = new Matrix(n, calculateMatrixCoef);
            mtr.Solve(rh, ref answ);
            time_csharp = mtr.time;
            Console.WriteLine("затрачено времени на C#:{0}", time_csharp);

            coef = time_csharp / time_cpp;
        }

        public override string ToString()
        {
            return "    Size: " + n + "\n" +
                "    Time on C++: " + time_cpp + "\n" +
                "    Time on C#: " + time_csharp + "\n" +
                "    Coef C#/C++: " + coef + "\n";
        }

    }

    [Serializable]
    public class TestTime
    {
        private List<TestTimeItem> test_time;

        public TestTime(){ }

        public void CreateList()
        {
            test_time = new List<TestTimeItem>();
        }

        public void Add(TestTimeItem time)
        {
            test_time.Add(time);
        }
        
        public static bool Save(string filename, TestTime obj)
        {
            FileStream fs = null;
            try
            {
                fs = File.Open(filename, FileMode.OpenOrCreate);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, obj);
                Console.WriteLine("Объект сериализован");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                if(fs != null)
                {
                    fs.Close();
                }
                  
            }
            return true;
        }

        public override string ToString()
        {
            //typical ToString
            string str = "TestTime ToString():\n";
            foreach (TestTimeItem time in test_time){
                str += time.ToString() + '\n';
            }
            return str;
        }

        public static bool Load(string filename, ref TestTime obj)
        {
            //десериализация из файла people.dat
            
            FileStream fs = null;
            try
            {
                fs = File.OpenRead(filename);
                BinaryFormatter formatter = new BinaryFormatter();
                obj = (TestTime)formatter.Deserialize(fs);
                Console.WriteLine("Объект десериализован");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                System.Console.WriteLine("Не удалось десериализовать файл, создан новый" + filename);
                obj.CreateList(); 
                return false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
            return true;
        }

    }
}

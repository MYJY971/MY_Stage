using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Class1 c1, c2, c3;
            int i=0;

            c1 = new Class2(i);
            c2 = new Class3();
            c3 = new Class4(i);

            c1.Draw();
            c1.Calibrate();
            c1.Control();
            c2.Draw();
            c2.Calibrate();
            c2.Control();
            c3.Draw();
            c3.Calibrate();
            c3.Control();

            string path1, path2;

            path1 = "monPath.jpg";
            path2 = path1.Substring(0, path1.Length - 3)+"xml";
            

            Console.WriteLine(path1);
            Console.WriteLine(path2);


            Console.ReadLine();

        }
    }
}

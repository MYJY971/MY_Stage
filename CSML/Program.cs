using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSML
{
    class Program
    {
        static void Main(string[] args)
        {
            //Matrix M = new Matrix("-0.9377, -0.1085, 0.3301 ; 0.3455, -0.3897, 0.8536; 0.0360, 0.9145 , 0.4029");
            Matrix M = new Matrix(new double[3, 3] { { -0.9377, -0.1085, 0.3301 }, { 0.3455, -0.3897, 0.8536 }, { 0.0360, 0.9145, 0.4029 } });

            Console.WriteLine(M.Inverse());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractTest
{
    class Class2 : Class1
    {
        public Class2(int i)
        {

        }

        public override void Draw()
        {
            Console.WriteLine("DESSIN");
        }

        public override void Calibrate()
        {
            Console.WriteLine("Pas de calibration");
        }
    }
}

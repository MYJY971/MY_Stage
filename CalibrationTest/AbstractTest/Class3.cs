using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractTest
{
    class Class3 : Class1
    {
        public override void Draw()
        {
            Console.WriteLine("Autre dessin");
        }

        public override void Calibrate()
        {
            Console.WriteLine("Pas de calibration");
        }
    }
}

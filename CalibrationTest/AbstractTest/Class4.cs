using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractTest
{
    class Class4 : Class2
    {
        public Class4(int i)
            :base(i)
        {

        }

        public override void Calibrate()
        {
            Console.WriteLine("Calibration !");
        }


    }
}

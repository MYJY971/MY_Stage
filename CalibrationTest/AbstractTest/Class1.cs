using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractTest
{
    public abstract class Class1
    {
        public abstract void Draw();
        public abstract void Calibrate();
        public void Control()
        {
            Console.WriteLine("Nan");
        }

    }
}

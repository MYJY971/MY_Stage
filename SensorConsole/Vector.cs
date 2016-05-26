using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorConsole
{
    class Vector
    {
        public float x, y, z;

        public Vector(float a, float b, float c)
        {
            this.x = a;
            this.y = b;
            this.z = c;
        }

        public Vector()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
        }

        public String getString()
        {
            String s = "(" + x + "," + y + "," + z + ")";
            return s;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testMatrice
{
    class Program
    {
        static void Main(string[] args)
        {
            double angle = Math.PI * -90 / 180.0;///Math.PI / 2;

            Vector _target0 = new Vector(0, 0, 0);
            Vector _target = new Vector(0, 0, 0);
            Vector _eye = new Vector(15, 0, 0);
            Vector _up0 = new Vector(0, 0, 1);
            Vector _up = new Vector(0, 0, 1);
            Vector tmp = new Vector(0, 0, 0);

            RotationMatrix _matRot = new RotationMatrix(2, angle);
            tmp = _target0.Translate(-1, _matRot.ProductMat(_eye));
            _target = tmp.Translate(_eye);
            Console.WriteLine(_target);

            angle += angle;
            _matRot.Update(angle);
            

            tmp = _target0.Translate(-1, _matRot.ProductMat(_eye));
            _target = tmp.Translate(_eye);
            Console.WriteLine(_target);

            //Console.WriteLine(_matRot.ProductMat(new Vector(0,-15,0)));

            //newTarget = newTarget.Translate(-1, _matRot.ProductMat(_eye)).Translate(_eye);



            // Keep the console window open in debug mode.
            Console.ReadKey();

        }
    }
}

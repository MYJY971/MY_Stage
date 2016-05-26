using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorConsole
{
    
    class Matrix
    {
        float[,] _values;
        int _size;
        int _row, _col;
        float _x,_y,_z,_w;
        float _a, _b, _c, _d, _e, _f, _g, _h, _i;


        public Matrix(int row, int col)
        {
            initRowColSize(row,col);

            this._values = new float[_row, _col];
        }

        public Matrix(int row, int col, float[] values)
        {
            initRowColSize(row, col);

            for (int r=0; r<_row; ++r )
            {
                for(int c=0; c<_col; ++c)
                {
                    _values[r, c] = values[r * _row + c];
                }
            }
        }

        public Matrix(float a, float b, float c, float d, float e, float f, float g, float h, float i)
        {

            /*_values[0, 0] = a; _values[0, 1] = b; _values[0, 2] = c;
            _values[1, 0] = d; _values[1, 1] = e; _values[1, 2] = f;
            _values[2, 0] = g; _values[2, 1] = h; _values[2, 2] = i;*/

            this._a = a;
            this._b = b;
            this._c = c;
            this._d = d;
            this._e = e;
            this._f = f;
            this._g = g;
            this._h = h;
            this._i = i;
            

        }

        private void initRowColSize (int row, int col)
        {
            this._row = row;
            this._col = col;
            this._size = this._row * this._col;

        }

        public String getString ()
        {
            String s = "{ {0,8:0.000} , {0,8:0.000} , {0,8:0.000} }";
            return s;

        }

        public Vector productVect(Vector v)
        {
            Vector res = new Vector(0,0,0);
            /*res.x = this._values[0, 0] * v.x + this._values[0, 1] * v.y + this._values[0, 2];
            res.y = this._values[1, 0] * v.x + this._values[1, 1] * v.y + this._values[1, 2];
            res.z = this._values[2, 0] * v.x + this._values[2, 1] * v.y + this._values[2, 2];*/

            res.x = _a * v.x + _b * v.y + _c * v.z;
            res.y = _d * v.x + _e * v.y + _f * v.z;
            res.z = _g * v.x + _h * v.y + _i * v.z;

            return res;
        }

        
       
    }
}

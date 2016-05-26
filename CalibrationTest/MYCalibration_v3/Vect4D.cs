using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationLibrary
{
    public struct Vect4D //OpenTK Vector4
    {
        #region Fields

        /// <summary>
        /// The X component of the Vector4.
        /// </summary>
        public float X;

        /// <summary>
        /// The Y component of the Vector4.
        /// </summary>
        public float Y;

        /// <summary>
        /// The Z component of the Vector4.
        /// </summary>
        public float Z;

        /// <summary>
        /// The W component of the Vector4.
        /// </summary>
        public float W;

        /// <summary>
        /// Defines a unit-length Vector4 that points towards the X-axis.
        /// </summary>
        public static Vect4D UnitX = new Vect4D(1, 0, 0, 0);

        /// <summary>
        /// Defines a unit-length Vector4 that points towards the Y-axis.
        /// </summary>
        public static Vect4D UnitY = new Vect4D(0, 1, 0, 0);

        /// <summary>
        /// Defines a unit-length Vector4 that points towards the Z-axis.
        /// </summary>
        public static Vect4D UnitZ = new Vect4D(0, 0, 1, 0);

        /// <summary>
        /// Defines a unit-length Vector4 that points towards the W-axis.
        /// </summary>
        public static Vect4D UnitW = new Vect4D(0, 0, 0, 1);

        /// <summary>
        /// Defines a zero-length Vector4.
        /// </summary>
        public static Vect4D Zero = new Vect4D(0, 0, 0, 0);

        /// <summary>
        /// Defines an instance with all components set to 1.
        /// </summary>
        public static readonly Vect4D One = new Vect4D(1, 1, 1, 1);


        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="value">The value that will initialize this instance.</param>
        public Vect4D(float value)
        {
            X = value;
            Y = value;
            Z = value;
            W = value;
        }

        /// <summary>
        /// Constructs a new Vector4.
        /// </summary>
        /// <param name="x">The x component of the Vector4.</param>
        /// <param name="y">The y component of the Vector4.</param>
        /// <param name="z">The z component of the Vector4.</param>
        /// <param name="w">The w component of the Vector4.</param>
        public Vect4D(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }



        /// <summary>
        /// Constructs a new Vector4 from the given Vector3.
        /// The w component is initialized to 0.
        /// </summary>
        /// <param name="v">The Vector3 to copy components from.</param>
        /// <remarks><seealso cref="Vector4(Vector3, float)"/></remarks>
        public Vect4D(Vect3D v)
        {
            X = (float)v.X;
            Y = (float)v.Y;
            Z = (float)v.Z;
            W = 0.0f;
        }

        /// <summary>
        /// Constructs a new Vector4 from the specified Vector3 and w component.
        /// </summary>
        /// <param name="v">The Vector3 to copy components from.</param>
        /// <param name="w">The w component of the new Vector4.</param>
        public Vect4D(Vect3D v, float w)
        {
            X = (float)v.X;
            Y = (float)v.Y;
            Z = (float)v.Z;
            W = w;
        }

        /// <summary>
        /// Constructs a new Vector4 from the given Vector4.
        /// </summary>
        /// <param name="v">The Vector4 to copy components from.</param>
        public Vect4D(Vect4D v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
            W = v.W;
        }

        #endregion

        #region Transform

        /// <summary>Transform a Vector by the given Matrix</summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <returns>The transformed vector</returns>
        public static Vect4D Transform(Vect4D vec, Mat4 mat)
        {
            Vect4D result;
            Transform(ref vec, ref mat, out result);
            return result;
        }

        /// <summary>Transform a Vector by the given Matrix</summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <param name="result">The transformed vector</param>
        public static void Transform(ref Vect4D vec, ref Mat4 mat, out Vect4D result)
        {
            result = new Vect4D(
                vec.X * mat.Row0.X + vec.Y * mat.Row1.X + vec.Z * mat.Row2.X + vec.W * mat.Row3.X,
                vec.X * mat.Row0.Y + vec.Y * mat.Row1.Y + vec.Z * mat.Row2.Y + vec.W * mat.Row3.Y,
                vec.X * mat.Row0.Z + vec.Y * mat.Row1.Z + vec.Z * mat.Row2.Z + vec.W * mat.Row3.Z,
                vec.X * mat.Row0.W + vec.Y * mat.Row1.W + vec.Z * mat.Row2.W + vec.W * mat.Row3.W);
        }



        #endregion

        #region Operators
        /// <summary>
        /// Divides an instance by a scalar.
        /// </summary>
        /// <param name="vec">The instance.</param>
        /// <param name="scale">The scalar.</param>
        /// <returns>The result of the calculation.</returns>
        public static Vect4D operator /(Vect4D vec, float scale)
        {
            float mult = 1.0f / scale;
            vec.X *= mult;
            vec.Y *= mult;
            vec.Z *= mult;
            vec.W *= mult;
            return vec;
        }

        #endregion

    }
}

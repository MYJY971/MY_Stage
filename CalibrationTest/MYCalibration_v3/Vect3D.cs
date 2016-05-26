using System;

//using Logyline.Math.Geometry2D;
using System.Xml;
//using Logyline.Common.IO.XmlSerialisation;
using System.Xml.Schema;
using System.Xml.Serialization;
using OpenTK;

namespace CalibrationLibrary//Logyline.Math.Geometry3D
{
	/// <summary>
	/// Summary description for Class.
	/// </summary>
	/// <summary>
	/// Summary description for Vect3D.
	/// </summary>
	public struct Vect3D: IXmlSerializable
	{
		public double x, y, z;

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        public double Z
        {
            get { return z; }
            set { z = value; }
        }

        public Vect3D(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        /*public Vect3D(Vect2D pt2D)
        {
            this.x = pt2D.x;
            this.y = pt2D.y;
            this.z = 0;
        }*/
        /*public static Vect3D FromVect2D(Vect2D vect2, double z)
        {
            Vect3D result = new Vect3D(vect2.x, vect2.y, z);
            return result;
        }*/

		public double this[int index]
		{
			get{
				double l_value=0;
				switch(index)
				{
					case 0: l_value=x; break;
					case 1: l_value=y; break;
					case 2: l_value=z; break;
					default: throw new IndexOutOfRangeException("impossible de récupererla composante de rang "+index+" dans l'objet Vect3D");
				}
				return l_value;
			}

			set{
				switch(index)
				{
					case 0: x=value; break;
					case 1: y=value; break;
					case 2: z=value; break;
					default: throw new IndexOutOfRangeException("impossible d'initialiser la composante de rang "+index+" dans l'objet Vect3D");
				}
			}
		}

		/*public bool Equals(Vect3D value)
		{
			 return (System.Math.Abs(this.x-value.x)<MathTools.LOW_EPS)
				 && (System.Math.Abs(this.y-value.y)<MathTools.LOW_EPS)
				 && (System.Math.Abs(this.z-value.z)<MathTools.LOW_EPS);
		}*/

        public bool Equals(Vect3D vect2, double distEps)
        {
            return Vect3D.Dist(this, vect2) <= distEps;
        }

		public static bool Equals(Vect3D vect1, Vect3D vect2, double distEps)
		{
            return vect1.Equals(vect2, distEps);
		}

        public static bool Equals(Vect3D vect1, Vect3D vect2)
        {
            return vect1.Equals(vect2);
        }

        public bool IsColinear(Vect3D vect2)
        {
            return AreColinear(this, vect2);
        }

        public static bool AreColinear(Vect3D vect1, Vect3D vect2)
        {
            vect1.Normalize();
            vect2.Normalize();
            return vect1.Equals(vect2) || vect1.Equals(Vect3D.Multiply(vect2, -1.0));
        }

		public override string ToString()
		{
			return String.Format("[x={0}, y={1}, z={2}]", System.Math.Round(x, 3), 
															System.Math.Round(y, 3),
															System.Math.Round(z, 3));
		}

        public string Str
        {
            get { return "x=" + x.ToString() + " y=" + y.ToString() + " z=" + z.ToString(); }
        }
		
		public static Vect3D Vector3D(Vect3D v1, Vect3D v3)
		{
            Vect3D l_result = new Vect3D();
			l_result.x = v3.x - v1.x;
			l_result.y = v3.y - v1.y;
			l_result.z = v3.z - v1.z;
			return l_result;
		}

		public static Vect3D NULL_VECTOR
		{
			get{
				return new Vect3D(0, 0, 0);
			}
		}

        public static Vect3D NOT_ASSIGNED_VECTOR
        {
            get { return new Vect3D(Double.NaN, Double.NaN, Double.NaN); }
        }

        public static Vect3D X_VECTOR
        {
            get
            {
                return new Vect3D(1, 0, 0);
            }
        }

        public static Vect3D X_VECTOR_INV
        {
            get
            {
                return new Vect3D(-1, 0, 0);
            }
        }

        public static Vect3D Y_VECTOR
        {
            get
            {
                return new Vect3D(0, 1, 0);
            }
        }

        public static Vect3D Y_VECTOR_INV
        {
            get
            {
                return new Vect3D(0, -1, 0);
            }
        }

        public static Vect3D Z_VECTOR
        {
            get
            {
                return new Vect3D(0, 0, 1);
            }
        }


        public static Vect3D Z_VECTOR_INV
        {
            get
            {
                return new Vect3D(0, 0, -1);
            }
        }

        public bool IsAssignedVector
        {
            get { return !(double.IsNaN(x) || double.IsNaN(y) || double.IsNaN(z)); }
        }
        public bool IsInfinityVector
        {
            get { return (double.IsInfinity(x) || double.IsInfinity(y) || double.IsInfinity(z)); }
        }
        public bool IsValidVector
        {
            get { return IsAssignedVector && !IsInfinityVector; }
        }

		/*public Vect2D VectorXY
		{
			get { return new Vect2D(x, y); }
		}*/


        public Vector3 ToVector3()
        {
            return new Vector3((float)this.x, (float)this.y, (float)this.z);
        }

        public Vector2 ToVecor2()
        {
            return new Vector2((float)this.x, (float)this.y);
        }
 
		public double[] GetArrayValues()
		{
			double [] l_values = new double[3];
			l_values[0] = x;
			l_values[1] = y;
			l_values[2] = z;
			return l_values;
		}

		public void Multiply(double p_scalar)
		{
			this.x = this.x * p_scalar;
			this.y = this.y * p_scalar;
			this.z = this.z * p_scalar;
		}

		public static Vect3D Multiply(Vect3D p_vect, double p_scalar)
		{
            Vect3D l_result = new Vect3D();
			l_result.x = p_vect.x * p_scalar;
			l_result.y = p_vect.y * p_scalar;
			l_result.z = p_vect.z * p_scalar;
			return l_result;
		}

        public static Vect3D Multiply(Vect3D p_vect, double coefx, double coefy, double coefz)
        {
            Vect3D l_result = new Vect3D();
            l_result.x = p_vect.x * coefx;
            l_result.y = p_vect.y * coefy;
            l_result.z = p_vect.z * coefz;
            return l_result;
        }

        public static Vect3D Multiply(Vect3D p_vect, Vect3D vectCoef)
        {
            Vect3D l_result = new Vect3D();
            l_result.x = p_vect.x * vectCoef.x;
            l_result.y = p_vect.y * vectCoef.y;
            l_result.z = p_vect.z * vectCoef.z;
            return l_result;
        }


        public void Move(Vect3D p_dir, double p_dist)
        {
            Vect3D l_dir = p_dir;
            l_dir.Normalize();
            l_dir.Multiply(p_dist);
            x += l_dir[0];
            y += l_dir[1];
            z += l_dir[2];
        }

        public static Vect3D Move(Vect3D p_point, Vect3D p_dir, double p_dist)
        {
            Vect3D l_dir = p_dir;
            l_dir.Normalize();
            l_dir.Multiply(p_dist);
            return Sum(p_point, l_dir);
        }

		public static Vect3D Bary(Vect3D [] vects)
		{
			Vect3D result = Vect3D.NULL_VECTOR;
			foreach (Vect3D l_vect in  vects) {
				result.x = l_vect.x+result.x;
				result.y = l_vect.y+result.y;
				result.z = l_vect.z+result.z;
			}
			result.x = result.x / vects.Length;
			result.y = result.y / vects.Length;
			result.z = result.z / vects.Length;
			return result;
		}

		public static Vect3D Bary(Vect3D vect1, Vect3D vect2)
		{
            Vect3D result = new Vect3D();
			result.x = (vect1.x + vect2.x) / 2.0;
			result.y = (vect1.y + vect2.y) / 2.0;
			result.z = (vect1.z + vect2.z) / 2.0;
			return result;
		}

        public static Vect3D Bary(Vect3D vect1, Vect3D vect2, Vect3D vect3)
        {
            Vect3D result = new Vect3D();
            result.x = (vect1.x + vect2.x + vect3.x) / 3.0;
            result.y = (vect1.y + vect2.y + vect3.y) / 3.0;
            result.z = (vect1.z + vect2.z + vect3.z) / 3.0;
            return result;
        }

		public static Vect3D Sum(Vect3D p_vect1, Vect3D p_vect3)
		{
            Vect3D l_result = new Vect3D();
			l_result.x = (p_vect1.x + p_vect3.x);
			l_result.y = (p_vect1.y + p_vect3.y);
			l_result.z = (p_vect1.z + p_vect3.z);
			return l_result;
		}

		public void Normalize()
		{
			double l_norm;
			l_norm = Norm();
			if (l_norm > 0) {
				x =x / l_norm;
				y =y / l_norm;
				z =z / l_norm;
			}
		}

		public static double Dist(Vect3D p1, Vect3D p3)
		{
			return Norm(Vect3D.Vector3D(p1, p3));
		}

		public double Dist(Vect3D p)
		{
			return Norm(Vect3D.Vector3D(this, p));
		}

		public static double SquareDist(Vect3D p1, Vect3D p3)
		{
			return Norm(Vect3D.Vector3D(p1, p3));
		}

		/*public static double DistPointToLine(Vect3D P, Vect3D M1, Vect3D M3)
		{
			Vect3D u, v;
			v.x = M3.x - M1.x;
			u.x = P.x  - M1.x;
			v.y = M3.y - M1.y;
			u.y = P.y  - M1.y;

			v.Normalize();
			return Det(u,v);
		}*/


		/// <summary>
		/// retourne le produit scalaire entre deux vecteur 3D.
		/// </summary>
		public static double ScalarProduct (Vect3D u, Vect3D v)
		{
			return (u.x*v.x + u.y*v.y + u.z*v.z);
		}

		/// <summary>
		/// retourne le produit vectoriel entre deux vecteur 3D.
		/// </summary>
        public static Vect3D VectorialProduct(Vect3D u, Vect3D v)
        {
            return new Vect3D(u.y * v.z - u.z * v.y,
                             u.z * v.x - u.x * v.z,
                             u.x * v.y - u.y * v.x);

        }

		/*public static Vect3D VectorialProduct(double[] u, double [] p_v)
		{
			if (u.Length != 3 || p_v.Length != 3)
				throw new Exception("u and p_v must be dimension 3");
			else
				return new Vect3D(u[1] * p_v[2] - u[2] * p_v[1],
							 u[2] * p_v[0] - u[0] * p_v[2],
							 u[0] * p_v[1] - u[1] * p_v[0]);

		}*/

		public double Norm()
		{
			return System.Math.Sqrt(x*x + y*y + z*z);
		}

		public double SquareNorm()
		{
			return (x*x + y*y + z*z);
		}

		public static double Norm(Vect3D v)
		{
			return v.Norm();
		}
		public static double SquareNorm(Vect3D v)
		{
			return v.SquareNorm();   
		}

        /*public static double Angle3D(Vect3D u, Vect3D v)
        {
            double l_angle, deno = ScalarProduct(u, v);
            Vect3D n = VectorialProduct(u, v);
            if (System.Math.Abs(deno) > MathTools.LOW_EPS)
                l_angle = System.Math.Atan(n.Norm() / deno);
            else l_angle = System.Math.PI / 2;

            if (deno < 0)
            {
                if (l_angle > 0)
                    l_angle = l_angle - System.Math.PI;
                else
                    l_angle = l_angle + System.Math.PI;
            }
            return l_angle;
        }*/
        /*public static double Angle3D_bis(Vect3D u, Vect3D v)
		{
            double angle;
			Vect3D or = NULL_VECTOR;
            u.Normalize();
            v.Normalize();
            if (u.Equals(or) || v.Equals(or)) return 0.0;
            if (!MathTools.LineAreEquals(or, u, or, v))
            {
                Vect3D axeZ2 = VectorialProduct(u, v); axeZ2.Normalize();
                Vect3D axeY2 = VectorialProduct(axeZ2, u); axeY2.Normalize();
                // on recalcul v dans le nouveau repere dont u correspond a l'axe des abscisse
                Vect3D u2 = MathTools.ChangementRepere(or, u, axeY2, axeZ2, u);
                Vect3D v2 = MathTools.ChangementRepere(or, u, axeY2, axeZ2, v);
                // la composante z de V2 est donc automatique nulle
                angle = MathTools.Angle2D(u2.VectorXY, v2.VectorXY);
            }
            else 
            {
                if (u.Equals(v)) angle = 0.0;
                else angle = System.Math.PI;
            }
            
            return angle;
		}*/

        #region operators
		public static Vect3D operator +(Vect3D a, Vect3D b)
		{
			return Vect3D.Sum(a, b);
		}
		public static Vect3D operator -(Vect3D a, Vect3D b)
		{
            Vect3D result = new Vect3D();
            result.x = (a.x - b.x);
            result.y = (a.y - b.y);
            result.z = (a.z - b.z);
            return result;
		}

		public static Vect3D operator *(Vect3D a, double b)
		{
			return Vect3D.Multiply(a, b);
		}
		public static Vect3D operator /(Vect3D a, double b)
		{
			return Vect3D.Multiply(a, 1/b);
		}
        #endregion

        #region IMPLEMENTATION DE IXMLSERIALISABLE
        public XmlSchema GetSchema()
        {
            return null;
        }
        public void ReadXml(XmlReader reader)
        {
            //XmlSerialisationTools.ReadStartElementFromTypeName(reader, this);

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                if (reader.Name == "x")
                {
                    x = double.Parse(reader.ReadElementString("x"));
                }
                else if (reader.Name == "y")
                {
                    y = double.Parse(reader.ReadElementString("y"));
                }
                else if (reader.Name == "z")
                {
                    z = double.Parse(reader.ReadElementString("z"));
                }
                else reader.Skip();
            }

            reader.ReadEndElement();
        }
        public void WriteXml(XmlWriter writer)
        {
            //XmlSerialisationTools.WriteStartElementFromURIAndTypeName(writer, this);
            writer.WriteElementString("x", x.ToString());
            writer.WriteElementString("y", y.ToString());
            writer.WriteElementString("z", z.ToString());
            writer.WriteEndElement();
        }
        #endregion

        //Ajout //////////////////////////////////////////////

        /// <summary>
        /// Calcul le produit scalaire
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static float Dot(Vect3D left, Vect3D right)
        {
            return (float)(left.x * right.x + left.y * right.y + left.z * right.z);
        }

        /// <summary>
        /// Contruit un nouveau vecteur3 à partir d'un vecteur4
        /// </summary>
        /// <param name="v"></param>
        public Vect3D(Vect4D v)
        {
            x = v.X;
            y = v.Y;
            z = v.Z;
        }

        public static Vect3D Transform(Vect3D vec, Mat4 mat)
        {
            Vect3D v;
            v.x = Vect3D.Dot(vec, new Vect3D(mat.Column0));
            v.y = Vect3D.Dot(vec, new Vect3D(mat.Column1));
            v.z = Vect3D.Dot(vec, new Vect3D(mat.Column2));
            return v;
        }

        #region Cross

        /// <summary>
        /// Caclulate the cross (vector) product of two vectors
        /// </summary>
        /// <param name="left">First operand</param>
        /// <param name="right">Second operand</param>
        /// <returns>The cross product of the two inputs</returns>
        public static Vect3D Cross(Vect3D left, Vect3D right)
        {
            Vect3D result;
            Cross(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Caclulate the cross (vector) product of two vectors
        /// </summary>
        /// <param name="left">First operand</param>
        /// <param name="right">Second operand</param>
        /// <returns>The cross product of the two inputs</returns>
        /// <param name="result">The cross product of the two inputs</param>
        public static void Cross(ref Vect3D left, ref Vect3D right, out Vect3D result)
        {
            result = new Vect3D(left.Y * right.Z - left.Z * right.Y,
                left.Z * right.X - left.X * right.Z,
                left.X * right.Y - left.Y * right.X);
        }

        #endregion

        public float Length
        {
            get
            {
                return (float)System.Math.Sqrt(X * X + Y * Y + Z * Z);
            }
        }

        public static Vect3D Normalize(Vect3D vec)
        {
            float scale = 1.0f / vec.Length;
            vec.X *= scale;
            vec.Y *= scale;
            vec.Z *= scale;
            return vec;
        }

        public static Vect3D operator -(Vect3D vec)
        {
            vec.X = -vec.X;
            vec.Y = -vec.Y;
            vec.Z = -vec.Z;
            return vec;
        }


        //pour OpenTK
        public static Vect3D ToVect3D (Vector3 vect)
        {
            return new Vect3D((double)vect.X, (double)vect.Y, (double)vect.Z);
        }


        //////////////////////////////////////////////
    }
}

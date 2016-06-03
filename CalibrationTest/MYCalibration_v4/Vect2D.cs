using System;

//using Logyline.Math.Geometry2D;
using System.Xml;
//using Logyline.Common.IO.XmlSerialisation;
using System.Xml.Schema;
using System.Xml.Serialization;
using OpenTK;

namespace CalibrationLibrary
{
	/// <summary>
	/// Summary description for Class.
	/// </summary>
	/// <summary>
	/// Summary description for Vect3D.
	/// </summary>
	public struct Vect2D: IXmlSerializable
	{
		public double x, y;

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

       
        public Vect2D(double x, double y)
        {
            this.x = x;
            this.y = y;
            
        }
        public Vect2D(Vect2D pt2D)
        {
            this.x = pt2D.x;
            this.y = pt2D.y;
            
        }

        //pour OpenTK
        public static Vect2D ToVect2D(Vector2 vect)
        {
            return new Vect2D((double)vect.X, (double)vect.Y);
        }

        public Vector2 ToVecor2()
        {
            return new Vector2((float)this.x, (float)this.y);
        }

        

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
                
                else reader.Skip();
            }

            reader.ReadEndElement();
        }
        public void WriteXml(XmlWriter writer)
        {
            //XmlSerialisationTools.WriteStartElementFromURIAndTypeName(writer, this);
            writer.WriteElementString("x", x.ToString());
            writer.WriteElementString("y", y.ToString());
            writer.WriteEndElement();
        }
        #endregion
    }
}

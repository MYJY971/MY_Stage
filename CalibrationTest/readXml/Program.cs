using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace readXml
{
    class Program
    {
        static void Main(string[] args)
        {
            double north;
            float pitch;
            float roll;
            float yaw;
            string northval;
            XDocument docSensor = XDocument.Load("photo_13-5-2016_10-29-21-372.xml");
            //IEnumerable<XElement> sensors = docSensor.Elements();

            //var sensor = sensors.ElementAt(0);


            //northval = sensor.Element("compass").Element("north").Value;
            northval = docSensor.Element("sensor").Element("compass").Element("north").Value;

            Console.WriteLine(northval);
            north = Convert.ToDouble(northval.Replace(".", ","));
            Console.WriteLine(north);


            Console.ReadKey();
        }
    }
}

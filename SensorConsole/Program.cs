using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.UI.Core;
using Windows.Devices.Sensors;

//using CSML;

namespace SensorConsole
{
    class Program
    {
        float _M11 = 100;
        /*private async void ReadingChanged(object sender, OrientationSensorReadingChangedEventArgs e)
        {
            OrientationSensorReading reading = e.Reading;
            
            
        }*/


        static void Main()
        {
            
            Console.WriteLine("Hello World");


            OrientationSensor sensor = OrientationSensor.GetDefault();

            Compass compass = Compass.GetDefault();


            if (sensor != null)
            {
                Console.WriteLine("Capteur détécté");
                // Establish the report interval
                uint minReportInterval = sensor.MinimumReportInterval;
                uint reportInterval = minReportInterval > 16 ? minReportInterval : 16;
                sensor.ReportInterval = reportInterval;

                // Assign an event handler for the reading-changed event
                 sensor.ReadingChanged += (OrientationSensor sender, OrientationSensorReadingChangedEventArgs args) =>
                 {
                    //Console.WriteLine(args.Reading.RotationMatrix.M11); 
                 };

               // sensor.ReadingChanged += new TypedEventHandler<OrientationSensor, OrientationSensorReadingChangedEventArgs>(ReadingChanged);

                

            }
            else
            {
                Console.WriteLine("pas de capteur d'orientation détécté");
    
            }

            if (compass != null)
            {
                Console.WriteLine("Boussole détécté");
                // Establish the report interval
                uint minReportInterval = compass.MinimumReportInterval;
                uint reportInterval = minReportInterval > 16 ? minReportInterval : 16;
                compass.ReportInterval = reportInterval;

                // Assign an event handler for the reading-changed event
                compass.ReadingChanged += (Compass sender, CompassReadingChangedEventArgs args) =>
                {
                    Console.WriteLine(args.Reading.HeadingMagneticNorth);
                };

                // sensor.ReadingChanged += new TypedEventHandler<OrientationSensor, OrientationSensorReadingChangedEventArgs>(ReadingChanged);



            }
            else
            {
                Console.WriteLine("pas de boussole détécté");

            }




            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}

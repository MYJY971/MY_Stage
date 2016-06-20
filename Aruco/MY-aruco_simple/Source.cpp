#include <iostream>
#include "aruco.h"
#include "cvdrawingutils.h"
#include <opencv2\highgui\highgui.hpp>
#include <fstream>

using namespace std;
using namespace cv;
using namespace aruco;


//class for parsing command line
//operator [](string cmd) return  whether cmd is present //string operator ()(string cmd) return the value as a string: -cmd value
class CmdLineParser 
{ 
	int argc;
	char **argv;

public: CmdLineParser(int _argc, char **_argv) :argc(_argc), argv(_argv) {}
			bool operator[] (string param) 
			{
				int idx = -1;
				for (int i = 0; i < argc && idx == -1; i++) 
					if (string(argv[i]) == param) idx = i;
				return (idx != -1);
			}
			string operator()(string param, string defvalue = "-1") 
			{
				int idx = -1;
				for (int i = 0; i < argc && idx == -1; i++)
					if (string(argv[i]) == param) idx = i;
				if (idx == -1) return defvalue;
				else  return (argv[idx + 1]); 
			}
};

int main(int argc, char **argv) {

	string image_path = "../aruco-test-data-2.0/1_single/image-test" ;
	string video_path = "../aruco-test-data-2.0/1_single/video.avi";
	string camPam_path = "../aruco-test-data-2.0/1_single/intrinsics.yml";


	
	try {
		CmdLineParser cml(argc, argv);
		if (argc == 1 || cml["-h"]) {
			cerr << "Usage: (in_image|video.avi) [-c cameraParams.yml] [-s markerSize] [-d <dicionary>:ARUCO default] [-o <outImage>] " << endl;
			cerr << "\tDictionaries: "; for (auto dict : aruco::Dictionary::getDicTypes())    cerr << dict << " "; cerr << endl;

			cout << "Example to work with apriltags dictionary : video.avi -d TAG36h11" << endl << endl;
			return 0;
		}


		aruco::CameraParameters CamParam;

		// read the input image
		cv::Mat InImage;
		// Open input and read image
		VideoCapture vreader(video_path);
		if (vreader.isOpened()) vreader >> InImage;
		else { cerr << "Could not open input" << endl; return -1; }
		// read camera parameters if specifed
		if (cml["-c"])  CamParam.readFromXMLFile(camPam_path);
		// read marker size if specified (default value -1)
		float MarkerSize = std::stof(cml("-s", "-1"));
		//Create the detector
		MarkerDetector MDetector;
		MDetector.setThresholdParams(7, 7);
		MDetector.setThresholdParamRange(2, 0);
		//Set the dictionary you want to work with, if you included option -d in command line
		//see dictionary.h for all types
		if (cml["-d"])  //if the -d is in the command line
			MDetector.setDictionary(cml("-d"), 0.f);//cml("-d") return the string after -d in the command line "example: ./program video.avi -d ARUCO", then, returns the string "ARUCO"

													// Ok, let's detect
		vector< Marker >  Markers = MDetector.detect(InImage, CamParam, MarkerSize);

		// for each marker, draw info and its boundaries in the image
		for (unsigned int i = 0; i < Markers.size(); i++) {
			cout << Markers[i] << endl;
			Markers[i].draw(InImage, Scalar(0, 0, 255), 2);
		}
		// draw a 3d cube in each marker if there is 3d info
		if (CamParam.isValid() && MarkerSize != -1)
			for (unsigned int i = 0; i < Markers.size(); i++) {
				CvDrawingUtils::draw3dCube(InImage, Markers[i], CamParam);
			}
		// show input with augmented information
		cv::namedWindow("in", 1);
		cv::imshow("in", InImage);
		while (char(cv::waitKey(0)) != 27); // wait for esc to be pressed


		if (cml["-o"]) cv::imwrite(cml("-o"), InImage);



	}
	catch (std::exception &ex)

	{
		cout << "Exception :" << ex.what() << endl;
		cout << ">_<!" << endl;
	}

}
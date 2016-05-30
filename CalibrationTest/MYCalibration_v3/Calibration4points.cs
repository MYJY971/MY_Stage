using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using openCV;
using System.Xml.Linq;

namespace CalibrationLibrary
{
    public class Calibration4points
    {
        
        private double _fx;
        private double _fy;
        private double _k1;
        private double _k2;
        private double _k3;
        private double _p1;
        private double _p2;

        public Vect3D _eyePosition; //= new Vector3(-10.0f, 0.0f, 0.0f);
        public Vect3D _targetPosition;// = Vector3.Zero;
        public Vect3D _upVector;// = new Vector3(0.0f, 0.0f, 1.0f);

        private double[,] _camIntParams = new double[3, 3];
        private double[] _distorsionCoef = new double[4];

        public List<Vect2D> _listImagePoints = new List<Vect2D>();
        public List<Vect3D> _listObjectPoints = new List<Vect3D>();

        private int _width; //largeur fenêtre
        private int _height; //hauteur fenêtre

        public double[] _glProjectionMatrix;
        public Mat4 _modelView;

        private String _calibrationFilePath;

        #region Constructeurs
        public Calibration4points(int width, int height, String path, List<Vect2D> listImagePoints, List<Vect3D> listObjectPoints, Vect3D eye, Vect3D target, Vect3D up)
        {
            this._width = width;
            this._height = height;
            this._calibrationFilePath = path;
            this._listImagePoints = listImagePoints;
            this._listObjectPoints = listObjectPoints;
            this._eyePosition = eye;
            this._targetPosition = target;
            this._upVector = up;
        }

        public Calibration4points(int width, int height, Vect3D eye, Vect3D target, Vect3D up)
        {
            this._width = width;
            this._height = height;
            this._eyePosition = eye;
            this._targetPosition = target;
            this._upVector = up;
        }

        public Calibration4points(int width, int height)
        {
            this._width = width;
            this._height = height;

        }

        public Calibration4points()
        {

        }

        #endregion

        #region Setters
        public void SetCalibrationFilePath(String path)
        {
            _calibrationFilePath = path;
        }

        public void SetListImagePoints(List<Vect2D> listImagePoints)
        {
            this._listImagePoints = listImagePoints;
        }

        public void SetListObjectPoints(List<Vect3D> listObjectPoints)
        {
            this._listObjectPoints = listObjectPoints;
        }

        #endregion

        #region Getters
        public Mat4 GetModelView()
        {
            return _modelView;
        }
        #endregion

        #region Recupérer les paramètres intresèques de la caméra
        private void GetCamParams(String path)
        {
            XDocument calibration = XDocument.Load(path);

            _fx = Convert.ToDouble(calibration.Element("calibration").Element("fx").Value.Replace(".", ","));
            _fy = Convert.ToDouble(calibration.Element("calibration").Element("fy").Value.Replace(".", ","));

            _k1 = Convert.ToDouble(calibration.Element("calibration").Element("k1").Value.Replace(".", ","));
            _k2 = Convert.ToDouble(calibration.Element("calibration").Element("k2").Value.Replace(".", ","));
            _k3 = Convert.ToDouble(calibration.Element("calibration").Element("k3").Value.Replace(".", ","));

            _p1 = Convert.ToDouble(calibration.Element("calibration").Element("p1").Value.Replace(".", ","));
            _p2 = Convert.ToDouble(calibration.Element("calibration").Element("p2").Value.Replace(".", ","));




            _camIntParams = new double[3, 3] { { _fx,      0,      _width /2.0},
                                               {   0,    _fy,      _height /2.0},
                                               {   0,      0,                   1} };

            _distorsionCoef = new double[] { _k1, _k2, _p1, _p2 };


        }

        #endregion

        #region Compute Pose
        private void ComputePose(List<Vect3D> objectPoints, List<Vect2D> imagePoints, double[,] camMatrix/*, bool useDistCoeffs*/, double[] distorsionCoef, out double[] transVect, out double[] rvec, out double[,] rotMat/*, out double reprojectionErrorPx, bool updateTAndR*/, out CvMat tvec_copy, out CvMat cvrvec_copy)
        {
            int point_count = 4;

            rvec = new double[3];
            transVect = new double[3];
            rotMat = new double[3, 3];

            CvMat objPoints = cvlib.CvCreateMat(3, point_count, cvlib.CV_32FC1);
            CvMat imgPoints = cvlib.CvCreateMat(2, point_count, cvlib.CV_32FC1);
            CvMat K = cvlib.CvCreateMat(3, 3, cvlib.CV_32FC1);
            CvMat cvrvec = cvlib.CvCreateMat(3, 1, cvlib.CV_32FC1);
            CvMat tvec = cvlib.CvCreateMat(3, 1, cvlib.CV_32FC1);
            CvMat distCoeff = cvlib.CvCreateMat(1, 4, cvlib.CV_32FC1);
            CvMat R = cvlib.CvCreateMat(3, 3, cvlib.CV_32FC1);

            // Initialisation des données
            // Points image
            for (int i = 0; i < point_count; i++)
            {
                cvlib.CvSetReal2D(ref imgPoints, 0, i, imagePoints[i].x);
                cvlib.CvSetReal2D(ref imgPoints, 1, i, imagePoints[i].y);
            }
            // Points objet
            for (int i = 0; i < point_count; i++)
            {
                cvlib.CvSetReal2D(ref objPoints, 0, i, objectPoints[i].x);
                cvlib.CvSetReal2D(ref objPoints, 1, i, objectPoints[i].y);
                cvlib.CvSetReal2D(ref objPoints, 2, i, objectPoints[i].z);
            }
            // Matrice de camera interne
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    cvlib.CvSetReal2D(ref K, i, j, camMatrix[i, j]);
                }
            }


            cvlib.CvFindExtrinsicCameraParams2(ref objPoints, ref imgPoints, ref K, ref distCoeff, ref cvrvec, ref tvec);


            // Copie du vecteur translation et de rotation de opencv->transVect
            for (int j = 0; j < 3; j++)
            {
                transVect[j] = cvlib.CvGetReal1D(ref tvec, j);
                rvec[j] = cvlib.CvGetReal1D(ref cvrvec, j);

            }

            CvMat jcb = cvlib.CvCreateMat(3, 9, cvlib.CV_32FC1);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    cvlib.CvSetReal2D(ref jcb, i, j, 0);
                }
            }

            // Manip StackOverFlow reference opencv opengl


            cvlib.CvSetReal1D(ref cvrvec, 0, -cvlib.CvGetReal1D(ref cvrvec, 0));
            cvlib.CvSetReal1D(ref cvrvec, 1, -cvlib.CvGetReal1D(ref cvrvec, 1));

            cvlib.CvRodrigues2(ref cvrvec, ref R, ref jcb);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    rotMat[i, j] = cvlib.CvGetReal2D(ref R, i, j);
                }
            }




            ////////////////////////////////////////////
            ///Calcul des erreurs de  reprojection de l'objet de calibration
            ///////////////////////////////////////////

            cvlib.CvSetReal1D(ref cvrvec, 0, -cvlib.CvGetReal1D(ref cvrvec, 0)); // car on a inversé avant
            cvlib.CvSetReal1D(ref cvrvec, 1, -cvlib.CvGetReal1D(ref cvrvec, 1)); // idem
            CvMat imgPointsReprojected = cvlib.CvCreateMat(2, point_count, cvlib.CV_32FC1);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    cvlib.CvSetReal2D(ref jcb, i, j, 0);
                }
            }

            cvlib.CvRodrigues2(ref cvrvec, ref R, ref jcb);


            // Calcul de la représentation en quaternions
            double[,] r = new double[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    r[i, j] = cvlib.CvGetReal2D(ref R, i, j);
                }
            }



            cvlib.CvProjectPoints2(ref objPoints, ref cvrvec, ref tvec, ref K, ref distCoeff, ref imgPointsReprojected);


            ///////////////////////////////////////////
            ///////////////////////////////////////////

            tvec_copy = cvlib.CvCreateMat(3, 1, cvlib.CV_32FC1);
            cvrvec_copy = cvlib.CvCreateMat(3, 1, cvlib.CV_32FC1);

            for (int i = 0; i < 3; i++)
            {
                cvlib.CvSetReal1D(ref tvec_copy, i, cvlib.CvGetReal1D(ref tvec, i));
                cvlib.CvSetReal1D(ref cvrvec_copy, i, cvlib.CvGetReal1D(ref cvrvec, i));
            }

        }

        #endregion

        #region Calibration
        public void CalibrateManually()
        {
            /*if (!ImagePointHasMoved) // Pour calibrer il faut bouger au moins un point image 
                return false;
            */
            if (_listImagePoints == null || _listObjectPoints == null || _listImagePoints.Count != 4 || _listObjectPoints.Count != 4)
                return ;

            /// La calibration

            //double focal = 795.734;
            //double[,] camMatrix = new double[3, 3] { { focal, 0, 319.5 }, { 0, focal, 239.5 }, { 0, 0, 1 } };

            // Matrice Camera
            //double focal = 2602.09;
            //double[,] camMatrix = new double[3, 3] { { focal, 0, 1535.5 }, { 0, focal, 2602.09 }, { 0, 0, 1 } };
            //double Axx = (double)this.Width / 640.0;
            //double Ayy = (double)this.Height / 480.0;
            //double fxx = camMatrix[0, 0] * Axx;
            //double cxx = camMatrix[0, 2] * Axx;
            //double fyy = camMatrix[1, 1] * Ayy;
            //double cyy = camMatrix[1, 2] * Ayy;

            //double[,] camIntParams = new double[3, 3] { {fxx, 0, cxx},
            //                                           {0, fyy, cyy},
            //                                           {0, 0,   1} };
            // Matrice appareil photo langlais

            GetCamParams(_calibrationFilePath);


            #region Paramètre Cam Langlais
            //_camIntParams = new double[3, 3] { {2602.09, 0,       1535.5},
            //                                            {0,       2602.09, 1151.5},
            //                                            {0,       0,       1} };

            //double k1 = -0.169965;
            //double k2 = 0.134545;
            //double p1 = 0.000688709;
            //double p2 = -0.00212149;
            //_distorsionCoef = new double[] { k1, k2, p1, p2 };
            #endregion

            #region Projet KFC
            //_camIntParams = new double[3, 3] { {1562.14, 0,       1023.5},
            //                                            {0,       1562.14, 679.5},
            //                                            {0,       0,       1} };
            //double k1 = -0.155416;
            //double k2 = 0.111837;
            //double p1 = 0.00019779;
            //double p2 = 0.00208697;
            //_distorsionCoef = new double[] { k1, k2, p1, p2 };

            #endregion


            double[,] rotMat = new double[3, 3]; // Matrice de rotation adaptée à OpenGl 
            double[] rvec = new double[3];        // Vecteur de rotation
            double[] transVect = new double[3];   // Vecteur de translation

            double w = this._width;
            double h = this._height;
            double fx = _camIntParams[0, 0];
            double fy = _camIntParams[1, 1];

            double zfar = 1000;
            double zNear = 1;

            double cx = _camIntParams[0, 2];
            double cy = _camIntParams[1, 2];


            //ref http://strawlab.org/2011/11/05/augmented-reality-with-OpenGL/
            _glProjectionMatrix = new double[16] {2*(fx/w), 0,  0,  0,
                                                        0, -2* (fy/h), 0, 0,
                                                        1-2 * (cx/w), 1-2*(cy/h), -(zfar+zNear)/(zfar-zNear), -1,
                                                        0, 0, -2* (zfar*zNear)/(zfar-zNear), 0};


            CvMat tvec_copy, cvrvec_copy;

            ComputePose(_listObjectPoints, _listImagePoints, _camIntParams, _distorsionCoef, out transVect, out rvec, out rotMat, out tvec_copy, out cvrvec_copy);
                        
            
            Mat4 modelViewTest = new Mat4((float)rotMat[0, 0], (float)rotMat[1, 0], (float)rotMat[2, 0], 0,
                                        (float)rotMat[0, 1], (float)rotMat[1, 1], (float)rotMat[2, 1], 0,
                                        (float)rotMat[0, 2], (float)rotMat[1, 2], (float)rotMat[2, 2], 0,
                                        (float)transVect[0], (float)transVect[1], (float)-transVect[2], 1);
            modelViewTest.Invert();

            Vect4D eyePos = Vect4D.Transform(new Vect4D(0, 0, 0, 1), modelViewTest);
            eyePos /= eyePos.W;

            #region transformations des paramètres extrinsèques en eyePosition, upVector, targetPosition 

            Vect3D realTransVect = new Vect3D((float)transVect[0], (float)transVect[1], -(float)transVect[2]);

            this._eyePosition = new Vect3D(eyePos.X, eyePos.Y, eyePos.Z);

            Mat4 rotatMatrixTransPosed = new Mat4((float)rotMat[0, 0], (float)rotMat[1, 0], (float)rotMat[2, 0], 0,
                                                       (float)rotMat[0, 1], (float)rotMat[1, 1], (float)rotMat[2, 1], 0,
                                                       (float)rotMat[0, 2], (float)rotMat[1, 2], (float)rotMat[2, 2], 0,
                                                       0, 0, 0, 1);
            rotatMatrixTransPosed.Transpose();

            _upVector = Vect3D.Transform(new Vect3D(0, 1, 0), rotatMatrixTransPosed);

            Vect3D ZVectorTransf = Vect3D.Transform(new Vect3D(0, 0, -1), rotatMatrixTransPosed); //on multiplie par -1 car la caméra OpenGL regarde en direction de -Z;

            Vect3D target = Vect3D.Move(new Vect3D(_eyePosition.X, _eyePosition.Y, _eyePosition.Z), new Vect3D(ZVectorTransf.X, ZVectorTransf.Y, ZVectorTransf.Z), 20);

            this._targetPosition = target;
            //this._eyePosition = new Vect3D(_eyePosition.X, _eyePosition.Y, _eyePosition.Z+1.0f);
            _modelView = Mat4.LookAt(_eyePosition, _targetPosition, _upVector);

            #endregion
        }

        #endregion

        
    }
}

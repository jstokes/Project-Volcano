using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Drawing;
using System.Drawing.Imaging;

namespace WpfApplication1
{
    class KnobControl : Canvas
    {
        private

            //Layout Objects
            System.Windows.Controls.Image _background;
            System.Windows.Controls.Image _center;
            BitmapImage                   _backgroundImage;
            BitmapImage                   _centerImage;
            System.Windows.Point          _midPoint;
            bool                          _isPressed;
            
            //Touch Control Objects
            System.Windows.Point          _touchOrigin;
            double       _touchY;
            bool         _touchRotation;
            bool         _firstPointLock;
            double       _currentAngle;      //the angle at which the control is already at
            double       _angle;             //the angle to add to the control
            const double _maxAngle = 230;
            RotateTransform               _rotate;
            TransformGroup                _transform;

        protected override void OnTouchDown(TouchEventArgs e)
        {
            _touchOrigin = e.TouchDevice.GetTouchPoint(this).Position;
            _angle = 0;

            //Pass the Message Up
            base.OnTouchDown(e);
        }

        protected override void OnTouchUp(TouchEventArgs e)
        {
            _currentAngle = _currentAngle + _angle;

            //Pass the message up
            base.OnTouchUp(e);
        }
        protected override void OnTouchMove(TouchEventArgs e)
        {
            System.Windows.Point newLocation = e.TouchDevice.GetTouchPoint(this).Position;
            _angle = CalculateAngle(_midPoint, _touchOrigin, newLocation);
            if (_currentAngle + _angle <= _maxAngle)
            {
                _rotate.Angle = _currentAngle + _angle;
                _center.RenderTransform = _transform;
            }
            if (_rotate.Angle >= _maxAngle) _rotate.Angle = _maxAngle;
            if (_rotate.Angle <= 0) _rotate.Angle = 0;

            //Pass the message up
            base.OnTouchMove(e);
        }

        public bool IsPressed()
        {
            return _isPressed;
        }
        public KnobControl(int width, int height)
        {
            _isPressed = false;
            _background = new System.Windows.Controls.Image();
            _background.Stretch = Stretch.Fill;
            _background.Width = width;
            _background.Height = height;
            this.Children.Add(_background);

            _center = new System.Windows.Controls.Image();
            _center.Stretch = Stretch.Fill;
            _center.Width = width;
            _center.Height = height;
            this.Children.Add(_center);

            _rotate = new RotateTransform(0, _center.Width / 2, _center.Height / 2);
            _transform = new TransformGroup();
            _transform.Children.Add(_rotate);

            _midPoint = new System.Windows.Point(_center.Width / 2, _center.Height / 2);

        }
        public void SetBackgroundImage(String fileName)
        {
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri(fileName, UriKind.Relative);
            bmp.EndInit();
            _backgroundImage = bmp;
            _background.Source = bmp;
        }
        public void SetCenterImage(String fileName)
        {
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri(fileName, UriKind.Relative);
            bmp.EndInit();
            _centerImage = bmp;
            _center.Source = bmp;
        }

        //Calculates the angle between two Points relative to the midpoint
        private double CalculateAngle(System.Windows.Point Mid, System.Windows.Point A, System.Windows.Point B)
        {
            double MidA = Math.Sqrt(Math.Pow(Mid.X - A.X, 2) + Math.Pow(Mid.Y - A.Y, 2));
            double AB = Math.Sqrt(Math.Pow(A.X - B.X, 2) + Math.Pow(A.Y - B.Y, 2));
            double MidB = Math.Sqrt(Math.Pow(Mid.X - B.X, 2) + Math.Pow(Mid.Y - B.Y, 2));
            Double angle = Math.Acos(((Math.Pow(AB, 2) - Math.Pow(MidA, 2) - Math.Pow(MidB, 2)) / (-2 * MidA * MidB)));

            double determinant = ((A.X - Mid.X) * (B.Y - Mid.Y)) - ((A.Y - Mid.Y) * (B.X - Mid.X));
            if (determinant < 0)
            {
                angle = -angle;
            }

            //check if memory got updated before the render
            if (Double.IsNaN(angle))
            {
                angle = 0;
            }
            return angle * (180 / Math.PI);
        }
        public double GetAngle()
        {
            return _currentAngle + _angle;
        }
    }
    
}

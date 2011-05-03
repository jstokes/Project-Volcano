using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;


namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private
            bool _isRect1Pressed;
            bool _isRect2Pressed;
            double _channel1Y;
            double _channel2Y;
            double _deck1Y;
            bool _channel1MixDown;
            bool _channel2MixDown;
            bool _deck1Rotation;
            bool _firstPointLock;
            double angle;

            System.Windows.Point previousPoint;
            System.Windows.Point _deck1MidPoint;
            TransformGroup trGrp1;
            TranslateTransform trTns1;
            TransformGroup trGrp2;
            TranslateTransform trTns2;
            TransformGroup trGrp3;
            RotateTransform rotTns3;
            DispatcherTimer dispatcherTimer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (!_deck1Rotation)
            {
                angle = angle + 1;
                rotTns3.Angle = angle;
                _deck1.RenderTransform = trGrp3;
            }
        }

        private void _channel1MixKnob_MouseLeave(object sender, MouseEventArgs e)
        {
            _channel1MixDown = false;
        }

        private void _channel1MixKnob_TouchDown(object sender, TouchEventArgs e)
        {
            _channel1MixDown = true;
        }

        private void _channel1MixKnob_TouchUp(object sender, TouchEventArgs e)
        {
            _channel1MixDown = false;
        }

        private void _channel1MixKnob_TouchMove(object sender, TouchEventArgs e)
        {
            if (_channel1MixDown)
            {
                System.Windows.Input.TouchPoint touchPoint = e.GetTouchPoint(this);
                System.Windows.Point point = touchPoint.Position;
                if (point.Y > _channel1Y)
                {
                    trTns1.Y = point.Y - _channel1Y;
                    _channel1MixKnob.RenderTransform = trGrp1;
                }
            }
        }

        private void _channel2MixKnob_TouchDown(object sender, TouchEventArgs e)
        {
            _channel2MixDown = true;
        }

        private void _channel2MixKnob_TouchUp(object sender, TouchEventArgs e)
        {
            _channel2MixDown = false;
        }

        private void _channel2MixKnob_TouchMove(object sender, TouchEventArgs e)
        {
            if (_channel2MixDown)
            {
                System.Windows.Input.TouchPoint touchPoint = e.GetTouchPoint(this);
                System.Windows.Point point = touchPoint.Position;
                if (point.Y > _channel2Y)
                {
                    trTns2.Y = point.Y - _channel2Y;
                    _channel2MixKnob.RenderTransform = trGrp2;
                }
            }
        }

        private void _deck1_TouchMove(object sender, TouchEventArgs e)
        {
            System.Windows.Input.TouchPoint touchPoint = e.GetTouchPoint(this);
            System.Windows.Point point = touchPoint.Position;
            double deltaY = point.Y - _deck1Y;
            if (_deck1Rotation && _firstPointLock)
            {
               //angle = angle + deltaY;
               angle = angle + CalculateAngle(_deck1MidPoint, previousPoint, point);
               rotTns3.Angle = angle;
               previousPoint = point;
                
            }
            //if it is the first time this touch position is
            //moved we initalize the previous point to the 
            //events initial position.
            if (!_firstPointLock)
            {
                previousPoint = point;
                _firstPointLock = true;
            }
            _deck1Y = point.Y;
        }

        private void _deck1_TouchDown(object sender, TouchEventArgs e)
        {
            System.Windows.Input.TouchPoint touchPoint = e.GetTouchPoint(this);
            System.Windows.Point point = touchPoint.Position;
            _deck1Y = point.Y;
            _deck1Rotation = true;
        }

        private void _deck1_TouchUp(object sender, TouchEventArgs e)
        {
            _deck1Rotation = false;
            _firstPointLock = false;
        }

        private double CalculateAngle(System.Windows.Point Mid, System.Windows.Point A, System.Windows.Point B){
	        double MidA = Math.Sqrt(Math.Pow(Mid.X-A.X, 2) + Math.Pow(Mid.Y-A.Y, 2));
	        double AB =   Math.Sqrt(Math.Pow(A.X-B.X, 2) + Math.Pow(A.Y-B.Y, 2));
	        double MidB = Math.Sqrt(Math.Pow(Mid.X-B.X, 2) + Math.Pow(Mid.Y-B.Y, 2));
            Double angle = Math.Acos(((Math.Pow(AB,2) - Math.Pow(MidA,2) - Math.Pow(MidB,2)) / (-2 * MidA * MidB)));

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

        private void _deck1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _deck1Rotation = true;
        }

        private void _deck1_MouseMove(object sender, MouseEventArgs e)
        {
            System.Windows.Point point = e.GetPosition(this);
            double deltaY = point.Y - _deck1Y;
            if (_deck1Rotation && _firstPointLock)
            {
                //angle = angle + deltaY;
                if (angle != 0)
                {
                    angle = angle + CalculateAngle(_deck1MidPoint, previousPoint, point);
                    rotTns3.Angle = angle;
                    _deck1.RenderTransform = trGrp3;
                }
                previousPoint = point;

            }
            //if it is the first time this touch position is
            //moved we initalize the previous point to the 
            //events initial position.
            if (!_firstPointLock)
            {
                previousPoint = point;
                _firstPointLock = true;
            }
            _deck1Y = point.Y;
        }

        private void _deck1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _deck1Rotation = false;
            _firstPointLock = false;
        }

        private void Grid_Initialized(object sender, EventArgs e)
        {
            _isRect1Pressed = false;
            _isRect2Pressed = false;
            _channel1MixDown = false;
            _channel2MixDown = false;
            _firstPointLock = true;

            _channel1Y = 175;
            _channel2Y = 175;
            angle = 1;
            _deck1MidPoint = new System.Windows.Point(_deck1.Width / 2 + _deck1.Margin.Left, _deck1.Height / 2 + _deck1.Margin.Top);

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dispatcherTimer.Start();

            //Link the transform to each element
            _channel1MixKnob.RenderTransform = trGrp1;
            _channel2MixKnob.RenderTransform = trGrp2;
            //_deck1.RenderTransform = trGrp3;

            //Set up the Translate for the Elements
            trTns1 = new TranslateTransform(0, 0);
            trGrp1 = new TransformGroup();
            trGrp1.Children.Add(trTns1);

            trTns2 = new TranslateTransform(0, 0);
            trGrp2 = new TransformGroup();
            trGrp2.Children.Add(trTns2);

            rotTns3 = new RotateTransform(1, _deck1.Width / 2, _deck1.Height / 2);
            trGrp3 = new TransformGroup();
            trGrp3.Children.Add(rotTns3);

        }

        private void image3_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void _deck1_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }
    }
}

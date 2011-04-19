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
using System.Windows.Threading;
using FMOD;

namespace WpfApplication1
{
    class DeckControl : Canvas
    {
        private
            const int RECORD_DIAMETER = 462;
            const int RECORD_MARGIN_LEFT = 235;
            const int RECORD_MARGIN_TOP = 15;
            const int KNOB_MARGIN_LEFT = 35;
            const int KNOB_MARGIN_TOP = 10;
            const int KNOB_PADDING = 10;
            const int KNOB_DIAMETER = 143;
            
            //deck control objects
            System.Windows.Controls.Image _mixerBackground;
            System.Windows.Point          _touchPoint;
            RecordController              _recordController;
            KnobControl                   _highPassKnob;
            KnobControl                   _lowPassKnob;
            KnobControl                   _reverbKnob;
            DispatcherTimer               dispatcherTimer;
            
            //transformation variables
            RotateTransform  rotate;
            TransformGroup   transform;
            
            //data processes
            System.Windows.Point previousPoint;
            System.Windows.Point _recordMidPoint;
            double               _recordY;
            bool                 _recordRotation;
            bool                 _firstPointLock;
            bool _twoTouchLock;
            bool _isPlaying;
            double               angle;
            int _touchPoints;
            Channel              channel;
            FMOD.System          system;

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (!_recordRotation && _isPlaying)
            {
                angle = angle + 1;
                rotate.Angle = angle;
                _recordController.RenderTransform = rotate;
            }
        }
        private void RecordTouchDown(object sender, TouchEventArgs e)
        {
            _touchPoints++;
            _recordRotation = true;
            System.Windows.Input.TouchPoint touchPoint = e.GetTouchPoint(this);
            System.Windows.Point point = touchPoint.Position;
            _recordY = point.Y;
            if (_touchPoints == 2)
            {
                if (!_isPlaying)
                {
                    channel.PlaySample();
                    _isPlaying = true;
                }
                else
                {
                    channel.StopSample();
                    _isPlaying = false;
                }
                _twoTouchLock = true;
            }
            if (_touchPoints == 1)
            {
                if (channel.IsPlaying())
                    channel.Pause();
            }
        }

        private void RecordTouchUp(object sender, TouchEventArgs e)
        {
            if(_touchPoints > 0) _touchPoints--;
            _recordRotation = false;
            _firstPointLock = false;
            if (_twoTouchLock)
            {
                if (_touchPoints == 0)
                {
                    _twoTouchLock = false;
                }
            }
            else
            {
                if (channel.IsPaused())
                {
                    channel.PlaySample();
                    channel.StopSeeking();
                }
            }
            
        }

        private void HPKnobTouchMove(object sender, TouchEventArgs e)
        {
            double angle = _highPassKnob.GetAngle();
            float conversion = (float)angle / 2.3f;
            channel.SetHighPass(conversion);
        }
        private void LPKnobTouchMove(object sender, TouchEventArgs e)
        {
            double angle = _lowPassKnob.GetAngle();
            float conversion = (float)angle / 2.3f;
            channel.SetLowPass(conversion);
        }

        private void RecordTouchMove(object sender, TouchEventArgs e)
        {
            System.Windows.Input.TouchPoint touchPoint = e.GetTouchPoint(this);
            System.Windows.Point point = touchPoint.Position;
            double deltaY = point.Y - _recordY;
            if (_recordRotation && _firstPointLock)
            {
                //angle = angle + deltaY;
                double angleDifference = CalculateAngle(_recordMidPoint, previousPoint, point);
                if (channel.GetCurrentTime() != 0 && !channel.IsSeeking())
                {
                    int seekPosition = (int) channel.GetCurrentTime();
                    double conversion = angleDifference * 52;
                    seekPosition = seekPosition + (int)conversion;
                    channel.PlaySoundAt((uint) seekPosition, (uint) 15);
                }
                angle = angle + angleDifference;
                rotate.Angle = angle;
                previousPoint = point;

               _recordController.RenderTransform = transform;
            }
            //if it is the first time this touch position is
            //moved we initalize the previous point to the 
            //events initial position.
            if (!_firstPointLock)
            {
                previousPoint = point;
                _firstPointLock = true;
            }
            _recordY = point.Y;
        }

        public DeckControl()
        {
            _mixerBackground = new Image();
            _mixerBackground.Height = 488;
            _mixerBackground.Width = 710;
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri("png/deck_background.png", UriKind.Relative);
            bmp.EndInit();
            _mixerBackground.Source = bmp;
            this.Children.Add(_mixerBackground);

            _recordController = new RecordController(RECORD_DIAMETER, RECORD_DIAMETER);
            _recordController.Margin = new Thickness(RECORD_MARGIN_LEFT, RECORD_MARGIN_TOP, 0, 0);
            _recordController.SetBackgroundImage(@"png/record.png");
            _recordController.Width = RECORD_DIAMETER;
            _recordController.Height = RECORD_DIAMETER;
            _recordController.TouchDown += new EventHandler<TouchEventArgs>(RecordTouchDown);
            _recordController.TouchUp += new EventHandler<TouchEventArgs>(RecordTouchUp);
            _recordController.TouchMove += new EventHandler<TouchEventArgs>(RecordTouchMove);
            _recordController.DragEnter += new System.Windows.DragEventHandler(RecordDragEnter);
            _recordController.DragLeave += new System.Windows.DragEventHandler(RecordDragLeave);
            _recordController.Drop += new System.Windows.DragEventHandler(RecordDrop);
            _recordController.AllowDrop = true;
            this.Children.Add(_recordController);

            _highPassKnob = new KnobControl(KNOB_DIAMETER, KNOB_DIAMETER);
            _highPassKnob.Margin = new Thickness(KNOB_MARGIN_LEFT, KNOB_MARGIN_TOP, 0, 0);
            _highPassKnob.SetBackgroundImage(@"png/dial_background.png");
            _highPassKnob.SetCenterImage(@"png/dial_knob.png");
            _highPassKnob.Width = KNOB_DIAMETER;
            _highPassKnob.Height = KNOB_DIAMETER;
            //_highPassKnob.TouchDown += new EventHandler<TouchEventArgs>(HPKnobTouchDown);
            //_highPassKnob.TouchUp += new EventHandler<TouchEventArgs>(HPKnobTouchUp);
            _highPassKnob.TouchMove += new EventHandler<TouchEventArgs>(HPKnobTouchMove);
            this.Children.Add(_highPassKnob);

            _lowPassKnob = new KnobControl(KNOB_DIAMETER, KNOB_DIAMETER);
            _lowPassKnob.Margin = new Thickness(KNOB_MARGIN_LEFT, KNOB_MARGIN_TOP + KNOB_DIAMETER * 1 + KNOB_PADDING * 1, 0, 0);
            _lowPassKnob.SetBackgroundImage(@"png/dial_background.png");
            _lowPassKnob.SetCenterImage(@"png/dial_knob.png");
            _lowPassKnob.Width = KNOB_DIAMETER;
            _lowPassKnob.Height = KNOB_DIAMETER;
            _lowPassKnob.TouchMove += new EventHandler<TouchEventArgs>(LPKnobTouchMove);
            this.Children.Add(_lowPassKnob);

            _reverbKnob = new KnobControl(KNOB_DIAMETER, KNOB_DIAMETER);
            _reverbKnob.Margin = new Thickness(KNOB_MARGIN_LEFT, KNOB_MARGIN_TOP + KNOB_DIAMETER * 2 + KNOB_PADDING * 2, 0, 0);
            _reverbKnob.SetBackgroundImage(@"png/dial_background.png");
            _reverbKnob.SetCenterImage(@"png/dial_knob.png");
            _reverbKnob.Width = KNOB_DIAMETER;
            _reverbKnob.Height = KNOB_DIAMETER;
            this.Children.Add(_reverbKnob);
            

            angle = 1;
            _recordMidPoint = new System.Windows.Point(_recordController.Width / 2 + _recordController.Margin.Left, _recordController.Height / 2 + _recordController.Margin.Top);
            rotate = new RotateTransform(1, _recordController.Width / 2, _recordController.Height / 2);
            transform = new TransformGroup();
            transform.Children.Add(rotate);

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dispatcherTimer.Start();

            system = new FMOD.System();
            FMOD.RESULT result = Factory.System_Create(ref system);
            system.init(8, INITFLAGS.NORMAL, (IntPtr)null);
            channel = new Channel(system);
            channel.Initialize(0);
            channel.SetHighPass(1.0f);
            _isPlaying = false;
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
        public void LoadSample(String sample)
        {
            channel.LoadSample(sample);
        }
        public void SetVolume(float value)
        {
            channel.SetVolume(value);
            
        }
        public bool IsPlaying()
        {
            return channel.IsPlaying();
        }
        private void RecordDragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("songInfo") ||
                sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
            _recordController.SetBackgroundImage("png/record_hover.png");
        }
        private void RecordDragLeave(object sender, DragEventArgs e)
        {
            _recordController.SetBackgroundImage("png/record.png"); 
            
        }
        private void RecordDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("songInfo"))
            {
                songInfo info = e.Data.GetData("songInfo") as songInfo;
                _recordController.SetBackgroundImage("png/record_hover_loading.png");
                channel.LoadSample(info.path);
                _recordController.UpdateLabel(info);
            }
            _recordController.SetBackgroundImage("png/record.png");
        }


    }
}

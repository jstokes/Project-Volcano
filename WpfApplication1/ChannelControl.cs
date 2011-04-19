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
using FMOD;

namespace WpfApplication1
{
    class ChannelControl : Canvas
    {
        private
            const int MIXERKNOB_WIDTH = 132;
            const int MIXERKNOB_HEIGHT = 122;
            const int MIXERKNOB_X = 57;
            const int SLIDERTOP = 110;
            const int SLIDERHEIGHT = 335;
            const int SLIDERBOTTOM = 500;
            const int PANEL_HEIGHT = 0;
            const int BUTTON_HEIGHT = 48;
            const int BUTTON_WIDTH = 111;
            const int BUTTON_PADDING = 3;
            const int BUTTON_MARGIN_LEFT = 10;
            const int BUTTON_MARGIN_TOP = 10;

            SliderButton _mixerKnob;
            ToggleButton _soloButton;
            ToggleButton _muteButton;
            ToggleButton _playButton;
            ToggleButton _loopButton;
            System.Windows.Controls.Image _mixerBackground;
            TranslateTransform translate;
            TransformGroup transform;
            System.Windows.Point _touchPoint;

            Channel channel;
            
            private void TouchDown(object sender, TouchEventArgs e)
            {
                System.Windows.Input.TouchPoint touchPoint = e.GetTouchPoint(this);
                System.Windows.Point point = touchPoint.Position;
                _touchPoint = touchPoint.Position;
            }

            private void TouchUp(object sender, TouchEventArgs e)
            {
                
            }

            private void TouchMove(object sender, TouchEventArgs e)
            {
                if (_mixerKnob.IsPressed())
                {
                    System.Windows.Input.TouchPoint touchPoint = e.GetTouchPoint(this);
                    System.Windows.Point point = touchPoint.Position;

                    double deltaY = point.Y - _touchPoint.Y;
                    
                    if (translate.Y <= SLIDERHEIGHT && translate.Y >= 0)
                    {
                        translate.Y += deltaY;
                        _mixerKnob.RenderTransform = transform;
                    }
                    if (translate.Y < 0) translate.Y = 0;
                    if (translate.Y > SLIDERHEIGHT) translate.Y = SLIDERHEIGHT;
                    _touchPoint = point;
                }
            }

        public ChannelControl()
        {
            _mixerBackground = new Image();
            _mixerBackground.Height = 552;
            _mixerBackground.Width = 243;
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri("png/channelSlider.png", UriKind.Relative);
            bmp.EndInit();
            _mixerBackground.Source = bmp;
            this.Children.Add(_mixerBackground);

            _soloButton = new ToggleButton(BUTTON_WIDTH, BUTTON_HEIGHT);
            _soloButton.Margin = new Thickness(BUTTON_MARGIN_LEFT, BUTTON_MARGIN_TOP, 0, 0);
            _soloButton.SetBackgroundImage(@"png/solo_off.png");
            _soloButton.SetPressedImage(@"png/solo_on.png");
            _soloButton.Width = BUTTON_WIDTH;
            _soloButton.Height = BUTTON_HEIGHT;
            //_soloButton.TouchDown += new EventHandler<TouchEventArgs>(TouchDown);
            //_soloButton.TouchUp += new EventHandler<TouchEventArgs>(TouchUp);
            //_soloButton.TouchMove += new EventHandler<TouchEventArgs>(TouchMove);
            this.Children.Add(_soloButton);

            _muteButton = new ToggleButton(BUTTON_WIDTH, BUTTON_HEIGHT);
            _muteButton.Margin = new Thickness(BUTTON_WIDTH + BUTTON_PADDING + BUTTON_MARGIN_LEFT, BUTTON_MARGIN_TOP, 0, 0);
            _muteButton.SetBackgroundImage(@"png/mute_off.png");
            _muteButton.SetPressedImage(@"png/mute_on.png");
            _muteButton.Width = BUTTON_WIDTH;
            _muteButton.Height = BUTTON_HEIGHT;
            //_muteButton.TouchDown += new EventHandler<TouchEventArgs>(TouchDown);
            //_muteButton.TouchUp += new EventHandler<TouchEventArgs>(TouchUp);
            //_muteButton.TouchMove += new EventHandler<TouchEventArgs>(TouchMove);
            this.Children.Add(_muteButton);

            _playButton = new ToggleButton(BUTTON_WIDTH, BUTTON_HEIGHT);
            _playButton.Margin = new Thickness(BUTTON_MARGIN_LEFT, BUTTON_HEIGHT + BUTTON_PADDING + BUTTON_MARGIN_TOP, 0, 0);
            _playButton.SetBackgroundImage(@"png/play_off.png");
            _playButton.SetPressedImage(@"png/play_on.png");
            _playButton.Width = BUTTON_WIDTH;
            _playButton.Height = BUTTON_HEIGHT;
            //_playButton.TouchDown += new EventHandler<TouchEventArgs>(TouchDown);
            //_playButton.TouchUp += new EventHandler<TouchEventArgs>(TouchUp);
            //_playButton.TouchMove += new EventHandler<TouchEventArgs>(TouchMove);
            this.Children.Add(_playButton);

            _loopButton = new ToggleButton(BUTTON_WIDTH, BUTTON_HEIGHT);
            _loopButton.Margin = new Thickness(BUTTON_WIDTH + BUTTON_PADDING + BUTTON_MARGIN_LEFT, BUTTON_HEIGHT + BUTTON_PADDING + BUTTON_MARGIN_TOP, 0, 0);
            _loopButton.SetBackgroundImage(@"png/loop_off.png");
            _loopButton.SetPressedImage(@"png/loop_on.png");
            _loopButton.Width = BUTTON_WIDTH;
            _loopButton.Height = BUTTON_HEIGHT;
            //_loopButton.TouchDown += new EventHandler<TouchEventArgs>(TouchDown);
            //_loopButton.TouchUp += new EventHandler<TouchEventArgs>(TouchUp);
            //_loopButton.TouchMove += new EventHandler<TouchEventArgs>(TouchMove);
            this.Children.Add(_loopButton);

            _mixerKnob = new SliderButton(MIXERKNOB_WIDTH, MIXERKNOB_HEIGHT);
            _mixerKnob.Margin = new Thickness(MIXERKNOB_X, SLIDERTOP, 0, 0);
            _mixerKnob.SetBackgroundImage("png/channelKnob.png");
            _mixerKnob.SetPressedImage("png/channelKnob.png");
            _mixerKnob.Width = MIXERKNOB_WIDTH;
            _mixerKnob.Height = MIXERKNOB_HEIGHT;
            _mixerKnob.TouchDown += new EventHandler<TouchEventArgs>(TouchDown);
            _mixerKnob.TouchUp += new EventHandler<TouchEventArgs>(TouchUp);
            _mixerKnob.TouchMove += new EventHandler<TouchEventArgs>(TouchMove);
            this.Children.Add(_mixerKnob);

            translate = new TranslateTransform(0, 0);
            transform = new TransformGroup();
            transform.Children.Add(translate);
            
        }
    }
}

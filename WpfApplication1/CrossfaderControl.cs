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
    class CrossfaderControl : Canvas
    {
        private
            const int MIXERKNOB_WIDTH = 90;
            const int MIXERKNOB_HEIGHT = 99;
            const int MIXERKNOB_X = 4;
            const int SLIDERTOP = 0;
            const int SLIDERHEIGHT = 335;
            const int SLIDERBOTTOM = 392;
            const int PANEL_HEIGHT = 0;
            const double midPoint = SLIDERHEIGHT / 2 + MIXERKNOB_HEIGHT / 2;
            const double halfLength = (SLIDERBOTTOM - SLIDERTOP) / 2;
            double ratio;

            SliderButton _mixerKnob;
            System.Windows.Controls.Image _mixerBackground;
            TranslateTransform translate;
            TransformGroup transform;
            System.Windows.Point _touchPoint;
            DeckControl deck1;
            DeckControl deck2;

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

                if (translate.Y <= SLIDERBOTTOM && translate.Y >= 0)
                {
                    translate.Y += deltaY;
                    _mixerKnob.RenderTransform = transform;
                }
                //if the slider is at the top or bottom, set its transform(don't mess up the logic)
                if (translate.Y < 0) translate.Y = 0;
                if (translate.Y > SLIDERBOTTOM) translate.Y = SLIDERBOTTOM;

                //now convert the transform position into something useful
                //say volume control

                //control favors right deck
                if (translate.Y > midPoint)
                {
                    if (deck2.IsPlaying()) deck2.SetVolume(1.0f);
                    ratio = (halfLength - (translate.Y - midPoint)) / halfLength;
                    if (deck1.IsPlaying()) deck1.SetVolume((float)ratio);
                }

                //control favors left deck
                if(translate.Y < midPoint)
                {
                    if(deck1.IsPlaying()) deck1.SetVolume(1.0f);
                    ratio = (halfLength - (midPoint - translate.Y)) / halfLength;
                    if (deck2.IsPlaying()) deck2.SetVolume((float)ratio);
                }

                _touchPoint = point;
            }
        }

        public CrossfaderControl(ref DeckControl d1, ref DeckControl d2)
        {
            _mixerBackground = new Image();
            _mixerBackground.Height = 489;
            _mixerBackground.Width = 97;
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri("png/crossfadeBackground.png", UriKind.Relative);
            bmp.EndInit();
            _mixerBackground.Source = bmp;
            this.Children.Add(_mixerBackground);

            _mixerKnob = new SliderButton(MIXERKNOB_WIDTH, MIXERKNOB_HEIGHT);
            _mixerKnob.Margin = new Thickness(MIXERKNOB_X, SLIDERTOP, 0, 0);
            _mixerKnob.SetBackgroundImage("png/crossfadeSlider.png");
            _mixerKnob.SetPressedImage("png/crossfadeSlider.png");
            _mixerKnob.Width = MIXERKNOB_WIDTH;
            _mixerKnob.Height = MIXERKNOB_HEIGHT;
            _mixerKnob.TouchDown += new EventHandler<TouchEventArgs>(TouchDown);
            _mixerKnob.TouchUp += new EventHandler<TouchEventArgs>(TouchUp);
            _mixerKnob.TouchMove += new EventHandler<TouchEventArgs>(TouchMove);
            this.Children.Add(_mixerKnob);

            translate = new TranslateTransform(0, SLIDERHEIGHT / 2 + MIXERKNOB_HEIGHT / 2);
            transform = new TransformGroup();
            transform.Children.Add(translate);
            _mixerKnob.RenderTransform = transform;

            deck1 = d1;
            deck2 = d2;
            

        }
    }
}

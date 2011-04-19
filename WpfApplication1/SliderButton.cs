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

namespace WpfApplication1
{
    class SliderButton : Canvas
    {
        private
            System.Windows.Controls.Image _mixerKnob;
            BitmapImage _backgroundImage;
            BitmapImage _pressedImage;
            bool _isPressed;

        protected override void OnTouchDown(TouchEventArgs e)
        {
            _isPressed = true;   
        }

        protected override void OnTouchUp(TouchEventArgs e)
        {
            _isPressed = false;
        }

        public bool IsPressed()
        {
            return _isPressed;
        }
        public SliderButton(int width, int height)
        {
            _mixerKnob = new Image();
            _mixerKnob.Stretch = Stretch.Fill;
            _mixerKnob.Width = width;
            _mixerKnob.Height = height;
            this.Children.Add(_mixerKnob);

        }
        public void SetBackgroundImage(String fileName)
        {
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri(fileName, UriKind.Relative);
            bmp.EndInit();
            _backgroundImage = bmp;
            _mixerKnob.Source = bmp;
        }
        public void SetPressedImage(String fileName)
        {
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri(fileName, UriKind.Relative);
            bmp.EndInit();
            _pressedImage = bmp;
        }
    }
}

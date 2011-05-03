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
using System.Timers;

namespace WpfApplication1
{
    class ToggleTapButton : Canvas
    {
        private
            System.Windows.Controls.Image _button;
        BitmapImage _backgroundImage;
        BitmapImage _pressedImage;
        bool _isPressed;

        protected override void OnTouchDown(TouchEventArgs e)
        {
            if (_isPressed)
            {
                _isPressed = false;
                _button.Source = _backgroundImage;
            }
            else
            {
                _isPressed = true;
                _button.Source = _pressedImage;
            }
            this.InvalidateVisual();
        }

        protected override void OnTouchUp(TouchEventArgs e)
        {

        }

        public bool IsPressed()
        {
            return _isPressed;
        }
        public ToggleTapButton(int width, int height)
        {
            _isPressed = false;
            _button = new System.Windows.Controls.Image();
            _button.Stretch = Stretch.Fill;
            _button.Width = width;
            _button.Height = height;
            this.Children.Add(_button);

        }
        public void SetBackgroundImage(String fileName)
        {
            //BitmapImage bmp = new BitmapImage();
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri(fileName, UriKind.Relative);
            bmp.EndInit();
            _backgroundImage = bmp;
            _button.Source = bmp;
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

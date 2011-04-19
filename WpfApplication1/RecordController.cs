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
    class RecordController : Canvas
    {
        private
            System.Windows.Controls.Image _button;
            BitmapImage _backgroundImage;
            BitmapImage _pressedImage;
            Label _artistLabel;
            Label _titleLabel;
            bool _isPressed;
            
        protected override void OnTouchDown(TouchEventArgs e)
        {
            
            this.InvalidateVisual();
        }

        protected override void OnTouchUp(TouchEventArgs e)
        {

        }

        public bool IsPressed()
        {
            return _isPressed;
        }
        public RecordController(int width, int height)
        {
            _isPressed = false;
            _button = new System.Windows.Controls.Image();
            _button.Stretch = Stretch.Fill;
            _button.Width = width;
            _button.Height = height;
            this.Children.Add(_button);

            _artistLabel = new System.Windows.Controls.Label();
            _artistLabel.Content = "Artist";
            _artistLabel.Margin = new Thickness(0, 170, 0, 0);
            _artistLabel.Width = 462;
            _artistLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
            _artistLabel.FontFamily = new System.Windows.Media.FontFamily("Calibri");
            _artistLabel.FontSize = 20.0;
            this.Children.Add(_artistLabel);

            _titleLabel = new System.Windows.Controls.Label();
            _titleLabel.Content = "Title";
            _titleLabel.Margin = new Thickness(100, 260, 0, 0);
            _titleLabel.Width = 262;
            _titleLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
            this.Children.Add(_titleLabel);

        }
        public void SetBackgroundImage(String fileName)
        {
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri(fileName, UriKind.Relative);
            bmp.EndInit();
            _backgroundImage = bmp;
            _button.Source = bmp;
            this.InvalidateVisual();
        }
        public void UpdateLabel(songInfo info)
        {
            _titleLabel.Content = info.title;
            _artistLabel.Content = info.artist;
            this.InvalidateVisual();
        }
    }
}

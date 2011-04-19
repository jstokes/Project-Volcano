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
    class DirectoryListItem : Canvas
    {
        private
            const int DIRECTORY_WIDTH = 385;
        //Layout Objects
        System.Windows.Controls.Image _background;
        BitmapImage _backgroundImage;
        bool _isPressed;
        System.Windows.Controls.Label _artist;
        System.Windows.Controls.Label _title;
        songInfo itemInfo;
        bool isSelected;

        //Touch Control Objects
        System.Windows.Point _touchOrigin;
        double _touchY;
        int _touchPoints;
        bool _touchRotation;
        bool _twoFingersDown;
        TranslateTransform _translate;
        TransformGroup _transform;

        protected override void OnTouchDown(TouchEventArgs e)
        {
            _touchPoints++;
            if (_touchPoints == 2) _twoFingersDown = true;
            isSelected = true;
            //Pass the Message Up
            base.OnTouchDown(e);
        }

        protected override void OnTouchUp(TouchEventArgs e)
        {
            _touchPoints--;
            _twoFingersDown = false;
            isSelected = false;
            //Pass the message up
            base.OnTouchUp(e);
        }
        protected override void OnTouchMove(TouchEventArgs e)
        {
            //two fingers means scrolling
            if (_twoFingersDown)
            {

            }

            //Pass the message up
            base.OnTouchMove(e);
        }

        public bool IsDoublePressed()
        {
            return _twoFingersDown;
        }
        public bool IsSelected()
        {
            return isSelected;
        }
        public DirectoryListItem(int width, int height, songInfo info)
        {
            itemInfo = info;

            _isPressed = false;
            _background = new System.Windows.Controls.Image();
            _background.Stretch = Stretch.Fill;
            _background.Width = width;
            _background.Height = height;
            this.Children.Add(_background);

            _artist = new System.Windows.Controls.Label();
            _artist.Content = info.artist;
            _artist.Margin = new Thickness(0, 0, 0, 0);
            _artist.FontFamily = new System.Windows.Media.FontFamily("Calibri");
            _artist.FontSize = 20.0;
            _artist.Foreground = System.Windows.Media.Brushes.White;
            this.Children.Add(_artist);

            _title = new System.Windows.Controls.Label();
            _title.Content = info.title;
            _title.FontFamily = new System.Windows.Media.FontFamily("Calibri");
            _title.FontSize = 16.0;
            _title.Foreground = System.Windows.Media.Brushes.White;
            _title.Margin = new Thickness(0, 30, 0, 0);
            this.Children.Add(_title);

            itemInfo = info;
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
        public songInfo GetSelectedInfo()
        {
            return itemInfo;
        }
        public void IsSelected(bool sel)
        {
            isSelected = sel;
        }
    }
}

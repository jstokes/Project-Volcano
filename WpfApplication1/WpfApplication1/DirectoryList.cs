using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
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
    class DirectoryList : Canvas
    {
        private
            const int DIRECTORY_WIDTH = 385;
            const int LIST_HEIGHT = 800;
            const int ITEM_HEIGHT = 100;
            const int SORTBUTTON_WIDTH = 127;
            const int SORTBUTTON_HEIGHT = 77;
            const int TITLEBUTTON_WIDTH = 131;
            const int ALBUMBUTTON_WIDTH = 127;

            //Layout Objects
            bool _isPressed;
            bool isSelected;
            DirectoryListItem[] _dirList;
            DirectoryClass _dir;
            DirectorySortButton _artistSort;
            DirectorySortButton _titleSort;
            DirectorySortButton _albumSort;

            //Touch Control Objects
            System.Windows.Point _touchOrigin;
            double _touchY;
            int _touchPoints;
            int _numItems;
            bool _touchRotation;
            bool _twoFingersDown;
            bool _isDragging;
            bool _isSorting;
            double _currentPosition;
            System.Windows.Input.TouchDevice _primaryDevice;
            System.Timers.Timer _touchTimer;
            TranslateTransform _translate;
            TransformGroup     _transform;

        protected override void OnTouchDown(TouchEventArgs e)
        {
            //check to see if any of the sorts are applied.
            if (_albumSort.IsPressed() || _titleSort.IsPressed() || _artistSort.IsPressed())
            {
                if (_albumSort.IsPressed())
                {
                    _dir.sortAlbum();
                    ReSort();
                }
                if (_titleSort.IsPressed())
                {
                    _dir.sortTitle();
                    ReSort();
                }
                if (_artistSort.IsPressed())
                {
                    _dir.sortArtist();
                    ReSort();
                }
            }
            else
            {
                _touchTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                _touchTimer.Interval = 30;
                _touchTimer.AutoReset = false;
                _touchTimer.Enabled = true;
                if (_touchPoints == 0)
                {
                    _primaryDevice = e.TouchDevice;
                    _isPressed = true;
                }
                _touchPoints++;
                if (_touchPoints == 2) _twoFingersDown = true;
                _touchOrigin = e.TouchDevice.GetTouchPoint(this).Position;
            }
            //_isDragging = true;
            //Pass the Message Up
            base.OnTouchDown(e);
        }

        protected override void OnTouchUp(TouchEventArgs e)
        {
            if (_touchPoints > 0) _touchPoints--;
            _twoFingersDown = false;
            _touchTimer.Enabled = false;

            if (e.TouchDevice == _primaryDevice)
            {
                _primaryDevice = null;
            }

            if (_touchPoints == 0)
            {
                _isDragging = false;
                _isPressed = false;
                _currentPosition = _translate.Y;
            }
            _isSorting = false;

            //Pass the message up
            base.OnTouchUp(e);
        }
        protected override void OnTouchMove(TouchEventArgs e)
        {
            if (e.GetTouchPoint(this).Position.Y < this.Height - SORTBUTTON_HEIGHT)
            {

                if (_primaryDevice == null) _primaryDevice = e.TouchDevice;
                if (e.TouchDevice == _primaryDevice && _twoFingersDown)
                {
                    System.Windows.Point newPoint = e.TouchDevice.GetTouchPoint(this).Position;
                    double deltaY = newPoint.Y - _touchOrigin.Y;
                    _translate.Y = _currentPosition + deltaY;
                    //two fingers means scrolling
                    if (_twoFingersDown)
                    {
                        if (_translate.Y < 0)
                        {
                            for (int i = 0; i < _numItems; i++)
                            {
                                _dirList[i].RenderTransform = _transform;
                            }
                        }
                    }
                }
            }
            else
            {
                _isSorting = true;
            }

            //Pass the message up
            base.OnTouchMove(e);
        }
        protected override void OnTouchLeave(TouchEventArgs e)
        {
            if(_touchPoints > 0 ) _touchPoints--;
            _twoFingersDown = false;

            if (e.TouchDevice == _primaryDevice)
            {
                _primaryDevice = null;
            }

            if (_touchPoints == 0)
            {
                _isDragging = false;
                _isPressed = false;
                _currentPosition = _translate.Y;
            }

            base.OnTouchLeave(e);
        }

        //if the finger is down long enough to flag
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (_touchPoints == 1)
            {
                _isDragging = true;
            }
        }


        public bool IsDragging()
        {
            return _isDragging;
        }
        public bool IsPressed()
        {
            return _isPressed;
        }
        public DirectoryList(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            _currentPosition = 0;
            _touchPoints = 0;
            _isSorting = false;

            _dir = new DirectoryClass("C:\\Users\\Administrator\\Documents\\Visual Studio 2010\\Projects\\WpfApplication1\\WpfApplication1\\media");
            _numItems = _dir.GetFileCount();
            _dirList = new DirectoryListItem[_numItems];
            //_dir.sortTitle();
            for (int i = 0; i < _numItems; i++)
            {
                _dirList[i] = new DirectoryListItem(DIRECTORY_WIDTH, ITEM_HEIGHT, _dir.getSongInfo(i));
                _dirList[i].Margin = new Thickness(0, ITEM_HEIGHT * i, 0, 0);
                _dirList[i].SetBackgroundImage(@"png/inforthekill.png");
                _dirList[i].Width = 385;
                _dirList[i].Height = 100;
                this.Children.Add(_dirList[i]);
            }
            _translate = new TranslateTransform(0, 0);
            _transform = new TransformGroup();
            _transform.Children.Add(_translate);
            this.ClipToBounds = true;

            _artistSort = new DirectorySortButton(SORTBUTTON_WIDTH, SORTBUTTON_HEIGHT);
            _artistSort.Margin = new Thickness(0 , this.Height - SORTBUTTON_HEIGHT, 0, 0);
            _artistSort.SetBackgroundImage("png/artist_off.png");
            _artistSort.SetPressedImage("png/artist_on.png");
            this.Children.Add(_artistSort);

            _titleSort = new DirectorySortButton(TITLEBUTTON_WIDTH, SORTBUTTON_HEIGHT);
            _titleSort.Margin = new Thickness(SORTBUTTON_WIDTH, this.Height - SORTBUTTON_HEIGHT, 0, 0);
            _titleSort.SetBackgroundImage("png/title_off.png");
            _titleSort.SetPressedImage("png/title_on.png");
            this.Children.Add(_titleSort);

            _albumSort = new DirectorySortButton(ALBUMBUTTON_WIDTH, SORTBUTTON_HEIGHT);
            _albumSort.Margin = new Thickness(SORTBUTTON_WIDTH + TITLEBUTTON_WIDTH, this.Height - SORTBUTTON_HEIGHT, 0, 0);
            _albumSort.SetBackgroundImage("png/album_off.png");
            _albumSort.SetPressedImage("png/album_on.png");
            this.Children.Add(_albumSort);

            _touchTimer = new System.Timers.Timer(200);
            _touchTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            _touchTimer.Interval = 500;
        }
        public songInfo GetSelectedSong()
        {
            songInfo info = null;
            int count = 0;
            while (count < _dir.GetFileCount())
            {
                if (_dirList[count].IsSelected())
                {
                    info = _dirList[count].GetSelectedInfo();
                    _dirList[count].SetBackgroundImage("png/inforthekill_selected.png");
                }
                count++;
            }
            if (info != null) isSelected = true;
            return info;
        }
        public bool IsSelected()
        {
            return isSelected;
        }
        public void ResetSelection()
        {
            int count = 0;
            while (count < _dir.GetFileCount())
            {
                if (_dirList[count].IsSelected())
                    _dirList[count].SetBackgroundImage("png/inforthekill.png");
                _dirList[count].IsSelected(false);
                count++;
            }
        }
        public bool IsSorting()
        {
            return _isSorting;
        }
        public void ReSort()
        {
            for (int i = 0; i < _numItems; i++)
            {
                _dirList[i].UpdateInfo(_dir.getSongInfo(i));
            }
            this.Children.Remove(_artistSort);
            this.Children.Remove(_titleSort);
            this.Children.Remove(_albumSort);
            this.Children.Add(_artistSort);
            this.Children.Add(_titleSort);
            this.Children.Add(_albumSort);
            this.InvalidateVisual();
        }
    }
}

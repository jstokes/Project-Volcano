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
using System.Windows.Shapes;
using FMOD;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MixerMain.xaml
    /// </summary>
    public partial class MixerMain : Window
    {
        private
            const int SLIDER_WIDTH = 250;
            const int SLIDER_MARGIN_TOP = 500;
            const int SLIDER_PADDING = 5;
            const int DECK_MARGIN_LEFT = 808;
            const int DECK_MARGIN_TOP = 10;
            const int DECK_WIDTH = 710;

            ChannelControl channel1;
            ChannelControl channel2;
            ChannelControl channel3;
            ChannelControl channel4;
            ChannelControl channel5;
            ChannelControl channel6;
            DeckControl deck1;
            DeckControl deck2;
            CrossfaderControl fader;
            DirectoryList directory;
            DirectoryListItem selected;
            songInfo selectedItem;    //contains the selected song data

            TranslateTransform translate;
            TransformGroup trnsGrp;

            bool selectedItemExists;
            bool dragDropInit;

        public MixerMain()
        {
            InitializeComponent();

            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            channel1 = new ChannelControl();
            channel1.Margin = new Thickness(0, SLIDER_MARGIN_TOP, 0, 0);
            _canvas.Children.Add(channel1);

            channel2 = new ChannelControl();
            channel2.Margin = new Thickness(SLIDER_WIDTH + SLIDER_PADDING, SLIDER_MARGIN_TOP, 0, 0);
            _canvas.Children.Add(channel2);

            channel3 = new ChannelControl();
            channel3.Margin = new Thickness(SLIDER_WIDTH * 2 + SLIDER_PADDING * 2, SLIDER_MARGIN_TOP, 0, 0);
            _canvas.Children.Add(channel3);

            channel4 = new ChannelControl();
            channel4.Margin = new Thickness(SLIDER_WIDTH * 3 + SLIDER_PADDING * 3, SLIDER_MARGIN_TOP, 0, 0);
            _canvas.Children.Add(channel4);

            channel5 = new ChannelControl();
            channel5.Margin = new Thickness(SLIDER_WIDTH * 4 + SLIDER_PADDING * 4, SLIDER_MARGIN_TOP, 0, 0);
            _canvas.Children.Add(channel5);

            channel6 = new ChannelControl();
            channel6.Margin = new Thickness(SLIDER_WIDTH * 5 + SLIDER_PADDING * 5, SLIDER_MARGIN_TOP, 0, 0);
            _canvas.Children.Add(channel6);

            deck1 = new DeckControl();
            deck1.Margin = new Thickness(0, DECK_MARGIN_TOP, 0, 0);
            deck1.LoadSample("../../media/In For The Kill.mp3");
            _canvas.Children.Add(deck1);

            deck2 = new DeckControl();
            deck2.Margin = new Thickness(DECK_MARGIN_LEFT, DECK_MARGIN_TOP, 0, 0);
            deck2.LoadSample("../../media/Bad Romance.mp3");
            _canvas.Children.Add(deck2);

            fader = new CrossfaderControl(ref deck1, ref deck2);
            fader.Margin = new Thickness(DECK_WIDTH, DECK_MARGIN_TOP, 0, 0);
            _canvas.Children.Add(fader);

            directory = new DirectoryList(385, 600);
            directory.Margin = new Thickness(DECK_MARGIN_LEFT + DECK_WIDTH, 0, 0, 0);
            directory.TouchDown += new EventHandler<TouchEventArgs>(DirectoryTouchDown);
            //directory.TouchUp += new EventHandler<TouchEventArgs>(DirectoryTouchUp);
            directory.TouchMove += new EventHandler<TouchEventArgs>(DirectoryTouchMove);
            _canvas.Children.Add(directory);

            selected = new DirectoryListItem(385, 100, new songInfo());
            //selected.TouchMove += new EventHandler<TouchEventArgs>(SelectedTouchMove);
            //selected.TouchUp += new EventHandler<TouchEventArgs>(SelectedTouchUp);

            translate = new TranslateTransform(0, 0);
            trnsGrp = new TransformGroup();
            trnsGrp.Children.Add(translate);

            selectedItemExists = false;
            dragDropInit = false;
        }

        ////////////////////////////////////////////
        // class interaction methods go here
        private void DirectoryTouchDown(object sender, TouchEventArgs e)
        {
            
        }
        private void DirectoryTouchMove(object sender, TouchEventArgs e)
        {
            if (directory.IsDragging() && !selectedItemExists)
            {
                //clear the last song info
                selectedItem = new songInfo();
                //get the song in the array that was selected
                selectedItem = directory.GetSelectedSong();

                selected = new DirectoryListItem(385, 100, selectedItem);
                selected.Margin = new Thickness((int)e.TouchDevice.GetTouchPoint(this).Position.X - (385 / 2), (int)e.TouchDevice.GetTouchPoint(this).Position.Y - (100 / 2), 0, 0);
                selected.SetBackgroundImage(@"png/inforthekill.png");
                selected.Width = 385;
                selected.Height = 100;
                if (!selectedItemExists)
                {
                    //_canvas.Children.Add(selected);
                    if (!dragDropInit)
                    {
                        // Initialize the drag & drop operation
                        DataObject dragData = new DataObject("songInfo", selectedItem);
                        DragDrop.DoDragDrop(selected, dragData, DragDropEffects.Move);
                    }
                    directory.ResetSelection();
                }
            }
        }

        private void DirectoryTouchUp(object sender, TouchEventArgs e)
        {
            
        }

        //private void SelectedTouchMove(object sender, TouchEventArgs e)
        //{
        //    translate.X = (int)e.TouchDevice.GetTouchPoint(this).Position.X - (385 / 2) - selected.Margin.Left;
        //    translate.Y = (int)e.TouchDevice.GetTouchPoint(this).Position.Y - (100 / 2) - selected.Margin.Top;
        //    selected.RenderTransform = trnsGrp;

            

        //}
        //private void SelectedTouchUp(object sender, TouchEventArgs e)
        //{
        //    _canvas.Children.Remove(selected);
        //    selectedItemExists = false;
        //    translate.X = 0;
        //    translate.Y = 0;
        //}
    }
}

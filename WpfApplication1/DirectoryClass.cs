using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TagLib;
using TagLib.Id3v2;


namespace WpfApplication1
{

    class songInfo
    {
        public string path, name, year, album, title, artist;
        public long size, songLength, bitRate;
        public songInfo() { }
    }
    class DirectoryClass
    {

        public DirectoryClass(string directory)
        {
            MakeDataArray(directory);

            for (int t = 0; t < songList.Length; t++)
            {
                Console.WriteLine("Artist: " + songList[t].artist);
                Console.WriteLine("Title: " + songList[t].title);
                Console.WriteLine("Album: " + songList[t].album);
                Console.WriteLine("Year: " + songList[t].year);
                Console.WriteLine("Length: " + songList[t].songLength + "s");
                Console.WriteLine("Bit rate: " + songList[t].bitRate + "kbps");
                Console.WriteLine("Size: " + songList[t].size + "kB\n");
            }
        }

        private static songInfo[] songList;
        private static int FILELISTLENGTH;
        private static int artistSort = -1;  //Change to 0 for ascending, 1 for descending.
        private static int albumSort = -1;
        private static int titleSort = -1;
        private static int yearSort = -1;


        static void resetSorts()
        {
            artistSort = -1;
            albumSort = -1;
            titleSort = -1;
            yearSort = -1;
        }

        public void sortArtist()
        {
            if (artistSort != 0)
            {
                sortArtistAsc();
            }
            else
            {
                sortArtistDesc();
            }
        }

        public void sortTitle()
        {
            if (titleSort != 0)
            {
                sortTitleAsc();
            }
            else
            {
                sortTitleDesc();
            }
        }

        public void sortAlbum()
        {
            if (albumSort != 0)
            {
                sortAlbumAsc();
            }
            else
            {
                sortAlbumDesc();
            }
        }

        public void sortYear()
        {
            if (yearSort != 0)
            {
                sortYearAsc();
            }
            else
            {
                sortYearDesc();
            }
        }

        static void sortArtistAsc()
        {
            int i, j;
            bool flag = true;
            songInfo temp;
            for (i = 1; (i <= FILELISTLENGTH) && flag; i++)
            {
                flag = false;
                for (j = 0; j < (FILELISTLENGTH - 1); j++)
                {
                    if (songList[j + 1].artist.CompareTo(songList[j].artist) < 0)
                    {
                        temp = songList[j];
                        songList[j] = songList[j + 1];
                        songList[j + 1] = temp;
                        flag = true;
                    }
                }
            }
            resetSorts();
            artistSort = 0;
        }

        static void sortArtistDesc()
        {
            int i, j;
            bool flag = true;
            songInfo temp;
            for (i = 1; (i <= FILELISTLENGTH) && flag; i++)
            {
                flag = false;
                for (j = 0; j < (FILELISTLENGTH - 1); j++)
                {
                    if (songList[j + 1].artist.CompareTo(songList[j].artist) > 0)
                    {
                        temp = songList[j];
                        songList[j] = songList[j + 1];
                        songList[j + 1] = temp;
                        flag = true;
                    }
                }
            }
            resetSorts();
            artistSort = 1;
        }

        static void sortTitleAsc()
        {
            int i, j;
            bool flag = true;
            songInfo temp;
            for (i = 1; (i <= FILELISTLENGTH) && flag; i++)
            {
                flag = false;
                for (j = 0; j < (FILELISTLENGTH - 1); j++)
                {
                    if (songList[j + 1].title.CompareTo(songList[j].title) < 0)
                    {
                        temp = songList[j];
                        songList[j] = songList[j + 1];
                        songList[j + 1] = temp;
                        flag = true;
                    }
                }
            }
            resetSorts();
            titleSort = 0;
        }

        static void sortTitleDesc()
        {
            int i, j;
            bool flag = true;
            songInfo temp;
            for (i = 1; (i <= FILELISTLENGTH) && flag; i++)
            {
                flag = false;
                for (j = 0; j < (FILELISTLENGTH - 1); j++)
                {
                    if (songList[j + 1].title.CompareTo(songList[j].title) > 0)
                    {
                        temp = songList[j];
                        songList[j] = songList[j + 1];
                        songList[j + 1] = temp;
                        flag = true;
                    }
                }
            }
            resetSorts();
            titleSort = 1;
        }

        static void sortAlbumAsc()
        {
            int i, j;
            bool flag = true;
            songInfo temp;
            for (i = 1; (i <= FILELISTLENGTH) && flag; i++)
            {
                flag = false;
                for (j = 0; j < (FILELISTLENGTH - 1); j++)
                {
                    if (songList[j + 1].album.CompareTo(songList[j].album) < 0)
                    {
                        temp = songList[j];
                        songList[j] = songList[j + 1];
                        songList[j + 1] = temp;
                        flag = true;
                    }
                }
            }
            resetSorts();
            albumSort = 0;
        }

        static void sortAlbumDesc()
        {
            int i, j;
            bool flag = true;
            songInfo temp;
            for (i = 1; (i <= FILELISTLENGTH) && flag; i++)
            {
                flag = false;
                for (j = 0; j < (FILELISTLENGTH - 1); j++)
                {
                    if (songList[j + 1].album.CompareTo(songList[j].album) > 0)
                    {
                        temp = songList[j];
                        songList[j] = songList[j + 1];
                        songList[j + 1] = temp;
                        flag = true;
                    }
                }
            }
            resetSorts();
            albumSort = 1;
        }

        static void sortYearAsc()
        {
            int i, j;
            bool flag = true;
            songInfo temp;
            for (i = 1; (i <= FILELISTLENGTH) && flag; i++)
            {
                flag = false;
                for (j = 0; j < (FILELISTLENGTH - 1); j++)
                {
                    if (songList[j + 1].year.CompareTo(songList[j].year) < 0)
                    {
                        temp = songList[j];
                        songList[j] = songList[j + 1];
                        songList[j + 1] = temp;
                        flag = true;
                    }
                }
            }
            resetSorts();
            yearSort = 0;
        }

        static void sortYearDesc()
        {
            int i, j;
            bool flag = true;
            songInfo temp;
            for (i = 1; (i <= FILELISTLENGTH) && flag; i++)
            {
                flag = false;
                for (j = 0; j < (FILELISTLENGTH - 1); j++)
                {
                    if (songList[j + 1].year.CompareTo(songList[j].year) > 0)
                    {
                        temp = songList[j];
                        songList[j] = songList[j + 1];
                        songList[j + 1] = temp;
                        flag = true;
                    }
                }
            }
            resetSorts();
            yearSort = 1;
        }

        public string getPath(int index)
        {
            try
            {
                return songList[index].path;
            }
            catch (IndexOutOfRangeException e)
            {
                TextWriter errorWriter = Console.Error;
                errorWriter.WriteLine(e.Message);
                return "";
            }
        }

        public string getName(int index)
        {
            try
            {
                return songList[index].name;
            }
            catch (IndexOutOfRangeException e)
            {
                TextWriter errorWriter = Console.Error;
                errorWriter.WriteLine(e.Message);
                return "";
            }
        }

        public string getTitle(int index)
        {
            try
            {
                return songList[index].title;
            }
            catch (IndexOutOfRangeException e)
            {
                TextWriter errorWriter = Console.Error;
                errorWriter.WriteLine(e.Message);
                return "";
            }
        }

        public string getArtist(int index)
        {
            try
            {
                return songList[index].artist;
            }
            catch (IndexOutOfRangeException e)
            {
                TextWriter errorWriter = Console.Error;
                errorWriter.WriteLine(e.Message);
                return "";
            }
        }

        public string getYear(int index)
        {
            try
            {
                return songList[index].year;
            }
            catch (IndexOutOfRangeException e)
            {
                TextWriter errorWriter = Console.Error;
                errorWriter.WriteLine(e.Message);
                return "";
            }
        }

        public string getAlbum(int index)
        {
            try
            {
                return songList[index].album;
            }
            catch (IndexOutOfRangeException e)
            {
                TextWriter errorWriter = Console.Error;
                errorWriter.WriteLine(e.Message);
                return "";
            }
        }

        public songInfo getSongInfo(int index)
        {
            return songList[index];
        }

        public long getTrackLength(int index)
        {
            try
            {
                return songList[index].songLength;
            }
            catch (IndexOutOfRangeException e)
            {
                TextWriter errorWriter = Console.Error;
                errorWriter.WriteLine(e.Message);
                return 0;
            }
        }

        public long getBitRate(int index)
        {
            try
            {
                return songList[index].bitRate;
            }
            catch (IndexOutOfRangeException e)
            {
                TextWriter errorWriter = Console.Error;
                errorWriter.WriteLine(e.Message);
                return 0;
            }
        }

        public long getSize(int index)
        {
            try
            {
                return songList[index].size;
            }
            catch (IndexOutOfRangeException e)
            {
                TextWriter errorWriter = Console.Error;
                errorWriter.WriteLine(e.Message);
                return 0;
            }
        }

        static void MakeDataArray(string userFile)
        {
            DirectoryInfo di = new DirectoryInfo(userFile);
            FileInfo[] rgFiles = di.GetFiles("*.mp3");


            FILELISTLENGTH = rgFiles.Length;
            songList = new songInfo[rgFiles.Length];

            TagLib.File temp;

            int x = 0;
            foreach (FileInfo fi in rgFiles)
            {
                songList[x] = new songInfo();
                temp = TagLib.File.Create(fi.FullName);
                songList[x].artist = temp.Tag.FirstPerformer;
                songList[x].title = temp.Tag.Title;
                songList[x].year = Convert.ToString(temp.Tag.Year);
                songList[x].album = temp.Tag.Album;
                songList[x].bitRate = temp.Properties.AudioBitrate;
                songList[x].songLength = (long)temp.Properties.Duration.TotalSeconds;
                songList[x].size = fi.Length / 1024;
                songList[x].name = fi.Name;
                songList[x].path = fi.FullName;
                x++;
            }
        }

        public int GetFileCount()
        {
            return FILELISTLENGTH;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TagLib;
using TagLib.Id3v2;


namespace WpfApplication1
{

    public struct songInfo{
        public string path, name, year, album, title, artist;
        public long size, songLength, bitRate;
    }

    class Program
    {
        static songInfo[] songList;
        static int FILELISTLENGTH;
        static DirectoryInfo di;

        public Program(string directory)
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
        }

        string getName(int index)
        {
            if (index < FILELISTLENGTH)
                return songList[index].name;
            else return "No song at position " + index;
        }

        string getTitle(int index)
        {
            if (index < FILELISTLENGTH)
                return songList[index].title;
            else return "No song at position " + index;
        }

        string getArtist(int index)
        {
            if (index < FILELISTLENGTH)
                return songList[index].artist;
            else return "No song at position " + index;
        }

        string getPath(int index)
        {
            if (index < FILELISTLENGTH)
                return songList[index].path;
            else return "No song at position " + index;
        }

        string getYear(int index)
        {
            if (index < FILELISTLENGTH)
                return songList[index].year;
            else return "No song at position " + index;
        }

        string getAlbum(int index)
        {
            if (index < FILELISTLENGTH)
                return songList[index].album;
            else return "No song at position " + index;
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
                temp = TagLib.File.Create(fi.FullName);
                songList[x].artist = temp.Tag.FirstPerformer;
                songList[x].title = temp.Tag.Title;
                songList[x].year = Convert.ToString(temp.Tag.Year);
                songList[x].album = temp.Tag.Album;
                songList[x].bitRate = temp.Properties.AudioBitrate;
                songList[x].songLength = (long)temp.Properties.Duration.TotalSeconds;
                songList[x].size = fi.Length / 1024;
                x++;
            }


        }

        public int GetFileCount()
        {
            return FILELISTLENGTH;
        }
    }
}

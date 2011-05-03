using System;
using NUnit.Framework;
using TagLib;

namespace TagLib.Tests.FileFormats
{   
    [TestFixture]
    public class AsfFormatTest : IFormatTest
    {
        private static string sample_file = "samples/sample.wma";
        private static string tmp_file = "samples/tmpwrite.wma";
        private File file;
        
        [TestFixtureSetUp]
        public void Init()
        {
            file = File.Create(sample_file);
        }
    
        [Test]
        public void ReadAudioProperties()
        {
            StandardTests.ReadAudioProperties (file);
        }
        
        [Test]
        public void ReadTags()
        {
            Assert.AreEqual("WMA album", file.Tag.Album);
            Assert.AreEqual("Dan Drake", file.Tag.FirstAlbumArtist);
            Assert.AreEqual("WMA artist", file.Tag.FirstPerformer);
            Assert.AreEqual("WMA comment", file.Tag.Comment);
            Assert.AreEqual("Brit Pop", file.Tag.FirstGenre);
            Assert.AreEqual("WMA title", file.Tag.Title);
            Assert.AreEqual(5, file.Tag.Track);
            Assert.AreEqual(2005, file.Tag.Year);
        }
        
        [Test]
        public void WriteStandardTags ()
        {
            StandardTests.WriteStandardTags (sample_file, tmp_file);
        }
        
        [Test]
        public void TestCorruptionResistance()
        {
            StandardTests.TestCorruptionResistance ("samples/corrupt/a.wma");
        }
    }
}

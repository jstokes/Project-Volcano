﻿using arkane;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FMOD;

namespace TestArkane
{
    
    
    /// <summary>
    ///This is a test class for ArkaneChannelTest and is intended
    ///to contain all ArkaneChannelTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ArkaneChannelTest
    {

        private static FMOD.System system;
        private static ArkaneChannel theChannel;
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            //Create System to Test
            system = new FMOD.System();
            Factory.System_Create(ref system);
            system.init(8, INITFLAGS.NORMAL, (IntPtr)null);
        }
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            // Create New Channel to Test on
            theChannel = new ArkaneChannel(system);
            theChannel.Initialize(0);
            theChannel.LoadSample("../../../media/wave.mp3");
            theChannel.PlaySample();
        }
        
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for GetPanLevel
        ///</summary>
        [TestMethod()]
        public void GetPanLevelTest()
        {
            float expected = 0F; // TODO: Initialize to an appropriate value
            theChannel.SetPanLevel(expected);
            float actual;
            actual = theChannel.GetPanLevel();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ArkaneChannel Constructor
        ///</summary>
        [TestMethod()]
        public void ArkaneChannelConstructorTest()
        {
            Assert.IsInstanceOfType(theChannel, typeof(ArkaneChannel));
        }

        /// <summary>
        ///A test for GetHighPass
        ///</summary>
        [TestMethod()]
        public void GetHighPassTest()
        {
            float expected = 10F; // TODO: Initialize to an appropriate value
            expected = (float)(expected * 220 + 7.8);
            if (expected > 22000) expected = 22000;
            theChannel.SetHighPass(10F);
            float actual;
            actual = theChannel.GetHighPass();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetVolume
        ///</summary>
        [TestMethod()]
        public void GetVolumeTest()
        {
            float expected = .8F; // TODO: Initialize to an appropriate value
            theChannel.SetVolume(expected);
            Assert.AreEqual(FMOD.RESULT.OK, theChannel.result);
            float actual;
            actual = theChannel.GetVolume();
            Assert.AreEqual(FMOD.RESULT.OK, theChannel.result);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetLowPass
        ///</summary>
        [TestMethod()]
        public void GetLowPassTest()
        {
            float expected = 10F; // TODO: Initialize to an appropriate value
            theChannel.SetLowPass(10F);
            expected = 100 - expected;
            expected = (float)(expected * 220 + 7.8);
            float actual;
            actual = theChannel.GetLowPass();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsPlaying
        ///</summary>
        [TestMethod()]
        public void IsPlayingTest()
        {
            Assert.IsTrue(theChannel.IsPlaying());
        }

        /// <summary>
        ///A test for LoadSample
        ///</summary>
        [TestMethod()]
        public void LoadSampleTest()
        {
            Assert.AreEqual(FMOD.RESULT.OK, theChannel.result);
        }

        /// <summary>
        ///A test for Mute
        ///</summary>
        [TestMethod()]
        public void MuteTest()
        {
            theChannel.Mute();
            Assert.AreEqual(0, theChannel.GetVolume());
        }

        /// <summary>
        ///A test for PlaySample
        ///</summary>
        [TestMethod()]
        public void PlaySampleTest()
        {
            Assert.AreEqual(FMOD.RESULT.OK, theChannel.result);
        }

        /// <summary>
        ///A test for SetHighPass
        ///</summary>
        [TestMethod()]
        public void SetHighPassTest()
        {
            float input = 10F; // TODO: Initialize to an appropriate value
            theChannel.SetHighPass(input);
            input = (float)(input * 220 + 7.8);
            Assert.AreEqual(input, theChannel.GetHighPass());
        }

        /// <summary>
        ///A test for SetLowPass
        ///</summary>
        [TestMethod()]
        public void SetLowPassTest()
        {
            float input = 10F; // TODO: Initialize to an appropriate value
            theChannel.SetLowPass(input);
            input = 100 - input;
            input = (float)(input * 220 + 7.8);
            float actual = theChannel.GetLowPass();
            Assert.AreEqual(input, actual);
        }

        /// <summary>
        ///A test for SetPanLevel
        ///</summary>
        [TestMethod()]
        public void SetPanLevelTest()
        {
            float newPanLevel = 0F; // TODO: Initialize to an appropriate value
            theChannel.SetPanLevel(newPanLevel);
            float actual = theChannel.GetPanLevel();
            Assert.AreEqual(newPanLevel, actual);
        }

        /// <summary>
        ///A test for SetReverb
        ///</summary>
        [TestMethod()]
        public void SetReverbTest()
        {
            theChannel.SetReverb();
            // TODO: ???
        }

        /// <summary>
        ///A test for SetVolume
        ///</summary>
        [TestMethod()]
        public void SetVolumeTest()
        {
            float newVol = .8F; // TODO: Initialize to an appropriate value
            theChannel.SetVolume(newVol);
            float actual = theChannel.GetVolume();
            Assert.AreEqual(newVol, actual);
        }

        /// <summary>
        ///A test for SetLoop
        ///</summary>
        [TestMethod()]
        public void SetLoopTest()
        {
            bool isLoop = false;
            theChannel.SetLoop(isLoop);
            // TODO
            // I'm not sure how to test these without having access to our 
            // channel variable
            // And no, its not a good idea to make that public
        }

        /// <summary>
        ///A test for StopSample
        ///</summary>
        [TestMethod()]
        public void StopSampleTest()
        {
            theChannel.StopSample();
            Assert.AreEqual(false, theChannel.IsPlaying());
        }
    }
}

using arkane;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FMOD;

namespace TestArkane
{
    
    
    /// <summary>
    ///This is a test class for SampleTest and is intended
    ///to contain all SampleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SampleTest
    {


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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for CreateSampleFromFile
        ///</summary>
        [TestMethod()]
        public void CreateSampleFromFileTest()
        {
            System sys = null; // TODO: Initialize to an appropriate value
            Sample target = new Sample(sys); // TODO: Initialize to an appropriate value
            string fileName = string.Empty; // TODO: Initialize to an appropriate value
            target.CreateSampleFromFile(fileName);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Play
        ///</summary>
        [TestMethod()]
        public void PlayTest()
        {
            System sys = null; // TODO: Initialize to an appropriate value
            Sample target = new Sample(sys); // TODO: Initialize to an appropriate value
            Channel channel = null; // TODO: Initialize to an appropriate value
            target.Play(channel);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}

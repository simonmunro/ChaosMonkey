using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ChaosMonkey.Domain;
using ChaosMonkey.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChaosMonkey.Tests
{
    /// <summary>
    /// Summary description for BasicTests
    /// </summary>
    [TestClass]
    public class BasicTests
    {
        public BasicTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        private Ec2Factory ec2Factory;

        [TestInitialize()]
        public void MyTestInitialize()
        {
            var appConfig = ConfigurationManager.AppSettings;

            ec2Factory = new Ec2Factory(appConfig["AWSAccessKey"], appConfig["AWSSecretKey"], "ec2.eu-west-1.amazonaws.com", null);
             
        }

        [TestMethod]
        public void can_list_all_instances()
        {
            var instances = ec2Factory.ListAllInstances();
            Assert.IsTrue(instances.Count > 0);
        }

        [TestMethod]
        public void list_instances_by_tag()
        {
            var instances = ec2Factory.ListInstancesByTag("Chaos", "1");
            Assert.IsTrue(instances.Count == 1);
        }

        [TestMethod]
        public void terminate_first_chaos_instance()
        {
            var instances = ec2Factory.ListInstancesByTag("Chaos", "1");
            if (instances.Count > 0)
            {
                ec2Factory.TerminateInstance(instances[0].RunningInstance[0].InstanceId);
            }
        }

        [TestMethod]
        public void can_save_settings_to_file()
        {
            var fileName = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetTempFileName()) + ".xml");
            var settings = new Settings()
                               {
                                   AwsAccessKey = "foo",
                                   AwsSecretKey = "bar",
                                   Ec2Endpoint = "EU",
                                   Tagkey = "chaos",
                                   TagValue = "1"
                               };

            Tasks.SaveSettings(fileName, settings, null);
            Assert.IsTrue(File.Exists(fileName));
        }

        [TestMethod]
        public void can_load_settings_from_file()
        {
            var settings = Tasks.LoadSettings((typeof(BasicTests)).Assembly.Location.Replace("ChaosMonkey.Tests.dll", "Settings.xml"), null);
            Assert.IsTrue(settings.Tagkey == "chaos");
        }
    }
}

using System;
using System.Configuration;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Htec.Common.Helpers;

namespace Htec.Sandbox.Tests
{
    /// <summary>
    /// EnvironmentSetup
    /// </summary>
    public class EnvironmentSetup
    {
        #region Fields
        private readonly string failedList =
            Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, @"../../failedTestList.txt"));
        private readonly string errorFileName =
            Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, @"../../errors.txt"));
        private readonly string browser = ConfigurationManager.AppSettings["browser"];
        private readonly Uri uri = new Uri(ConfigurationManager.AppSettings["defaultUrl"]);
        #endregion

        #region Ctor

        /// <summary>
        /// Sets up the environment.
        /// </summary>
        public EnvironmentSetup()
        {
            try
            {
                this.InitializeDriver();
                Utility.Driver.Manage().Window.Maximize();
                //Utility.Driver.Manage().Window.Size = new System.Drawing.Size(2048, 1200);
            }
            catch (Exception ex)
            {
                File.AppendAllText(this.errorFileName, string.Format("{0} - {1}{2}", TestContext.CurrentContext.Test.Name, ex.Message, Environment.NewLine));
                throw;
            }
        }

        #endregion

        #region TearDown

        /// <summary>
        /// BaseTestTeardown. Reports test failure.
        /// </summary>
        [TearDown]
        protected void BaseTestTeardown()
        {
            // it current test failed
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                // take a screen shot of current page
                Utility.TakeScreenshot("Error-" + TestContext.CurrentContext.Test.FullName.Split('(')[0]);

                File.AppendAllText(this.failedList, string.Format("{0}{1}", TestContext.CurrentContext.Test.FullName, Environment.NewLine));
            }
        }

        /// <summary>
        /// One time teardown. Reports test failure and quis all browser processes.
        /// </summary>
        [OneTimeTearDown]
        protected void TestFixtureTearDown()
        {
            try
            {
                File.AppendAllText(this.errorFileName, string.Format("{0} - {1}{2}", TestContext.CurrentContext.Test.Name, "TestFixtureTearDown", Environment.NewLine));
                Utility.QuitAndDispose();
            }
            catch (Exception ex)
            {
                File.AppendAllText(this.errorFileName, string.Format("{0} - {1}{2}", TestContext.CurrentContext.Test.Name, ex.Message, Environment.NewLine));
                throw;
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes web driver.
        /// </summary>
        private void InitializeDriver()
        {
            switch (this.browser)
            {
                case "chrome":
                    Utility.InitializeChromeDriver(this.uri, this.SetChromeOptions());
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Sets Options for ChromeDriver.
        /// </summary>
        /// <returns><see cref="ChromeOptions"/></returns>
        private ChromeOptions SetChromeOptions()
        {
            ChromeOptions options = new ChromeOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal
            };
            options.AddUserProfilePreference("download.default_directory", Utility.DownloadsDirectoryPath);
            options.AddUserProfilePreference("intl.accept_languages", "en");
            options.AddUserProfilePreference("disable-popup-blocking", "true");
            options.AddArgument("--start-maximized");
            return options;
        }
        #endregion

    }
}
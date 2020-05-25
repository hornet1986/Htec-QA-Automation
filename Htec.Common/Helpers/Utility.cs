using Htec.Common.CommonPages;
using Htec.Common.Extenders;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Htec.Common.Helpers
{
    public static class Utility
    {
        #region Consts

        private const double DefaultTimeout = 3;
        private const double DefaultMaxWait = 10;
        private const int DefaultRequestTimeout = 1000 * 60 * 20;

        #endregion

        #region Properties
        /// <summary>
        /// Gets the host URI.
        /// </summary>
        /// <value>
        /// The host URI.
        /// </value>
        public static Uri HostUri { get; private set; }

        /// <summary>
        /// Gets the driver.
        /// </summary>
        /// <value>
        /// The driver.
        /// </value>
        public static OpenQA.Selenium.IWebDriver Driver { get; private set; }

        /// <summary>
        /// Gets the paths of Downloads directory.
        /// </summary>
        public static string DownloadsDirectoryPath
        {
            get { return Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\Downloads")); }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes Chrome Web Driver
        /// </summary>
        /// <param name="hostUri"></param>
        /// <param name="chromeOptions"></param>
        public static void InitializeChromeDriver(Uri hostUri, ChromeOptions chromeOptions)
        {
            Driver = new ChromeDriver(chromeOptions);
            InitDriver(hostUri, false);
        }

        /// <summary>
        /// Takes the screenshot.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Name of the saved file.</returns>
        public static string TakeScreenshot(string fileName)
        {
            return Driver.TakeScreenshot(fileName);
        }

        /// <summary>
		/// Quits and disposes this instance.
		/// </summary>
		public static void QuitAndDispose()
        {
            Driver.Quit();
            Driver.Dispose();
            KillProcesses();
        }

        /// <summary>
		/// Navigates to page.
		/// </summary>
		/// <typeparam name="T">The Type</typeparam>
		/// <returns>The Result</returns>
		public static T NavigateToPage<T>()
            where T : BasePage, new()
        {
            return Driver.NavigateToPage<T>();
        }

        /// <summary>
		/// Waits this instance.
		/// </summary>
		public static void Wait()
        {
            Wait(DefaultTimeout);
        }

        /// <summary>
        /// Waits the specified seconds.
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        public static void Wait(double seconds)
        {
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
        }

        /// <summary>
        /// Waits for.
        /// </summary>
        /// <param name="condition">The condition.</param>
        public static void WaitFor(Func<bool> condition)
        {
            WaitFor(condition, DefaultMaxWait);
        }

        /// <summary>
        /// Waits for.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="maxWaitSeconds">The max wait seconds.</param>
        public static void WaitFor(Func<bool> condition, double maxWaitSeconds)
        {
            WaitFor(condition, maxWaitSeconds, null);
        }

        /// <summary>
        /// Waits for.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="maxWaitSeconds">The max wait seconds.</param>
        /// <param name="errorMessage">The error message.</param>
        public static void WaitFor(Func<bool> condition, double maxWaitSeconds, string errorMessage)
        {
            WaitFor(condition, maxWaitSeconds, 0.5, errorMessage);
        }

        /// <summary>
        /// Waits for.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="maxWaitSeconds">The max wait seconds.</param>
        /// <param name="pullIntervalSeconds">The pull interval seconds.</param>
        /// <param name="errorMessage">The error message.</param>
        public static void WaitFor(Func<bool> condition, double maxWaitSeconds, double pullIntervalSeconds, string errorMessage)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            while (sw.ElapsedMilliseconds < maxWaitSeconds * 1000)
            {
                try
                {
                    if (condition())
                    {
                        break;
                    }
                }
                catch (StaleElementReferenceException)
                {
                    // ignore exception
                }
                catch (NoSuchElementException)
                {
                    // ignore exception
                }
                catch (TargetInvocationException)
                {
                    // ignore exception
                }
                catch (ElementNotVisibleException)
                {
                    // ignore exception
                }

                Wait(pullIntervalSeconds);
            }

            Assert.IsTrue(condition(), errorMessage ?? "Wait for expired without reaching true condition.");
        }

        /// <summary>
        /// Waits for the element to be clickable
        /// </summary>
        /// <param name="waitTime">Wait time</param>
        /// <param name="webElement">The element</param>
        public static void WaitForElementToBeClickable(int waitTime, IWebElement webElement)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(waitTime));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(ElementNotInteractableException));
            wait.Until(ExpectedConditions.ElementToBeClickable(webElement));
        }

        /// <summary>
        /// Waits 5 seconds for element to be clickable
        /// </summary>
        /// <param name="webElement">The element</param>
        public static void WaitForElementToBeClickable(IWebElement webElement)
        {
            WaitForLoad();
            WaitForElementToBeClickable(8, webElement);
        }


        /// <summary>
        /// Waits for page to be fully loaded, including the Ajax calls
        /// </summary>
        public static void WaitForLoad()
        {
            IJavaScriptExecutor executorJS = (IJavaScriptExecutor)Driver;
            WaitFor(() => executorJS.ExecuteScript("return document.readyState").Equals("complete"), 30, 0.1, "Page is not fully loaded");
        }

        /// <summary>
        /// Gets the URI.
        /// </summary>
        /// <param name="relativePath">The relative path.</param>
        /// <returns>The Result</returns>
        public static Uri GetUri(string relativePath)
        {
            return new Uri(HostUri, new Uri(relativePath, UriKind.Relative));
        }

        /// <summary>
        /// Checks if element is visible.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool IsElementVisible(IWebElement element)
        {
            try
            {
                return element.IsVisible();
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Initializes the driver.
        /// </summary>
        /// <param name="hostUri"></param>
        /// <param name="setTimeouts"></param>
        private static void InitDriver(Uri hostUri, bool setTimeouts)
        {
            HostUri = hostUri;

            if (setTimeouts)
            {
                Driver.Manage().Timeouts().ImplicitWait.Seconds.Equals(1);
            }
        }

        /// <summary>
        /// Kills all processes within browser instances.
        /// </summary>
        private static void KillProcesses()
        {
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
                if (process.ProcessName == "geckodriver" || process.ProcessName == "firefox")
                {
                    try
                    {
                        process.Kill();
                    }
                    catch (Exception ex)
                    {
                        Debug.Write(string.Format("Kill process failed. Message: ", ex.Message));
                    }
                }
            }

        }
        #endregion
    }
}

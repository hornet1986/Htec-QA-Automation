using System;
using System.IO;
using OpenQA.Selenium;
using Htec.Common.CommonPages;
using Htec.Common.Helpers;
using NUnit.Framework;

namespace Htec.Common.Extenders
{
    public static class WebDriverExtender
    {
        #region Methods

        /// <summary>
		/// Navigates to relative path.
		/// </summary>
		/// <param name="webDriver">The web driver.</param>
		/// <param name="relativePath">The relative path.</param>
		public static void NavigateTo(this IWebDriver webDriver, string relativePath)
        {
            Uri pageUri = Utility.GetUri(relativePath);
            webDriver.NavigateTo(pageUri);
        }

        /// <summary>
        /// Navigates to URL.
        /// </summary>
        /// <param name="webDriver">The web driver.</param>
        /// <param name="url">The URL.</param>
        public static void NavigateTo(this IWebDriver webDriver, Uri url)
        {
            webDriver.Navigate().GoToUrl(url);
        }
        /// <summary>
        /// Takes the screenshot.
        /// </summary>
        /// <param name="webDriver">The web driver.</param>
        /// <param name="fileName">Name of the file.</param>
        public static string TakeScreenshot(this IWebDriver webDriver, string fileName)
        {
            Screenshot screen = (webDriver as ITakesScreenshot).GetScreenshot();
            DirectoryInfo di = CreateDirectory(Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "Screenshots")));

            string fullname = Path.Combine(di.FullName, fileName);
            string ext = ".png";

            //// The fully qualified file name must be less than 260 characters.
            int maxLenWoutExt = 260 - ext.Length;
            //// Trim filename to appropriate length.
            if (fullname.Length >= maxLenWoutExt)
            {
                fullname = fullname.Substring(0, maxLenWoutExt - 1);
            }

            string savedFileName = fullname + ext;
            screen.SaveAsFile(savedFileName, 0);
            return savedFileName;
        }

        /// <summary>
        /// Navigates to page.
        /// </summary>
        /// <typeparam name="T">The Type</typeparam>
        /// <param name="webDriver">The web driver.</param>
        /// <returns>The Result</returns>
        public static T NavigateToPage<T>(this IWebDriver webDriver)
            where T : BasePage, new()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)webDriver;
            T page = new T();
            webDriver.NavigateTo(page.RelativePath);
            webDriver.InitElements(page);

            Assert.AreEqual(true, js.ExecuteScript("return document.readyState").ToString().Equals("complete"));

            return page;
        }

        /// <summary>
		/// Initializes the page.
		/// </summary>
		/// <param name="webDriver">The web driver.</param>
		/// <param name="page">The page.</param>
		public static void InitializePage(this IWebDriver webDriver, BasePage page)
        {
            webDriver.InitElements(page);
            if (!string.IsNullOrEmpty(page.RelativePath))
            {
                webDriver.CheckPageUrl(page.RelativePath);
            }
        }

        /// <summary>
		/// Gets the current page.
		/// </summary>
		/// <typeparam name="T">The Type</typeparam>
		/// <param name="webDriver">The web driver.</param>
		/// <returns>The Result</returns>
		public static T GetCurrentPage<T>(this IWebDriver webDriver) where T : BasePage, new()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)webDriver;
            T page = new T();
            webDriver.InitializePage(page);

            Assert.AreEqual(true, js.ExecuteScript("return document.readyState").ToString().Equals("complete"));

            Utility.Driver.SwitchTo().DefaultContent();
            return page;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Creates the directory if does not exists.
        /// </summary>
        /// <param name="path">The directory to create.</param>
        /// <returns>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists</returns>	
        private static DirectoryInfo CreateDirectory(string path)
        {
            return Directory.Exists(path) ? new DirectoryInfo(path) : Directory.CreateDirectory(path);
        }

        /// <summary>
		/// Checks the page URL.
		/// </summary>
		/// <param name="webDriver">The web driver.</param>
		/// <param name="value">The value.</param>
		private static void CheckPageUrl(this IWebDriver webDriver, string value)
        {
            Utility.WaitFor(() => webDriver.Url.ToUpper().Contains(value.ToUpper()), 30, "URL doesn't match!");
        }

        #endregion
    }
}
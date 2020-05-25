using Htec.Common.Helpers;
using OpenQA.Selenium;

namespace Htec.Common.CommonPages
{
    public abstract class BasePage
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePage" /> class.
        /// </summary>
        /// <param name="relativePath">The relative path.</param>
        protected BasePage(string relativePath)
        {
            this.RelativePath = relativePath;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the driver.
        /// </summary>
        protected IWebDriver Driver
        {
            get
            {
                return Utility.Driver;
            }
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title
        {
            get
            {
                return this.Driver.Title;
            }
        }

        /// <summary>
        /// Gets the relative path.
        /// </summary>
        /// <value>
        /// The relative path.
        /// </value>
        public string RelativePath { get; private set; }
        #endregion

    }
}

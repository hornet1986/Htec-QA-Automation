using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Htec.Common.CommonPages;
using Htec.Common.Extenders;
using Htec.Common.Helpers;

namespace Htec.Sandbox.Core.Pages
{
    public class HomePage : BasePage
    {
        #region HtmlElements

        [FindsBy(How = How.ClassName, Using = "btn-secondary")]
        private IWebElement login_button;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes Home Page object.
        /// </summary>
        public HomePage()
            : base("")
        {

        }
        #endregion

        #region Methods
        public LoginPage ClickOnLoginButton()
        {
            this.login_button.Click();
            Utility.WaitFor(() => Utility.Driver.Url.ToLower().Contains("/login"), 2);
            return Utility.Driver.GetCurrentPage<LoginPage>();
        }
        #endregion
    }
}

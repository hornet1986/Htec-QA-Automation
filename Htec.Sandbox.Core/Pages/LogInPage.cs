using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Htec.Common.CommonPages;
using Htec.Common.Extenders;
using Htec.Common.Helpers;

namespace Htec.Sandbox.Core.Pages
{
    public class LoginPage : BasePage
    {
        #region HtmlElements

        [FindsBy(How = How.Name, Using = "email")]
        private IWebElement email_input;

        [FindsBy(How = How.Name, Using = "password")]
        private IWebElement password_input;

        [FindsBy(How = How.XPath, Using = "//button[@data-testid='submit_btn']")]
        private IWebElement submit_button;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes Login Page object
        /// </summary>
        public LoginPage()
            : base("/login")
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets email.
        /// </summary>
        public string Email
        {
            get
            {
                return this.email_input.GetValue();
            }
            set
            {
                this.email_input.Clear();
                this.email_input.SendKeys(value);
            }
        }

        /// <summary>
        /// Gets and sets password.
        /// </summary>
        public string Password
        {
            get { return this.password_input.GetValue(); }
            set
            {
                this.password_input.Clear();
                this.password_input.SendKeys(value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clicks on Submit button.
        /// </summary>
        public void ClickOnSubmit()
        {
            this.submit_button.Click();
            Utility.WaitFor(() => Utility.Driver.Url.ToLower().Contains("/dashboard"), 2);
        }

        #endregion
    }
}
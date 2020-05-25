using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Htec.Common.CommonPages;
using Htec.Common.Extenders;
using Htec.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Htec.Sandbox.Core.Pages
{
    public class DashboardPage : BasePage
    {
        #region HtmlElements

        [FindsBy(How = How.XPath, Using = "//div[@data-testid='use_cases_card_id']")]
        private IWebElement use_case_div; // traverse to Card Object if we have time

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes Dashboard page object.
        /// </summary>
        public DashboardPage()
            : base("/dashboard")
        {

        }

        #endregion

        #region Methods

        public UseCasesListPage ClickOnUseCases()
        {
            this.use_case_div.Click();
            Utility.WaitFor(() => Utility.Driver.Url.ToLower().Contains("/use-cases"), 2);
            return Utility.Driver.GetCurrentPage<UseCasesListPage>();
        }

        #endregion
    }
}

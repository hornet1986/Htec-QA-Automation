using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support;
using Htec.Common.CommonPages;
using Htec.Common.Extenders;
using Htec.Common.Helpers;

namespace Htec.Sandbox.Core.Pages
{
    public class UseCasesListPage : BasePage
    {
        #region HtmlElements

        [FindsBy(How = How.XPath, Using = "//a[@data-testid='create_use_case_btn']")]
        private IWebElement creteUseCaseButton;

        [FindsBy(How = How.XPath, Using = "//a[@class='list-group-item list-group-item-action']")]
        private IList<IWebElement> useCasesList;

        #endregion

        #region Ctor

        public UseCasesListPage()
            :base("/use-cases")
        {

        }

        #endregion

        #region Properties

        public int NumberOfUseCases
        {
            get
            {
                return this.useCasesList.Count();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates new use case in the repository.
        /// </summary>
        public UseCasePage ClickOnCreateUseCase()
        {
            this.creteUseCaseButton.Click();
            Utility.WaitForLoad();
            return Utility.Driver.GetCurrentPage<UseCasePage>();
        }

        public void EditUseCase(string caseId)
        {
            StringBuilder stringBuilder = new StringBuilder("edited ");
            stringBuilder.Append(Convert.ToBase64String(Guid.NewGuid().ToByteArray()));
            stringBuilder.ToString();

            IWebElement useCase = Utility.Driver.FindElement(By.XPath(string.Format("//a[@href='/use-cases/{0}']", caseId)));

            useCase.Click();
            Utility.Wait(1);
            UseCasePage useCasePage = Utility.Driver.GetCurrentPage<UseCasePage>();
            useCasePage.UseCaseTitle = string.Format("{0} {1}", useCasePage.UseCaseTitle, stringBuilder);
            useCasePage.Description = string.Format("{0} {1}", useCasePage.Description, stringBuilder);
            useCasePage.ExpectedResult = string.Format("{0} {1}", useCasePage.ExpectedResult, stringBuilder);

            IReadOnlyCollection<IWebElement> steps = Utility.Driver.FindElements(By.Id("stepId"));

            foreach (var step in steps)
            {
                step.SendKeys(stringBuilder.ToString());
            }

            useCasePage.ClickOnSubmitButton();
            Utility.Wait(2);
        }

        #endregion
    }
}

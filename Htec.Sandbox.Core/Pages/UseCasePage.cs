using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Htec.Common.CommonPages;
using Htec.Common.Extenders;
using Htec.Common.Helpers;

namespace Htec.Sandbox.Core.Pages
{
    public class UseCasePage : BasePage
    {
        #region HtmlElements

        [FindsBy(How = How.XPath, Using = "//button[@data-testid='remove_usecase_btn']")]
        private IWebElement remove_use_case_button;

        [FindsBy(How = How.Name, Using = "title")]
        private IWebElement tittle_input;

        [FindsBy(How = How.Name, Using = "description")]
        private IWebElement description_text_area;

        [FindsBy(How = How.Name, Using = "expected_result")]
        private IWebElement expected_result_input;

        [FindsBy(How = How.XPath, Using = "//input[@data-testid='automated-switch']")]
        private IWebElement is_automated_checkbox;

        [FindsBy(How = How.XPath, Using = "//button[@class='btn btn-light mb-3 addTestStep ']")]
        private IWebElement add_step_button;

        [FindsBy(How = How.XPath, Using = "//div[@class='sweet-alert ']")]
        private IWebElement delete_use_case_div;

        [FindsBy(How = How.Id, Using = "stepId")]
        private IWebElement use_case_step_input;

        [FindsBy(How = How.XPath, Using = "//button[@data-testid='submit_btn']")]
        private IWebElement submit_button;
        #endregion

        #region Ctor

        public UseCasePage()
            : base("")
        {

        }

        #endregion

        #region Properties

        public string UseCaseTitle
        {
            get
            {
                return this.tittle_input.GetValue();
            }
            set
            {
                this.tittle_input.Clear();
                this.tittle_input.SendKeys(value);
            }
        }

        public string Description
        {
            get
            {
                return this.description_text_area.GetValue();
            }
            set
            {
                this.description_text_area.Clear();
                this.description_text_area.SendKeys(value);
            }
        }

        public string ExpectedResult
        {
            get
            {
                return this.expected_result_input.GetValue();
            }
            set
            {
                this.expected_result_input.Clear();
                this.expected_result_input.SendKeys(value);
            }
        }

        public bool IsAutomated
        {
            get
            {
                return this.is_automated_checkbox.Selected;
            }
            set
            {
                this.is_automated_checkbox.SetCheckBoxState(value);
            }
        }

        public string Step
        {
            get
            {
                return this.use_case_step_input.GetValue();
            }
            set
            {
                this.use_case_step_input.Clear();
                this.use_case_step_input.SendKeys(value);
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Clicks on remove use case button.
        /// </summary>
        public void ClicksOnRemoveUseCaseButton()
        {
            Utility.WaitFor(() => Utility.IsElementVisible(remove_use_case_button), 1);
            this.remove_use_case_button.Click();
            Utility.WaitFor(() => Utility.IsElementVisible(delete_use_case_div), 1);
            delete_use_case_div.FindElement(By.XPath("//button[@class='btn btn-lg btn-danger ']")).Click();
        }

        /// <summary>
        /// Clicks on Submit button.
        /// </summary>
        public void ClickOnSubmitButton()
        {
            this.submit_button.Click();
            Utility.WaitFor(() => Utility.Driver.Url.ToLower().Contains("/use-cases"), 2);
        }

        #endregion
    }
}

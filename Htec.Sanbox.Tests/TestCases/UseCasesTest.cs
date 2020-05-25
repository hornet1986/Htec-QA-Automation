using NUnit.Framework;
using Htec.Sandbox.Core.Pages;
using Htec.Sandbox.Core.UseCases;
using Htec.Common.Helpers;
using Htec.Common.Extenders;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium.Support.UI;
using System;
using OpenQA.Selenium;
using System.Linq;

namespace Htec.Sandbox.Tests
{
    [TestFixture]
    [Category("UseCases")]
    public class UseCasesTest : BaseTest
    {
        [Test]
        public static void CreateUseCase()
        {
            // Gets the use cases from json file
            UseCaseManager useCaseManager = new UseCaseManager();
            List<UseCaseModel> useCases = useCaseManager.LoadUseCases();

            CommonActions.LoginFromHomePage();
            DashboardPage dashboardPage = Utility.Driver.GetCurrentPage<DashboardPage>();
            UseCasesListPage useCasesListPage = dashboardPage.ClickOnUseCases();

            foreach (var useCase in useCases)
            {
                UseCasePage useCasePage = useCasesListPage.ClickOnCreateUseCase();
                useCasePage.UseCaseTitle = useCase.Title;
                useCasePage.Description = useCase.Description;
                useCasePage.ExpectedResult = useCase.ExpectedResult;
                useCasePage.IsAutomated = useCase.isAutomated;

                StringBuilder stringBuilder = new StringBuilder();

                int lastIndex = useCase.Steps.Count - 1;

                for (int i = 0; i <= lastIndex; i++)
                {
                    string step = useCase.Steps[i];
                    if (lastIndex != i)
                    {
                        stringBuilder.Append(step + ",");
                    }
                    else
                    {
                        stringBuilder.Append(step);
                    }
                }
                useCasePage.Step = stringBuilder.ToString();
                useCasePage.ClickOnSubmitButton();

            }

            Assert.AreEqual(useCases.Count, useCasesListPage.NumberOfUseCases);
        }

        [Test]
        public void EditUseCases()
        {
            CommonActions.LoginFromHomePage();
            DashboardPage dashboardPage = Utility.Driver.GetCurrentPage<DashboardPage>();
            Utility.Wait(2);//this could be avoided by adding wait on loader, but I was sleepy when I was working on this :)
            UseCasesListPage useCasesListPage = dashboardPage.ClickOnUseCases();
            Utility.Wait(2);

            IReadOnlyCollection<IWebElement> useCases = Utility.Driver.FindElements(By.XPath("//a[@class='list-group-item list-group-item-action']"));

            List<string> useCaseIds = new List<string>();

            foreach (IWebElement useCase in useCases)
            {
                string useCaseId = useCase.GetAttribute("href").Split('/').Last();
                useCaseIds.Add(useCaseId);
            }

            foreach (string useCaseId in useCaseIds)
            {
                useCasesListPage.EditUseCase(useCaseId);
            }
        }

        //[Test]
        //public static void DeleteUseCases()
        //{
        //    CommonActions.LoginFromHomePage();
        //    DashboardPage dashboardPage = Utility.Driver.GetCurrentPage<DashboardPage>();
        //    Utility.Wait(2);
        //    UseCasesListPage useCasesListPage = dashboardPage.ClickOnUseCases();
        //    Utility.Wait(2);
        //    do
        //    {
        //        int counter = Utility.Driver.FindElements(By.XPath("//a[@class='list-group-item list-group-item-action']")).Count();
        //        if (counter == 0)
        //        {
        //            break;
        //        }
        //        WebDriverWait webDriverWait = new WebDriverWait(Utility.Driver, TimeSpan.FromSeconds(2));
        //        IWebElement useCase = webDriverWait.Until(driver => driver.FindElement(By.XPath("//a[@class='list-group-item list-group-item-action']")));
        //        useCase.Click();
        //        Utility.Wait(2);
        //        UseCasePage useCasePage = Utility.Driver.GetCurrentPage<UseCasePage>();
        //        useCasePage.ClicksOnRemoveUseCaseButton();
        //        Utility.Wait(2);
        //    } while (true);

        //    Assert.AreEqual(0, useCasesListPage.NumberOfUseCases);
        //}
    }
}

using Htec.Sandbox.Core.Pages;
using Htec.Common.Helpers;
using System.Configuration;

namespace Htec.Sandbox.Tests
{
    public static class CommonActions
    {
        public static void LoginFromHomePage()
        {
            HomePage homePage = Utility.NavigateToPage<HomePage>();
            LoginPage loginPage = homePage.ClickOnLoginButton();
            loginPage.Email = ConfigurationManager.AppSettings["username"];
            loginPage.Password = ConfigurationManager.AppSettings["password"];
            loginPage.ClickOnSubmit();
        }
    }
}
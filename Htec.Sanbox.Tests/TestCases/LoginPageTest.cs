using NUnit.Framework;

namespace Htec.Sandbox.Tests
{
    [TestFixture]
    [Category("Login")]
    public class LoginPageTest : BaseTest
    {
        [Test]
        public static void LoginToQaSandbox()
        {
            CommonActions.LoginFromHomePage();
        }
    }
}
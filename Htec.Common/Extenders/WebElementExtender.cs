using Htec.Common.Helpers;
using OpenQA.Selenium;

namespace Htec.Common.Extenders
{
    public static class WebElementExtender
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="webElement">The web element.</param>
        /// <returns>The Result</returns>
        public static string GetValue(this IWebElement webElement)
        {
            return webElement.GetAttribute("value");
        }

        /// <summary>
        /// Sets checkbox.
        /// </summary>
        /// <param name="webElement"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static bool SetCheckBoxState(this IWebElement webElement, bool state)
        {
            if (state != webElement.Selected)
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)Utility.Driver;
                js.ExecuteScript("arguments[0].click();", webElement);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified web element is visible.
        /// </summary>
        /// <param name="webElement">The web element.</param>
        /// <returns>
        ///   <c>true</c> if the specified web element is visible; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsVisible(this IWebElement webElement)
        {
            try
            {
                return webElement.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        }
    }
}
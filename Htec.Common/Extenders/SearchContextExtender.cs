using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Htec.Common.Extenders
{
    public static class SearchContextExtender
    {
        /// <summary>
		/// Initialize the elements.
		/// </summary>
		/// <param name="searchContext">The search context.</param>
		/// <param name="page">The page.</param>
		public static void InitElements(this ISearchContext searchContext, object page)
        {
            PageFactory.InitElements(searchContext, page);
        }
    }
}
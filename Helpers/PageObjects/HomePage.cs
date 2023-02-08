using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolStatusAutomation.Helpers.PageObjects
{
    public class HomePage : ElementUtility
    {
        private string _schoolStatusHomePage = "https://www.schoolstatus.com/";
        private string _schoolStatusHomePageHowItWorksButton = "#hero > div.inner-wrapper > div > div.copy > a.button.button-white";
        private IWebDriver driver;

        public HomePage(IWebDriver driver)
        {
            this.driver = driver;
        }

        #region Homepage functions

        /// <summary>
        /// Goes to SchoolStatus HomePage and wait until an element is visible, confirming it's loaded.
        /// </summary>
        /// <returns>A boolean indicating success</returns>
        public async Task<bool> GoToHomePage()
        {
            try
            {
                // Navigate to Homepage
                driver.Navigate().GoToUrl(_schoolStatusHomePage);
                driver.Manage().Window.Maximize();
                await PauseAsync(300);

                // Make sure there's a button visible before we assume the page has loaded.
                GetElement(driver, By.CssSelector(_schoolStatusHomePageHowItWorksButton), 15);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion Homepage functions
    }
}

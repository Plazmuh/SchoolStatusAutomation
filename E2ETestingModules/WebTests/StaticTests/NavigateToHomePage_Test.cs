using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using SchoolStatusAutomation.Helpers.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SchoolStatusAutomation.E2ETestingModules.WebTests.StaticTests
{
    [TestFixture(typeof(ChromeDriver))]
    [Parallelizable(ParallelScope.Fixtures)]
    [Author("Raymond Dasilva", "raymond.dasilva@outlook.com")]
    public class NavigateToHomePage_Test<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver Driver;

        #region Test Case

        [OneTimeSetUp]
        public void CreateDriver()
        {

            if (typeof(TWebDriver).Name == "ChromeDriver")
            {
                try
                {
                    Driver = new ChromeDriver();
                }
                catch (Exception exception)
                {
                    if (exception.Message.Contains("This version of ChromeDriver"))
                    {
                        Console.WriteLine("Please update your machines Chrome to the latest by:" +
                            "Opening Chrome -> Settings (3-dots) in upper right corner -> Settings -> Update Chrome");
                        Debug.WriteLine("Please update your machines Chrome to the latest by:" +
                            "Opening Chrome -> Settings (3-dots) in upper right corner -> Settings -> Update Chrome");
                    }

                    throw new Exception("Incorrect version of Chrome Installed... Please update your machines Chrome to the latest by:" +
                        "Opening Chrome -> Settings (3-dots) in upper right corner -> Settings -> Update Chrome... If this doesn't work," +
                        "Please message me.");
                }
            }
            else
                Driver = new TWebDriver();
        }

        /// <summary>
        /// Initializes Driver, and heads to desired Homepage for Testing
        /// </summary>
        /// <returns></returns>
        [SetUp]
        public async Task SetUpTestAsync()
        {
            try
            {
                var Page = new HomePage(Driver);
                await Page.GoToHomePage().ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                Assert.Fail($"Failed going to Home Page... Error: {exception.Message}");
            }
        }

        /// <summary>
        /// Tear the Driver down once done
        /// </summary>
        /// <returns></returns>
        [TearDown]
        public async Task TearDownTestAsync()
        {
            await ElementUtility.TearDown(Driver);
        }

        /// <summary>
        /// Test: Go through a few different pages and make sure elements are visible
        /// </summary>
        /// <returns></returns>
        [Test, Order(1)]
        public async Task NavigateToSchoolStatusHomePageAsync()
        {
            // Wait for Cookies pop-up to show, then accept. -- This will prevent future click intercepts if not handled
            var elem = ElementUtility.GetElement(Driver, By.CssSelector("#hs-eu-confirmation-button"), 60);
            elem.Click();
            await ElementUtility.PauseAsync(3000).ConfigureAwait(false);

            // Wait until "See it in action" is visible then click it -- Should go to a new page
            ElementUtility.GetElement(Driver, By.CssSelector("#hero > div.inner-wrapper > div > div.copy > a.button.modal-link.button-arrow"), 60).Click();
            await ElementUtility.PauseAsync(3000).ConfigureAwait(false);

            // Test Passed.
            Assert.Pass();
        }

        #endregion Test Case
    }
}

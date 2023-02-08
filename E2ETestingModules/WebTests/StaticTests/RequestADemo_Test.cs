using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using SchoolStatusAutomation.Helpers.PageObjects;
using System;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Diagnostics;

namespace SchoolStatusAutomation.E2ETestingModules.WebTests.StaticTests
{
    [TestFixture(typeof(ChromeDriver))]
    [Parallelizable(ParallelScope.Fixtures)]
    [Author("Raymond Dasilva", "raymond.dasilva@outlook.com")]
    public class RequestADemo<TWebDriver> where TWebDriver : IWebDriver, new()
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
        /// Test: Request a demo -
        /// </summary>
        /// <returns></returns>
        [Test, Order(1)]
        public async Task RequestADemoTestAsync()
        {
            // Wait for Cookies pop-up to show, then accept. -- This will prevent future click intercepts if not handled
            var elem = ElementUtility.GetElement(Driver, By.CssSelector("#hs-eu-confirmation-button"), 60);
            elem.Click();
            await ElementUtility.PauseAsync(3000).ConfigureAwait(false);

            // Wait until "How It Works" is visible then click it -- Should go to a new page
            ElementUtility.GetElement(Driver, By.CssSelector("#hero > div.inner-wrapper > div > div.copy > a.button.modal-link.button-arrow"), 60).Click();
            await ElementUtility.PauseAsync(3000).ConfigureAwait(false);

            // Wait until "Request a Demo" is visible then click it -- Should go to a new page, click Esc first
            ElementUtility.GetElement(Driver, By.CssSelector("#hero > div.inner-wrapper > div > div.copy > a.button.modal-link.button-arrow"), 60).SendKeys(Keys.Enter);
            await ElementUtility.PauseAsync(4000).ConfigureAwait(false);
            ElementUtility.GetElement(Driver, By.CssSelector("#hero > div.inner-wrapper > div > div.copy > a"), 60).Click();
            ElementUtility.GetElement(Driver, By.CssSelector("#hero > div.inner-wrapper > div > div.copy > a"), 60).Click();
            await ElementUtility.PauseAsync(3000).ConfigureAwait(false);


            // Wait for fields to be visible, then send appropriate keys.
            string firstNameFieldCss = "#firstname-8f440650-2a99-49ee-9509-d421a78cd7a8_7940";
            string lastNameFieldCss = "#lastname-8f440650-2a99-49ee-9509-d421a78cd7a8_7940";
            string emailFieldCss = "#email-8f440650-2a99-49ee-9509-d421a78cd7a8_7940";
            string companyFieldCss = "#company-8f440650-2a99-49ee-9509-d421a78cd7a8_7940";
            string jobTitleFieldCss = "#jobtitle-8f440650-2a99-49ee-9509-d421a78cd7a8_7940";

            string[] fields = new string[5]
            {
                firstNameFieldCss,
                lastNameFieldCss,
                emailFieldCss,
                companyFieldCss,
                jobTitleFieldCss
            };
            if (!ElementUtility.GetElements(Driver, fields, 60))
                Assert.Fail("Not all fields are visible in SchoolStatus Request Demo...");

            // Send keys to each of the fields
            ElementUtility.GetElement(Driver, By.CssSelector(firstNameFieldCss), 60).SendKeys("Raymond");
            ElementUtility.GetElement(Driver, By.CssSelector(lastNameFieldCss), 60).SendKeys("Dasilva");
            ElementUtility.GetElement(Driver, By.CssSelector(emailFieldCss), 60).SendKeys("raymond.dasilva@outlook.com");
            ElementUtility.GetElement(Driver, By.CssSelector(companyFieldCss), 60).SendKeys("SchoolStatus Automation");
            ElementUtility.GetElement(Driver, By.CssSelector(jobTitleFieldCss), 60).SendKeys("SDET");

            // State Dropdown
            await ElementUtility.PauseAsync(3000).ConfigureAwait(false);
            ElementUtility.GetElement(Driver, By.CssSelector("#state-8f440650-2a99-49ee-9509-d421a78cd7a8_7940"), 60).Click();
            ElementUtility.GetElement(Driver, By.CssSelector("#state-8f440650-2a99-49ee-9509-d421a78cd7a8_7940"), 60).SendKeys("F");
            ElementUtility.GetElement(Driver, By.CssSelector("#state-8f440650-2a99-49ee-9509-d421a78cd7a8_7940"), 60).SendKeys(Keys.Enter);

            // Submit button -- We shouldn't click submit, as it's a demo... But the option is there.
            ElementUtility.GetElement(Driver, By.CssSelector("#hsForm_8f440650-2a99-49ee-9509-d421a78cd7a8_7940 > div.hs_submit.hs-submit > div.actions > input")).Click();

            await Task.Delay(8000);

            // Click Request a Demo -- Wait for DynamoDB/Event/ServiceBus to send message
            // TODO

            // Test Passed.
            Assert.Pass();
        }

        #endregion Test Case
    }
}

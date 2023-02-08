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

namespace SchoolStatusAutomation.Helpers
{
    public class InitializeDriver<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver Driver;

        public IWebDriver InitliazeNewWebDriver()
        {
            if (typeof(TWebDriver).Name == "ChromeDriver")
            {
                try
                {
                    return new ChromeDriver();
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
                return new TWebDriver();
        }
    }
}

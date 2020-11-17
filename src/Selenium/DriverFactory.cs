using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;

namespace Musketeer.Selenium
{
    public static class DriverFactory
    {

        private static EdgeOptions GetEdgeOptions()
        {
            var options = new EdgeOptions();
            options.UseChromium = true;
            return options;
        }

        public static IWebDriver Build(Browser browser, string remoteWebDriver)
        {
            if (string.IsNullOrEmpty(remoteWebDriver))
            {
                return browser switch
                {
                    Browser.CHROME => new ChromeDriver(),
                    Browser.FIREFOX => new FirefoxDriver(),
                    Browser.INTERNET_EXPORER => new InternetExplorerDriver(),
                    Browser.EDGE => new EdgeDriver(GetEdgeOptions()),
                    _ => throw new System.Exception("No valid browser")
                };
            }
            else
            {
                DriverOptions options = browser switch
                {
                    Browser.CHROME => new ChromeOptions(),
                    Browser.FIREFOX => new FirefoxOptions(),
                    Browser.INTERNET_EXPORER => new InternetExplorerOptions(),
                    Browser.EDGE => GetEdgeOptions(),
                    _ => throw new System.Exception("No valid browser")
                };
                return new RemoteWebDriver(new Uri(remoteWebDriver), options);
            }
        }

    }
}
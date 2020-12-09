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

        public static IWebDriver Build(Browser browser, string remoteWebDriver, bool useHeadlessBrowser)
        {
            if (string.IsNullOrEmpty(remoteWebDriver))
            {
                return browser switch
                {
                    Browser.CHROME => CreateChromeDriver(useHeadlessBrowser),
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
                    Browser.CHROME => CreateChromeOptions(useHeadlessBrowser),
                    Browser.FIREFOX => new FirefoxOptions(),
                    Browser.INTERNET_EXPORER => new InternetExplorerOptions(),
                    Browser.EDGE => GetEdgeOptions(),
                    _ => throw new System.Exception("No valid browser")
                };
                return new RemoteWebDriver(new Uri(remoteWebDriver), options);
            }
        }

        public static ChromeOptions CreateChromeOptions(bool useHeadlessBrowser)
        {
            // NOTE: This is needed to Run Test in Azure Pipeline using Linux
            // Source: https://forum.katalon.com/t/unable-to-execute-testcases-over-linux/18995
            ChromeOptions co = new ChromeOptions();
            if(useHeadlessBrowser) co.AddArguments("--headless");
            co.AddArguments("--no-sandbox");
            co.AddArguments("--disable-dev-shm-usage");
            return co;
        }

        public static ChromeDriver CreateChromeDriver(bool useHeadlessBrowser)
        {
            return new ChromeDriver(CreateChromeOptions(useHeadlessBrowser));
        }
    }
}
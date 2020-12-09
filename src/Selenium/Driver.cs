using System;
using System.Drawing;
using System.Threading;
using Musketeer.Config;
using Musketeer.Extensions;
using OpenQA.Selenium;

namespace Musketeer.Selenium
{
    public class Driver
    {
        [ThreadStatic]
        private static IWebDriver _driver;

        [ThreadStatic]
        public static Wait Wait;

        [ThreadStatic]
        public static LocalStorage LocalStorage;

        [ThreadStatic]
        public static SessionStorage SessionStorage;

        [ThreadStatic]
        private static IConfiguration _config;

        private static string _baseUrl;

        public static IWebDriver Current => _driver ?? throw new NullReferenceException("WebDriver is null");

        public static string BaseUrl => _baseUrl ?? throw new NullReferenceException("BaseUrl is null");

        public static string Title => Current.Title;

        public static int CurrentTimeout { get; private set; }

        public static void Init(IConfiguration config)
        {
            _config = config;
            _driver = DriverFactory.Build(config.Browser, config.RemoteWebDriver, config.UseHeadlessBrowser);
            Wait = new Wait(config.TimeOut);
            SetImplicitTimeout(config.TimeOut);
            SessionStorage = new SessionStorage();
            LocalStorage = new LocalStorage();
            CurrentTimeout = _config.TimeOut;
            _baseUrl = config.Url;
        }

        public static void Quit()
        {
            Current.Quit();
            Current.Dispose();
        }

        public static void GoTo(string path, string description = null)
        {
            var url = "";
            if (path.StartsWith("http"))
                url = path;
            else
                url = _baseUrl.UrlAppend(path);
            Current.Navigate().GoToUrl(url);
        }

        public static void SetImplicitTimeout(int timeOutInSeconds)
        {
            Current.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeOutInSeconds);
            CurrentTimeout = timeOutInSeconds;
        }

        public static void SetImplicitTimeout(TimeSpan timeSpan)
        {
            Current.Manage().Timeouts().ImplicitWait = timeSpan;
            CurrentTimeout = timeSpan.Seconds;
        }

        public static void ResetImplicitTimeout()
        {
            Current.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(_config.TimeOut);
            CurrentTimeout = _config.TimeOut;
        }

        public static string ExecuteJs(string command, object parameter)
        {
            try
            {
                var param = (parameter is Element) ? ((Element)parameter).Current : parameter;
                return ((IJavaScriptExecutor)Current).ExecuteScript(command, param).ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string ExecuteJs(string command)
        {
            try
            {
                return ((IJavaScriptExecutor)Current).ExecuteScript(command).ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string GetUserAgent() =>
            ExecuteJs("return navigator.userAgent");

        public static void Sleep(int millis) =>
            Thread.Sleep(millis);

        public static void WaitUntilPageLoaded() =>
            Wait.Until(_ => ExecuteJs("return document.readyState").Equals("complete"));

        public static void Maximize() =>
            Current.Manage().Window.Maximize();

        public static Size Size =>
            new Size
            {
                Width = ExecuteJs("return document.documentElement.clientWidth").ToInt(),
                Height = ExecuteJs("return document.documentElement.clientHeight").ToInt() 
            };

        public static void RefreshPage() =>
            Current.Navigate().Refresh();

        public static void FindAbsentElement(By by, int timeout)
        {
            var start = DateTime.Now;
            SetImplicitTimeout(TimeSpan.FromMilliseconds(500));

            while ((DateTime.Now - start).Seconds < timeout)
            {
                try
                {
                    Driver.Current.FindElement(by);
                    Driver.Sleep(500);
                }
                catch (NoSuchElementException)
                {
                    ResetImplicitTimeout();
                    return;
                }
            }
            throw new ElementFoundException($"Element never got absent {by}");
        }

        public static void FindAbsentElement(By by)
        {
            FindAbsentElement(by, _config.TimeOut);
        }

        public static void FindAbsentElements(By by)
        {
            FindAbsentElements(by, _config.TimeOut);
        }

        public static void FindAbsentElements(By by, int timeout)
        {
            var start = DateTime.Now;
            SetImplicitTimeout(TimeSpan.FromMilliseconds(500));

            while ((DateTime.Now - start).Seconds < timeout)
            {
                var elements = Current.FindElements(by);
                if (elements.Count == 0)
                {
                    ResetImplicitTimeout();
                    return;
                }
                Driver.Sleep(500);
            }
            throw new ElementFoundException("Elements never got absent {by}");
        }
    }
}
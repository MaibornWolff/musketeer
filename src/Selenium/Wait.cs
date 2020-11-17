using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;

namespace Musketeer.Selenium
{
    public class Wait
    {
        private readonly WebDriverWait _wait;
        public int CurrentTimeout { get; }

        public Wait(int waitSeconds)
        {
            CurrentTimeout = waitSeconds;
            _wait = new WebDriverWait(Driver.Current, TimeSpan.FromSeconds(waitSeconds))
            {
                PollingInterval = TimeSpan.FromMilliseconds(500)
            };

            _wait.IgnoreExceptionTypes(
              typeof(NoSuchElementException),
              typeof(ElementNotVisibleException),
              typeof(StaleElementReferenceException)
            );
        }

        public void Until(Func<IWebDriver, bool> condition, string message = "")
        {
            try
            {
                _wait.Until(condition);
            }
            catch (TimeoutException e)
            {
                if (String.IsNullOrWhiteSpace(message))
                    throw e;
                else
                    throw new Exception(message);
            }
        }
    }
}
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Musketeer.Selenium
{
    public class Element : IWebElement
    {

        private IWebElement _element;
        public IWebElement Current => _element ?? throw new NullReferenceException("Element is null");

        public By By { get; }

        private readonly int _index = -1;

        public Element(IWebElement element, By by, int index)
        {
            _element = element;
            By = by;
            _index = index;
        }

        public Element(IWebElement element, By by)
        {
            _element = element;
            By = by;
        }
        
        public Element(IWebElement element, string locator)
        {
            _element = element;
            By = GetByFromLocator(locator);
        }

        public Element(string locator)
        {
            By = GetByFromLocator(locator);
            try
            {
                _element = Driver.Current.FindElement(By);
            }
            catch(InvalidSelectorException)
            {
                throw new Exception($"{locator} is not a valid selector");
            }
        }

        
        
        public Element(By by)
        {
            try
            {
                _element = Driver.Current.FindElement(by);
            }
            catch(InvalidSelectorException)
            {
                throw new Exception($"{by} is not a valid selector");
            }
            By = by;
        }

        private By GetByFromLocator(string locator)
        {
            if (locator.StartsWith("//") || locator.StartsWith(".."))
                return By.XPath(locator);

            return By.CssSelector(locator);
        }
        
        public void ReloadElement()
        {
            if(_index == -1)
            {
                _element = Driver.Current.FindElement(By);
            }
            else
            {
                var elements = Driver.Current.FindElements(By);
                if(elements.Count < _index) throw new Exception($"Reloaded element list does not contain index {_index}");
                _element = elements[_index];
            }
        }

        public string Locator()
        {
            var parts = By.ToString().Split(": ");
            if (parts[0].Contains("ClassName")) return $".{parts[1]}";
            if (parts[0].Contains("Id")) return $"#{parts[1]}";
            return parts[1];
        }

        public string TagName { get => StaleSave<string>("TagName"); }

        public string Text { get => StaleSave<string>("Text"); }

        public bool Enabled { get => StaleSave<bool>("Enabled"); }

        public bool Selected { get => StaleSave<bool>("Selected"); }

        public Point Location { get => StaleSave<Point>("Location"); }

        public Size Size { get => StaleSave<Size>("Size"); }

        public bool Displayed { get => StaleSave<bool>("Displayed"); }

        public void Clear() =>
            StaleSave(Current.Clear);

        public void Click() =>
            StaleSave(Current.Click);

        public void DoubleClick()
        {
            var actions = new Actions(Driver.Current);
            StaleSave(actions.DoubleClick(Current).Perform);
        }

        // Invoke method in stale save does not work with findElement
        public IWebElement FindElement(By by)
        {
            var retry = 1;
            while (retry <= 10)
            {
                try
                {
                    return Current.FindElement(by);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Stale element [{Locator()}] - retry {retry}");
                    ReloadElement();
                }

                retry++;
            }
            throw new StaleElementReferenceException($"Element '{Locator()}' never became unstale"); 
        }

        // Invoke method in stale save does not work with findElements
        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            var retry = 1;
            while (retry <= 10)
            {
                try
                {
                    return Current.FindElements(by);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Stale element [{Locator()}] - retry {retry}");
                    ReloadElement();
                }

                retry++;
            }
            throw new StaleElementReferenceException($"Element '{Locator()}' never became unstale"); 
        }

        public string GetAttribute(string attributeName) =>
            StaleSave(Current.GetAttribute, attributeName);

        public string GetCssValue(string propertyName) =>
            StaleSave(Current.GetCssValue, propertyName);

        public string GetProperty(string propertyName) =>
            StaleSave(Current.GetProperty, propertyName);

        public void SendKeys(string text) =>
            StaleSave(Current.SendKeys, text);

        public void Submit() =>
            StaleSave(Current.Submit);

        public T StaleSave<T>(string argument)
        {
            try
            {
                return GetValue<T>(argument);
            }
            catch(StaleElementReferenceException)
            {
                Driver.Sleep(500);
                _element = Driver.Current.FindElement(By);
                return GetValue<T>(argument);
            }
        }

        public T GetValue<T>(string argument)
        {
            switch(argument)
            {
                case "TagName":
                    return (T)Convert.ChangeType(Current.TagName, typeof(T));
                case "Text":
                    return (T)Convert.ChangeType(Current.Text, typeof(T));
                case "Enabled":
                    return (T)Convert.ChangeType(Current.Enabled, typeof(T));
                case "Displayed":
                    return (T)Convert.ChangeType(Current.Displayed, typeof(T));
                case "Location":
                    return (T)Convert.ChangeType(Current.Location, typeof(T));
                case "Selected":
                    return (T)Convert.ChangeType(Current.Selected, typeof(T));
                case "Size":
                    return (T)Convert.ChangeType(Current.Size, typeof(T));
                default:
                    throw new ArgumentException($"Argument {argument} not available");
            }
        }

        private void StaleSave<T>(Action<T> action, T parameter)
        {
            try
            {
                action(parameter);
            }
            catch(StaleElementReferenceException)
            {
                Driver.Sleep(1000);
                _element = FindElement(By);
                action(parameter);
            }
        }

        private void StaleSave(Action action)
        {
            try
            {
                action();
            }
            catch (StaleElementReferenceException)
            {
                Driver.Sleep(1000);
                _element = FindElement(By);
                action();
            }
        }

        private TResult StaleSave<T, TResult>(Func<T, TResult> func, T parameter, int numberOfRetries = 10)
        {
            if(numberOfRetries < 1) throw new Exception("Retries for stale elements needs to be at least 1");
            var currentRetry = 1;
            while(currentRetry <= numberOfRetries)
            {
                try
                {
                    return (TResult)Convert.ChangeType(typeof(IWebElement).GetMethod(func.Method.Name)?.Invoke(_element, new object[] {parameter}), typeof(TResult));
                }
                catch(Exception e)
                {
                    var message = $"Stale element '{Locator()}";
                    if(_index > -1) message += $" - index {_index}";
                    message += $"' - retry {currentRetry}";
                    Console.WriteLine(message);
                    Driver.Sleep(500);
                    ReloadElement();
                }
                currentRetry++;
            }
            throw new StaleElementReferenceException($"Element '{Locator()}' never became unstale");
        }

        public bool IsFullyDisplayedOnScreen()
        {
            var isVerticallyOnScreen = Driver.Size.Width - Current.Location.X >= Current.Size.Width;
            var isHorizontallyOnScreen = Driver.Size.Height - Current.Location.Y >= Current.Size.Height;
            return isVerticallyOnScreen && isHorizontallyOnScreen;
        }
    }
}
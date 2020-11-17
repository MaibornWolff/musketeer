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
        public IWebElement Current => _element ?? throw new System.NullReferenceException("Element is null");

        public By By { get; private set; }

        public Element(IWebElement element, By by)
        {
            _element = element;
            By = by;
        }

        public Element(By by)
        {
            _element = Driver.Current.FindElement(by);
            By = by;
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

        public IWebElement FindElement(By by)
        {
            return Current.FindElement(by);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return Current.FindElements(by);
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

        private TResult StaleSave<T, TResult>(Func<T, TResult> func, T parameter)
        {
            try
            {
                return func(parameter);
            }
            catch (StaleElementReferenceException)
            {
                Driver.Sleep(1000);
                _element = FindElement(By);
                return func(parameter);
            }
        }
    }
}
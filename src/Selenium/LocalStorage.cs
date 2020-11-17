using System;

namespace Musketeer.Selenium
{
    public class LocalStorage : IStorage
    {
        public string GetItem(string key) =>
            Driver.ExecuteJs($"return window.localStorage.getItem('{key}')").ToString();


        public void SetItem(string key, string value) =>
            Driver.ExecuteJs($"return window.localStorage.setItem('{key}', '{value})");

        public void RemoveItem(string key) =>
            Driver.ExecuteJs($"return window.localStorage.removeItem('{key}')");

        public string Key(int index) =>
            Driver.ExecuteJs($"return window.localStorage.key({index})").ToString();

        public int Length() =>
            Int32.Parse(Driver.ExecuteJs($"return window.localStorage.length"));

        public void Clear() =>
            Driver.ExecuteJs($"return window.localStorage.clear()");
    }
}

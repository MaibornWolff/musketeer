using System;

namespace Musketeer.Selenium
{
    public class SessionStorage : IStorage
    {
        public string GetItem(string key) =>
            Driver.ExecuteJs($"return window.sessionStorage.getItem('{key}')").ToString();


        public void SetItem(string key, string value) =>
            Driver.ExecuteJs($"return window.sessionStorage.setItem('{key}', '{value})");

        public void RemoveItem(string key) =>
            Driver.ExecuteJs($"return window.sessionStorage.removeItem('{key}')");

        public string Key(int index) =>
            Driver.ExecuteJs($"return window.sessionStorage.key({index})").ToString();

        public int Length() =>
            Int32.Parse(Driver.ExecuteJs($"return window.sessionStorage.length"));

        public void Clear() =>
            Driver.ExecuteJs($"return window.sessionStorage.clear()");
    }
}

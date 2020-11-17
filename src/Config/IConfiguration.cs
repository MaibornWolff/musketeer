using System.Collections.Generic;
using Musketeer.Selenium;

namespace Musketeer.Config
{
    public interface IConfiguration
    {
        Browser Browser { get; }
        string Url { get; }
        Dictionary<string, TestUser> TestUsers { get; }
        int TimeOut { get; }
        public string RemoteWebDriver { get; }
        public Dictionary<string, string> Custom { get; }
    }
}
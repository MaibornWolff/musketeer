using System;
using System.Collections.Generic;
using System.Linq;
using Musketeer.Config;
using Musketeer.Extensions;

namespace Musketeer.Utils
{
    public static class ConfigUtils
    {
        private static Dictionary<string, object> _parameters;

        public static void SetParameters(Dictionary<string, object> parameters) =>
            _parameters = parameters;
        
        public static string GetStringParameterOrDefault(string parameterName, string defaultValue) =>
            _parameters.ContainsKey(parameterName) ? _parameters[parameterName].ToString() : defaultValue;
        public static int GetIntParameterOrDefault(string parameterName, int defaultValue)
        {
            if (_parameters.ContainsKey(parameterName))
            {
                int intValue;
                if (Int32.TryParse(_parameters[parameterName].ToString(), out intValue))
                {
                    return intValue;
                }
                else
                {
                    var e = new ArgumentException("Invalid value for timeout is set");
                    throw e;
                }
            }
            else
            {
                return defaultValue;
            }
        }

        public static T GetEnumParameterOrDefault<T>(string parameterName, T defaultValue) where T : Enum
        {
            if (_parameters.ContainsKey(parameterName))
            {
                return (T)Enum.Parse(typeof(T), _parameters[parameterName]?.ToString()?.ToUpper() ?? "");
            }
            else
            {
                return defaultValue;
            }
        }

        public static Dictionary<string, TestUser> GetUsers()
        {
            var users = new Dictionary<string, TestUser>();

            foreach (var param in _parameters.Keys.Where(key => key.Contains("username")))
            {
                var temp = param.Split('_');
                var key = String.Join('_', temp.Skip(1));

                var username = _parameters[param].ToString();
                var password = GetStringParameterOrDefault($"password_{key}", "").ToSecureString();

                var user = new TestUser
                {
                    Username = username,
                    Password = password
                };

                users.Add(key, user);
            }
            return users;
        }

        public static Dictionary<string, string> GetCustomParameters()
        {
            var parameters = new Dictionary<string, string>();

            foreach(var param in _parameters.Keys.Where(key => key.Contains("custom_")))
            {
                var temp = param.Split("_");
                if(temp.Count() < 2 || temp[1] == "") throw new Exception($"Cannot read configuration parameter '{param}'");

                var key = String.Join('_', temp.Skip(1));

                parameters.Add(key, _parameters[param].ToString());
            }
            return parameters;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Musketeer.Config;
using Musketeer.Extensions;
using Musketeer.Logging;

namespace Musketeer.Utils
{
    public static class ConfigUtils
    {
        public static string GetStringParameterOrDefault(string parameterName, string defaultValue) =>
            Context.Con.Properties.Contains(parameterName) ? Context.Con.Properties[parameterName].ToString() : defaultValue;
        public static int GetIntParameterOrDefault(string parameterName, int defaultValue)
        {
            if (Context.Con.Properties.Contains(parameterName))
            {
                int intValue;
                if (Int32.TryParse(Context.Con.Properties[parameterName].ToString(), out intValue))
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
            if (Context.Con.Properties.Contains(parameterName))
            {
                return (T)Enum.Parse(typeof(T), Context.Con.Properties[parameterName].ToString().ToUpper());
            }
            else
            {
                return defaultValue;
            }
        }

        public static Dictionary<string, TestUser> GetUsers()
        {
            var users = new Dictionary<string, TestUser>();
            IDictionary<string, object> properties = (IDictionary<string, object>)Context.Con.Properties;

            foreach (var param in properties.Keys.Where(key => key.Contains("username")))
            {
                var temp = param.Split('_');
                var key = String.Join('_', temp.Skip(1));

                var username = Context.Con.Properties[param].ToString();
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
            IDictionary<string, object> properties = (IDictionary<string, object>)Context.Con.Properties;

            foreach(var param in properties.Keys.Where(key => key.Contains("custom_")))
            {
                var temp = param.Split("_");
                if(temp.Count() < 2 || temp[1] == "") throw new Exception($"Cannot read configuration parameter '{param}'");

                var key = String.Join('_', temp.Skip(1));

                parameters.Add(key, Context.Con.Properties[param].ToString());
            }
            return parameters;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Musketeer.Extensions
{
    public static class StringExtensions
    {
        public static SecureString ToSecureString(this string @string)
        {
            var secureString = new SecureString();
            if (@string.Length > 0)
            {
                foreach (var c in @string.ToCharArray())
                    secureString.AppendChar(c);
            }
            return secureString;
        }
        public static string ToUnsecureString(this SecureString secureString)
        {
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        public static string ToJson<T>(this T convert) =>
            JsonConvert.SerializeObject(convert, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Converters = new List<JsonConverter> { new StringEnumConverter() },
                ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() }
            });

        public static string AddRandomString(this string text, int length, string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789") =>
            text + new string(Enumerable.Range(1, length).Select(_ => chars[new Random().Next(chars.Length)]).ToArray());
        
        public static int ToInt(this string text)
        {
            try
            {
                return Int32.Parse(text); 
            }
            catch (Exception e)
            { 
                throw new Exception($"Cannot parse text '{text}' to int");
            }
        }
    }
}
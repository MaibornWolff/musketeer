using System;

namespace Musketeer.Extensions
{
    public static class UrlExtensions
    {
        public static string UrlAppend(this string url, string path)
        {
            if (url == "") return path;
            if (path == "") return url;

            if (url[^1] == '/' && path[0] == '/')
            {
                return url + path.Remove(0, 1);
            }

            if (url[^1] != '/' && path[0] != '/')
            {
                return url + '/' + path;
            }

            return url + path;
        }

        public static string UrlAppendPort(this string url, int port)
        {
            return url[^1] == '/' ? url.Remove(url.Length - 1, 1) + ':' + port : url + ':' + port;
        }

        public static string UrlAppendParameter(this string url, string key, string value)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));
            if (url.Contains("?"))
                return $"{url}&{key}={value}";
            return $"{url}?{key}={value}";
        }
    }
}
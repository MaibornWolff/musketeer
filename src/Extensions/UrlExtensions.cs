using System;

namespace Musketeer.Extensions
{
    public static class UrlExtensions
    {
        public static string UrlAppend(this String url, string path)
        {
            if (url == "") return path;
            if (path == "") return url;

            if (url[url.Length - 1] == '/' && path[0] == '/')
            {
                return url + path.Remove(0, 1);
            }
            else if (url[url.Length - 1] != '/' && path[0] != '/')
            {
                return url + '/' + path;
            }
            else
            {
                return url + path;
            }
        }

        public static string UrlAppendPort(this String url, int port)
        {
            return (url[url.Length - 1] == '/') ?
                url.Remove(url.Length - 1, 1) + ':' + port :
                url + ':' + port;
        }
    }
}
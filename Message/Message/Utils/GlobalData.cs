using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;

namespace Message.Utils
{
    static class GlobalData
    {
        public static string Cookies = null;

        private static Properties properties = null;

        private static SQLiteUtils sqliteUtils = null;

        public static void initAssets(AssetManager assets)
        {
            properties = new Properties();
            Stream stream = assets.Open("url.properties");
            properties.Load(stream);
        }

        public static void initSQLite(Context context)
        {
            sqliteUtils = new SQLiteUtils(context, "message");
        }

        public static SQLiteUtils GetSQLiteUtils()
        {
            return sqliteUtils;
        }

        public static string GetUrl(string type)
        {
            if (properties == null)
            {
                throw new Exception("配置文件初始化失败!");
            }
            return properties.GetProperty("URL") + properties.GetProperty(type);
        }

        public static void SetCookies(HttpWebRequest request)
        {
            if (request == null || Cookies == null)
            {
                return;
            }
            if (request.CookieContainer == null)
            {
                request.CookieContainer = new CookieContainer();
            }
            request.CookieContainer.SetCookies(request.RequestUri, Cookies);
        }
    }
}
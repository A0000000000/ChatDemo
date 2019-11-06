using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Message.Utils
{
    static class HttpUtils
    {
        public static string Get(string url, Dictionary<string, string> param = null)
        {
            string realUrl = url;
            if (param != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (KeyValuePair<string, string> kv in param)
                {
                    sb.Append($"{kv.Key}={kv.Value ?? ""}&");
                }
                if (sb.Length > 0)
                {
                    sb.Remove(sb.Length - 1, 1);
                    realUrl = realUrl + "?" + sb.ToString();
                }
            }
            HttpWebRequest request = WebRequest.CreateHttp(realUrl);
            GlobalData.SetCookies(request);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            StreamReader sr = new StreamReader(response.GetResponseStream());
            string res = sr.ReadToEnd();
            sr.Close();
            if (GlobalData.Cookies == null)
            {
                GlobalData.Cookies = response.Headers["Set-Cookie"];
            }
            return res;
        }
        public static string Post(string url, Dictionary<string, string> param = null)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            GlobalData.SetCookies(request);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            if (param != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (KeyValuePair<string, string> kv in param)
                {
                    sb.Append($"{kv.Key}={kv.Value ?? ""}&");
                }
                if (sb.Length > 0)
                {
                    sb.Remove(sb.Length - 1, 1);
                    byte[] data = Encoding.ASCII.GetBytes(sb.ToString());
                    request.ContentLength = data.Length;
                    request.GetRequestStream().Write(data, 0, data.Length);
                    request.GetRequestStream().Close();
                }
            }
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            StreamReader sr = new StreamReader(response.GetResponseStream());
            string res = sr.ReadToEnd();
            sr.Close();
            if (GlobalData.Cookies == null)
            {
                GlobalData.Cookies = response.Headers["Set-Cookie"];
            }
            return res;
        }

        public static void AsynGet(string url, Action<string> Function, Dictionary<string, string> param = null)
        {
            new Thread(() =>
            {
                string realUrl = url;
                if (param != null)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (KeyValuePair<string, string> kv in param)
                    {
                        sb.Append($"{kv.Key}={kv.Value ?? ""}&");
                    }
                    if (sb.Length > 0)
                    {
                        sb.Remove(sb.Length - 1, 1);
                        realUrl = realUrl + "?" + sb.ToString();
                    }
                }
                HttpWebRequest request = WebRequest.CreateHttp(realUrl);
                GlobalData.SetCookies(request);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                StreamReader sr = new StreamReader(response.GetResponseStream());
                string res = sr.ReadToEnd();
                sr.Close();
                if (GlobalData.Cookies == null)
                {
                    GlobalData.Cookies = response.Headers["Set-Cookie"];
                }
                Function(res);
            }).Start();
        }

        public static void AsynPost(string url, Action<string> Function, Dictionary<string, string> param = null)
        {
            new Thread(() =>
            {
                HttpWebRequest request = WebRequest.CreateHttp(url);
                GlobalData.SetCookies(request);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                if (param != null)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (KeyValuePair<string, string> kv in param)
                    {
                        sb.Append($"{kv.Key}={kv.Value ?? ""}&");
                    }
                    if (sb.Length > 0)
                    {
                        sb.Remove(sb.Length - 1, 1);
                        byte[] data = Encoding.ASCII.GetBytes(sb.ToString());
                        request.ContentLength = data.Length;
                        request.GetRequestStream().Write(data, 0, data.Length);
                        request.GetRequestStream().Close();
                    }
                }
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                StreamReader sr = new StreamReader(response.GetResponseStream());
                string res = sr.ReadToEnd();
                sr.Close();
                if (GlobalData.Cookies == null)
                {
                    GlobalData.Cookies = response.Headers["Set-Cookie"];
                }
                Function(res);
            }).Start();

        }
    }

}
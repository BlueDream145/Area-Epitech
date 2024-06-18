using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Area.Server.Services
{
    public class Service : IService
    {
        public static string ConcatUri(Dictionary<string, string> values, string url)
        {
            try
            {
                var postData = "";
                int index = 0;

                foreach (KeyValuePair<string, string> pair in values)
                {
                    if (index != 0)
                        postData += "&";
                    postData += pair.Key + "=" + Uri.EscapeDataString(pair.Value);
                    index++;
                }
                if (index != 0)
                    url += "?" + postData;
                return (url);
            }
            catch { return (url); }
        }

        public static string MakeRequest(string url)
        {
            Uri ourUri = new Uri(url);
            WebRequest req = WebRequest.Create(url);
            WebResponse resp = req.GetResponse();
            string responseString;

            using (Stream stream = resp.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                responseString = reader.ReadToEnd();
            }
            return (responseString);
        }
        public static string MakeGetRequest(Dictionary<string, string> values, string url, bool concat = false)
        {
            HttpWebResponse response;
            HttpWebRequest request;
            string res;

            try
            {
                if (concat)
                    url = ConcatUri(values, url);
                request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/json; charset=utf-8";
                request.Method = "GET";
                request.Headers.Add(HttpRequestHeader.CacheControl, "no-cache, no-store");
                foreach (KeyValuePair<string, string> v in values)
                    request.Headers.Add(v.Key, v.Value);
                response = (HttpWebResponse)request.GetResponse();
                res = new StreamReader(response.GetResponseStream()).ReadToEnd();
                response.Close();
                return (res);
            }
            catch { return (""); }
        }
    }
}

using Area.MobileClient.Managers;
using Area.Shared.Managers;
using Area.Shared.Protocol;
using Area.Shared.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace Area.MobileClient.Client
{
    public class SimpleClient
    {

        #region "Variables"

        private static readonly HttpClient network = new HttpClient();

        #endregion

        #region "Builder"

        public SimpleClient()
        { }

        #endregion

        #region "Methods"

        public async void Send(NetworkMessage msg)
        {
            string query = "http://" + Constants.Server_Address + ":" + Constants.Server_Port.ToString() + "/" + msg.GetType().Name.ToLower();
            PropertyInfo[] properties = msg.GetType().GetProperties();
            bool first = true;

            foreach(PropertyInfo prop in properties)
            {
                if (prop.Name.CompareTo("MessageId") == 0)
                    continue;
                if (first)
                {
                    first = false;
                    query += "?" + prop.Name.ToLower() + "=" + prop.GetValue(msg);
                } else
                {
                    query += "&" + prop.Name.ToLower() + "=" + prop.GetValue(msg);
                }
            }
            HttpWebResponse response = null;
            Stream dataStream = null;
            WebRequest request = WebRequest.Create(query);
            request.Timeout = 10000;
            request.Credentials = CredentialCache.DefaultCredentials;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
                Console.WriteLine(response.StatusDescription);
                dataStream = response.GetResponseStream();
            }
            catch (WebException)
            { return; }
            if (response == null || dataStream == null)
                return;
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            DataReceived(responseFromServer);
            reader.Close();
            dataStream.Close();
            response.Close();
        }

        public string ConcatUri(Dictionary<string, string> values, string url)
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

        public string MakeGetRequest(Dictionary<string, string> values, string url, bool concat = false)
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

        public void Dispose()
        {

        }

        #endregion

        #region "Events"

        private void DataReceived(string data)
        {
            JObject json = JObject.Parse(data);
            int id = (int)json.SelectToken("MessageId");
            KeyValuePair <Type, NetworkMessage> pair = ProtocolManager.GetMessageInstance(id);
            NetworkMessage msg = pair.Value;

            if (pair.Key == null || pair.Value == null)
                return;
            KeyValuePair<Type, MethodInfo> value = HandlersManager.GetMessageHandler(pair.Key);
            if (value.Key == null || value.Value == null)
                return;
            object classInstance = Activator.CreateInstance(value.Key, null);
            MethodInfo method = value.Value;
            msg.Deserialize(json);
            var result = method.Invoke(classInstance, new object[] { this, msg });
        }

        #endregion

    }
}

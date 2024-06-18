using Area.Server.Database.Models;
using Area.Server.Database.Tables;
using Area.Server.Managers;
using Area.Shared.Managers;
using Area.Shared.Protocol;
using Area.Shared.Protocol.Other;
using Area.Shared.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Area.Server.Network
{
    public class HttpServer
    {

        #region "Variables"

        private bool running;

        private HttpListener listener;

        private Thread acceptingthread;

        #endregion

        #region "Builder"

        public HttpServer()
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://*:" + Constants.HTTP_Port + "/");
            running = false;
        }

        #endregion

        #region "Methods"

        private void Accept()
        {
            try
            {
                listener.Start();
            } catch (HttpListenerException)
            {
                Logger.Error("Can't listen on port " + Constants.HTTP_Port);
                return;
            }
            Logger.Debug("Http listening on port " + Constants.HTTP_Port);
            while (running)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                string responseString = "";
                Uri myUri = new Uri("http://127.0.0.1:8080" + request.RawUrl);
                string className = myUri.LocalPath.Remove(0, 1);

                if (className.CompareTo("about.json") == 0)
                {
                    Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    responseString = "<HTML><BODY>" +
                        "{\"client \": { \"host\":  \"" + request.LocalEndPoint.Address.ToString() + "\" }, \"server \": { \"current_time\":  "
                        + unixTimestamp + ", \"services \": [";
                    foreach (ServiceModel service in ServiceTable.Cache)
                    {
                        responseString += service.ToString();
                        if (service != ServiceTable.Cache[ServiceTable.Cache.Count - 1])
                            responseString += ", ";
                    }
                    responseString += "] }}" + "</BODY></HTML>";
                } else
                {
                    KeyValuePair<Type, NetworkMessage> pair = ProtocolManager.GetMessageInstance(className);
                    NetworkMessage msg = pair.Value;

                    if (pair.Key == null || pair.Value == null)
                    {
                        responseString = JsonConvert.SerializeObject(new UnknowBehaviourMessage());
                    } else
                    {
                        KeyValuePair<Type, MethodInfo> value = HandlersManager.GetMessageHandler(pair.Key);
                        if (value.Key == null || value.Value == null)
                        {
                            responseString = JsonConvert.SerializeObject(new UnknowBehaviourMessage());
                        } else
                        {
                            object classInstance = Activator.CreateInstance(value.Key, null);
                            MethodInfo method = value.Value;

                            msg.Deserialize(myUri.Query);
                            var result = method.Invoke(classInstance, new object[] { msg });
                            if (result != null)
                            {
                                responseString = JsonConvert.SerializeObject(result);
                            }
                        }
                    }

                }
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
                output.Dispose();
            }
        }

        public void Start()
        {
            if (running)
                return;
            running = true;
            acceptingthread = new Thread(new ThreadStart(Accept));
            acceptingthread.Start();
        }

        public void Abort()
        {
            if (!running)
                return;
            running = false;
            acceptingthread.Abort();
        }

        public void Dispose()
        {
            Abort();
        }

        #endregion

    }
}

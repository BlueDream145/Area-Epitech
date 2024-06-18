
using Area.Shared.Protocol.Connection.Enums;
using Area.Shared.Protocol.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Shared.Protocol.Connection
{
    public class IdentificationResultMessage : NetworkMessage
    {

        public const int ProtocolId = (int)NetworkEnum.IdentificationResultMessage;

        public override int MessageId { get { return ProtocolId; } }

        public string Username { get; private set; }

        public string Name { get; private set; }

        public string Mail { get; private set; }

        public string Token { get; private set; }

        public IdentificationResultEnum Result { get; private set; }

        public List<ServiceMessage> Services { get; private set; }

        public IdentificationResultMessage(IdentificationResultEnum result)
        {
            Result = result;
            Username = "";
            Name = "";
            Mail = "";
            Token = "";
        }

        public IdentificationResultMessage(IdentificationResultEnum result, string username, string name, string mail, string token, List<ServiceMessage> services)
        {
            Result = result;
            Username = username;
            Name = name;
            Mail = mail;
            Token = token;
            Services = services;
        }

        public IdentificationResultMessage()
        {
            Result = IdentificationResultEnum.None;
        }
        public override void Deserialize(string query)
        {
            // useless
        }

        public override void Deserialize(JObject json)
        {
            Username = (string)json.SelectToken("Username");
            Name = (string)json.SelectToken("Name");
            Mail = (string)json.SelectToken("Mail");
            Token = (string)json.SelectToken("Token");
            Result = (IdentificationResultEnum)((int)json.SelectToken("Result"));
            Services = new List<ServiceMessage>();
            if (json.SelectToken("Services") != null)
            {
                try
                {
                    JArray services = (JArray)json.SelectToken("Services");
                    foreach (JObject obj in services)
                    {
                        ServiceMessage serv = new ServiceMessage();
                        serv.Deserialize(obj);
                        Services.Add(serv);
                    }
                } catch { }
            }
        }
    }
}

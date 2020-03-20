
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Shared.Protocol.Services
{
    public class ServiceListMessage : NetworkMessage
    {

        public const int ProtocolId = (int)NetworkEnum.ServiceListMessage;

        public override int MessageId { get { return ProtocolId; } }

        public List<ServiceMessage> Services { get; set; }

        public ServiceListMessage(List<ServiceMessage> services)
        {
            Services = services;
        }

        public ServiceListMessage()
        {
            Services = new List<ServiceMessage>();
        }

        public override void Deserialize(string query)
        {
            // useless
        }

        public override void Deserialize(JObject json)
        {
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
                }
                catch { }
            }
        }
    }
}

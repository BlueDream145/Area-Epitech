
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Area.Shared.Protocol.Services
{
    public class DeleteServiceMessage : NetworkMessage
    {

        public const int ProtocolId = (int)NetworkEnum.DeleteServiceMessage;

        public override int MessageId { get { return ProtocolId; } }

        public int ServiceId { get; private set; }

        public string Token { get; private set; }


        public DeleteServiceMessage(int serviceId, string token)
        {
            ServiceId = serviceId;
            Token = token;
        }
        public DeleteServiceMessage() { }

        public override void Deserialize(string query)
        {
            ServiceId = Convert.ToInt32(HttpUtility.ParseQueryString(query).Get("serviceid"));
            Token = HttpUtility.ParseQueryString(query).Get("token");
        }

        public override void Deserialize(JObject json)
        {
            // useless
        }
    }
}

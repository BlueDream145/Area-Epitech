
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Shared.Protocol.Handshake
{
    public class HelloConnectMessage : NetworkMessage
    {

        public const int ProtocolId = (int)NetworkEnum.HelloConnectMessage;

        public override int MessageId { get { return ProtocolId; } }

        public HelloConnectMessage() { }


        public override void Deserialize(string query)
        {
            // useless
        }

        public override void Deserialize(JObject json)
        {
            // useless
        }
    }
}

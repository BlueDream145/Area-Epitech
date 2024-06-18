
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Shared.Protocol.Other
{
    public class UnknowBehaviourMessage : NetworkMessage
    {

        public const int ProtocolId = (int)NetworkEnum.UnknowBehaviourMessage;

        public override int MessageId { get { return ProtocolId; } }

        public UnknowBehaviourMessage() { }


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


using Area.Shared.Protocol.Connection.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Shared.Protocol.Connection
{
    public class RegisterResultMessage : NetworkMessage
    {

        public const int ProtocolId = (int)NetworkEnum.RegisterResultMessage;

        public override int MessageId { get { return ProtocolId; } }

        public RegisterResultEnum Result { get; private set; }

        public RegisterResultMessage(RegisterResultEnum result)
        {
            Result = result;
        }

        public RegisterResultMessage()
        {
            Result = RegisterResultEnum.None;
        }
        public override void Deserialize(string query)
        {
            // useless
        }

        public override void Deserialize(JObject json)
        {
            Result = (RegisterResultEnum)((int)json.SelectToken("Result"));
        }
    }
}

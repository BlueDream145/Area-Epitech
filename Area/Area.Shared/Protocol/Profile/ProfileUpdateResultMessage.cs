
using Area.Shared.Protocol.Profile.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Shared.Protocol.Profile
{
    public class ProfileUpdateResultMessage : NetworkMessage
    {
        public const int ProtocolId = (int)NetworkEnum.ProfileUpdateResultMessage;

        public override int MessageId { get { return ProtocolId; } }

        public ProfileResultEnum Result { get; private set; }

        public ProfileUpdateResultMessage(ProfileResultEnum result)
        {
            Result = result;
        }

        public ProfileUpdateResultMessage()
        {
            Result = ProfileResultEnum.None;
        }

        public override void Deserialize(string query)
        {
            // useless
        }

        public override void Deserialize(JObject json)
        {
            Result = (ProfileResultEnum)((int)json.SelectToken("Result"));
        }
    }
}


using Area.Shared.Protocol.Reactions.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Shared.Protocol.Reactions
{
    public class ReactionResultMessage : NetworkMessage
    {

        public const int ProtocolId = (int)NetworkEnum.ReactionResultMessage;

        public override int MessageId { get { return ProtocolId; } }

        public ReactionResultEnum Result { get; set; }

        public int Reactionid { get; set; }

        public string Output { get; set; }

        public ReactionResultMessage(ReactionResultEnum result, int reactionid)
        {
            Result = result;
            Reactionid = reactionid;
        }

        public ReactionResultMessage() { }

        public override void Deserialize(string query)
        {
            // useless
        }

        public override void Deserialize(JObject json)
        {
            Result = (ReactionResultEnum)((int)json.SelectToken("Result"));
            Output = (string)json.SelectToken("Output");
            Reactionid = Convert.ToInt32(json.SelectToken("Reactionid"));
        }
    }
}


using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Area.Shared.Protocol.Reactions
{
    public class ReactionRequestMessage : NetworkMessage
    {
        public const int ProtocolId = (int)NetworkEnum.ReactionRequestMessage;

        public override int MessageId { get { return ProtocolId; } }

        public int ActionId { get; set; }

        public string Params { get; set; }

        public string Token { get; private set; }

        public ReactionRequestMessage(int actionId, string _params, string token)
        {
            ActionId = actionId;
            Token = token;
            Params = _params;
        }

        public ReactionRequestMessage()
        {
        }

        public override void Deserialize(string query)
        {
            ActionId = Convert.ToInt32(HttpUtility.ParseQueryString(query).Get("actionid"));
            Params = HttpUtility.ParseQueryString(query).Get("params");
            Token = HttpUtility.ParseQueryString(query).Get("token");
        }

        public override void Deserialize(JObject json)
        {
            // useless
        }
    }
}

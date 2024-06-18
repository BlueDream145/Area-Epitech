
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;

namespace Area.Shared.Protocol.Actions
{
    public class ActionRequestMessage : NetworkMessage
    {
        public const int ProtocolId = (int)NetworkEnum.ActionRequestMessage;

        public override int MessageId { get { return ProtocolId; } }

        public int ActionId { get; set; }

        public string Params { get; set; }

        public string Token { get; private set; }

        public ActionRequestMessage(int actionId, string _params, string token)
        {
            ActionId = actionId;
            Params = _params;
            Token = token;
        }

        public ActionRequestMessage()
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

        }
    }
}

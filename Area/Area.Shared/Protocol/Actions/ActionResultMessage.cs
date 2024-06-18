
using Area.Shared.Protocol.Actions.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Shared.Protocol.Actions
{
    public class ActionResultMessage : NetworkMessage
    {
        public const int ProtocolId = (int)NetworkEnum.ActionResultMessage;

        public override int MessageId { get { return ProtocolId; } }

        public ActionResultEnum Result { get; set; }

        public int ActionId { get; set; }

        public int ServiceId { get; set; }

        public string Output { get; set; }

        public string Param { get; set; }

        public ActionResultMessage(ActionResultEnum result, int serviceid, int actionid, string output, string param)
        {
            Result = result;
            ServiceId = serviceid;
            ActionId = actionid;
            Output = output;
            Param = param;
        }

        public ActionResultMessage() { }

        public override void Deserialize(string query)
        {
            // useless
        }

        public override void Deserialize(JObject json)
        {
            Result = (ActionResultEnum)((int)json.SelectToken("Result"));
            ActionId = Convert.ToInt32(json.SelectToken("ActionId"));
            ServiceId = Convert.ToInt32(json.SelectToken("ServiceId"));
            Output = (string)json.SelectToken("Output");
            Param = (string)json.SelectToken("Param");
        }
    }
}

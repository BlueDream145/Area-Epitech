
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Shared.Protocol.Entities
{
    public class ActionMessage : NetworkMessage
    {

        public const int ProtocolId = (int)NetworkEnum.ActionMessage;

        public override int MessageId { get { return ProtocolId; } }

        public int Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }
        public string DisplayName
        {
            get
            {
                return (string.Format("Action : {0} ({1})", Name, Description));
            }
        }

        public List<ReactionMessage> Links;

        public ActionMessage(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public ActionMessage() { }

        public override void Deserialize(string query)
        {
            // useless
        }

        public override void Deserialize(JObject json)
        {
            Id = (int)json.SelectToken("Id");
            Name = (string)json.SelectToken("Name");
            Description = (string)json.SelectToken("Description");
        }
    }
}

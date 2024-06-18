
using Area.Shared.Protocol.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Shared.Protocol.Services
{
    public class ServiceMessage : NetworkMessage
    {

        public const int ProtocolId = (int)NetworkEnum.ServiceMessage;

        public override int MessageId { get { return ProtocolId; } }

        public int Id { get; private set; }

        public string Name { get; private set; }

        public string DisplayName {
            get {
                if (Registered)
                    return (string.Format("Service name: {0} (Registered)", Name));
                else
                    return (string.Format("Service name: {0} (Not registered)", Name));
            }
        }

        public string Description { get; private set; }

        public bool Registered { get; private set; }

        public List<ActionMessage> Actions { get; private set; }

        public List<ReactionMessage> Reactions { get; private set; }

        public ServiceMessage(int id, string name, string description, bool registered, List<ActionMessage> actions, List<ReactionMessage> reactions)
        {
            Id = id;
            Name = name;
            Description = description;
            Registered = registered;
            Actions = actions;
            Reactions = reactions;
        }
        public ServiceMessage()
        {
            Actions = new List<ActionMessage>();
            Reactions = new List<ReactionMessage>();
        }

        public override void Deserialize(string query)
        {
            // useless
        }

        public override void Deserialize(JObject json)
        {
            Id = (int)json.SelectToken("Id");
            Name = (string)json.SelectToken("Name");
            Description = (string)json.SelectToken("Description");
            Registered = (bool)json.SelectToken("Registered");
            Actions = new List<ActionMessage>();
            Reactions = new List<ReactionMessage>();
            if (json.SelectToken("Actions") != null)
            {
                try
                {
                    JArray actions = (JArray)json.SelectToken("Actions");
                    foreach (JObject obj in actions)
                    {
                        ActionMessage msg = new ActionMessage();
                        msg.Deserialize(obj);
                        Actions.Add(msg);
                    }
                }
                catch { }
            }
            if (json.SelectToken("Reactions") != null)
            {
                try
                {
                    JArray reactions = (JArray)json.SelectToken("Reactions");
                    foreach (JObject obj in reactions)
                    {
                        ReactionMessage msg = new ReactionMessage();
                        msg.Deserialize(obj);
                        Reactions.Add(msg);
                    }
                }
                catch { }
            }
        }
    }
}

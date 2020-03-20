
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Area.Shared.Protocol.Connection
{
    public class IdentificationMessage : NetworkMessage
    {

        public const int ProtocolId = (int)NetworkEnum.IdentificationMessage;

        public string Username { get; private set; }

        public string Password { get; private set; }

        public override int MessageId { get { return ProtocolId; } }

        public IdentificationMessage(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public IdentificationMessage()
        {
            Username = string.Empty;
            Password = string.Empty;
        }
        public override void Deserialize(string query)
        {
            Username = HttpUtility.ParseQueryString(query).Get("username");
            Password = HttpUtility.ParseQueryString(query).Get("password");
        }

        public override void Deserialize(JObject json)
        {
            // useless
        }
    }
}

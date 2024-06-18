
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Area.Shared.Protocol.Connection
{
    public class RegisterMessage : NetworkMessage
    {

        public const int ProtocolId = (int)NetworkEnum.RegisterMessage;

        public override int MessageId { get { return ProtocolId; } }

        public string Username { get; private set; }

        public string Name { get; private set; }

        public string Mail { get; private set; }

        public string Password { get; private set; }

        public RegisterMessage() { }

        public RegisterMessage(string username, string name, string mail, string password)
        {
            Username = username;
            Name = name;
            Mail = mail;
            Password = password;
        }
        public override void Deserialize(string query)
        {
            Username = HttpUtility.ParseQueryString(query).Get("username");
            Password = HttpUtility.ParseQueryString(query).Get("password");
            Name = HttpUtility.ParseQueryString(query).Get("name");
            Mail = HttpUtility.ParseQueryString(query).Get("mail");
        }

        public override void Deserialize(JObject json)
        {
            // useless
        }
    }
}

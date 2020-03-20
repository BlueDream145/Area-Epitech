
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Area.Shared.Protocol.Profile
{
    public class ProfileUpdateRequestMessage : NetworkMessage
    {

        public const int ProtocolId = (int)NetworkEnum.ProfileUpdateRequestMessage;

        public override int MessageId { get { return ProtocolId; } }

        public string Username { get; private set; }

        public string Name { get; private set; }

        public string Mail { get; private set; }

        public string Password { get; private set; }

        public string Token { get; private set; }

        public ProfileUpdateRequestMessage() { }

        public ProfileUpdateRequestMessage(string username, string name, string mail, string password, string token)
        {
            Username = username;
            Name = name;
            Mail = mail;
            Password = password;
            Token = token;
        }

        public override void Deserialize(string query)
        {
            Username = HttpUtility.ParseQueryString(query).Get("username");
            Name = HttpUtility.ParseQueryString(query).Get("name");
            Mail = HttpUtility.ParseQueryString(query).Get("mail");
            Password = HttpUtility.ParseQueryString(query).Get("password");
            Token = HttpUtility.ParseQueryString(query).Get("token");
        }

        public override void Deserialize(JObject json)
        {
            // useless
        }

    }
}


using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Area.Shared.Protocol.Services
{
    public class RegisterServiceMessage : NetworkMessage
    {

        public const int ProtocolId = (int)NetworkEnum.RegisterServiceMessage;

        public override int MessageId { get { return ProtocolId; } }

        public int ServiceId { get; private set; }

        public string Username { get; private set; }

        public string Password { get; private set; }

        public string Token { get; private set; }

        public RegisterServiceMessage(int serviceId, string username, string password, string token)
        {
            ServiceId = serviceId;
            Username = username;
            Password = password;
            Token = token;
        }
        public RegisterServiceMessage() { }

        public override void Deserialize(string query)
        {
            ServiceId = Convert.ToInt32(HttpUtility.ParseQueryString(query).Get("serviceid"));
            Username = HttpUtility.ParseQueryString(query).Get("username");
            Password = HttpUtility.ParseQueryString(query).Get("password");
            Token = HttpUtility.ParseQueryString(query).Get("token");
        }

        public override void Deserialize(JObject json)
        {
            // useless
        }
    }
}

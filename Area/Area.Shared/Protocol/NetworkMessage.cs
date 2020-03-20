using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Shared.Protocol
{
    public abstract class NetworkMessage
    {
        public abstract int MessageId
        {
            get;
        }

        public abstract void Deserialize(string query);

        public abstract void Deserialize(JObject json);
    }
}

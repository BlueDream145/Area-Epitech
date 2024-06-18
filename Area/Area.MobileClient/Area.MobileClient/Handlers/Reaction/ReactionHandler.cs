using Area.MobileClient.Client;
using Area.Shared.Protocol.Reactions;
using Area.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.MobileClient.Handlers.Reaction
{
    public class ReactionHandler : IHandler
    {

        public void HandleReactionResultMessage(SimpleClient client, ReactionResultMessage msg)
        {
            Logger.Debug("ReactionResultMessage");
        }

    }
}

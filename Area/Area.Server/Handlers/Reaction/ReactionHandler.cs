using Area.Server.Database.Models;
using Area.Server.Database.Tables;
using Area.Shared.Entities;
using Area.Shared.Protocol.Other;
using Area.Shared.Protocol.Reactions;
using Area.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Server.Handlers.Reaction
{
    public class ReactionHandler : IHandler
    {
        public object HandleReactionRequestMessage(ReactionRequestMessage msg)
        {
            Logger.Debug("ReactionRequestMessage");
            UserModel model = UserTable.GetModelByToken(msg.Token);
            if (model == null || !msg.Params.Contains("|"))
                return new UnknowBehaviourMessage();
            string[] parts = msg.Params.Split("|", StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
                return new UnknowBehaviourMessage();
            try
            {
                int reaction = Convert.ToInt32(parts[0]);
                string param = parts[1];
                TriggerManager.HandleTrigger((TriggerEnum)msg.ActionId, (ReactionEnum)reaction, param);
                return new ReactionResultMessage(Shared.Protocol.Reactions.Enums.ReactionResultEnum.Success, msg.ActionId);
            } catch { }
            return new UnknowBehaviourMessage();
        }
    }
}

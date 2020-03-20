using Area.Server.Database.Models;
using Area.Server.Database.Tables;
using Area.Server.Managers;
using Area.Server.Services;
using Area.Shared.Entities;
using Area.Shared.Protocol.Actions;
using Area.Shared.Protocol.Actions.Enums;
using Area.Shared.Protocol.Other;
using Area.Shared.Utils;
using Newtonsoft.Json;
using Steam.Models.SteamCommunity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Area.Server.Handlers.Action
{
    public class ActionHandler : IHandler
    {
        public object HandleActionRequestMessage(ActionRequestMessage msg)
        {
            Logger.Debug("ActionRequestMessage");
            UserModel model = UserTable.GetModelByToken(msg.Token);
            ActionModel action = ActionTable.GetModelById(msg.ActionId);
            ServiceModel service = ServiceTable.GetModelByActionId(msg.ActionId);
            if (model == null || action == null || service == null)
                return new UnknowBehaviourMessage();
            AccountModel account = AccountTable.GetModelByServiceId(service.Id);
            if (account == null)
                return new UnknowBehaviourMessage();
            return (ActionDispatcher.DispatchAction(model, action, service, account, msg));
        }

    }
}

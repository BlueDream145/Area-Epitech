using Area.Server.Database.Models;
using Area.Server.Database.Tables;
using Area.Shared.Entities;
using Area.Shared.Protocol.Entities;
using Area.Shared.Protocol.Other;
using Area.Shared.Protocol.Services;
using Area.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Server.Handlers.Service
{
    public class ServiceHandler : IHandler
    {
        public static object UpdateServices(UserModel m)
        {
            List<ServiceMessage> services = new List<ServiceMessage>();

            foreach(ServiceModel model in ServiceTable.Cache)
            {
                bool registered = false;

                if (model.RegisteredUsers.Contains(m))
                    registered = true;
                List<ActionMessage> actions = new List<ActionMessage>();
                List<ReactionMessage> reactions = new List<ReactionMessage>();

                foreach (ActionModel action in model.Actions)
                    actions.Add(new ActionMessage(action.Id, action.Name, action.Description));
                foreach (ReactionModel reaction in model.Reactions)
                    reactions.Add(new ReactionMessage(reaction.Id, reaction.Name, reaction.Description));

                services.Add(new ServiceMessage(model.Id, model.Name, model.Description, registered, actions, reactions));
            }
            return (new ServiceListMessage(services));
        }

        public object HandleRegisterServiceMessage(RegisterServiceMessage msg)
        {
            Logger.Debug("RegisterServiceMessage");
            UserModel model = UserTable.GetModelByToken(msg.Token);
            if (model == null)
                return new UnknowBehaviourMessage();
            ServiceModel service = ServiceTable.GetModelById(msg.ServiceId);

            if (service == null)
                 return (new UnknowBehaviourMessage());
            if (model != null && !service.RegisteredUsers.Contains(model))
            {
                service.RegisteredUsers.Add(model);
                service.Update();
                AccountTable.InsertModel(new AccountModel((ServiceEnum)msg.ServiceId,
                    msg.Username, msg.Password, model.Id));
            }
            return (UpdateServices(model));
        }

        public object HandleDeleteServiceMessage(DeleteServiceMessage msg)
        {
            Logger.Debug("DeleteServiceMessage");
            UserModel model = UserTable.GetModelByToken(msg.Token);
            if (model == null)
                return new UnknowBehaviourMessage();
            ServiceModel service = ServiceTable.GetModelById(msg.ServiceId);

            if (service == null)
                return (new UnknowBehaviourMessage());
            if (model != null && service.RegisteredUsers.Contains(model))
            {
                service.RegisteredUsers.Remove(model);
                service.Update();
                bool done = false;
                while (!done)
                {
                    foreach (AccountModel m in AccountTable.Cache)
                        if (m.Service == msg.ServiceId && m.OwnerId == model.Id)
                        {
                            AccountTable.RemoveModel(m);
                            break;
                        }
                    done = true;
                }
                return (UpdateServices(model));
            }
            return (new UnknowBehaviourMessage());
        }

    }
}

using Area.Server.Database.Models;
using Area.Server.Database.Tables;
using Area.Server.Handlers.Service;
using Area.Shared.Protocol.Connection;
using Area.Shared.Protocol.Connection.Enums;
using Area.Shared.Protocol.Entities;
using Area.Shared.Protocol.Handshake;
using Area.Shared.Protocol.Services;
using Area.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Area.Server.Handlers.Connection
{
    public class ConnectionHandler : IHandler
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static List<ServiceMessage> GetServices(UserModel m)
        {
            List<ServiceMessage> services = new List<ServiceMessage>();

            foreach (ServiceModel model in ServiceTable.Cache)
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
            return (services);
        }


        public object HandleHelloConnectMessage(HelloConnectMessage msg)
        {
            Logger.Debug("HelloConnectMessage");
            return (new HelloConnectMessage());
        }

        public object HandleIdentificationMessage(IdentificationMessage msg)
        {
            Logger.Debug("IdentificationMessage");
            IdentificationResultEnum result = IdentificationResultEnum.None;

            try
            {
                UserModel model = UserTable.Cache.Find(f => f.Username == msg.Username);

                if (model == null || model == default(UserModel) || model.Password != msg.Password)
                    result = IdentificationResultEnum.BadCredentials;
                else
                {
                    model.Token = RandomString(12);
                    model.Update();
                    return (new IdentificationResultMessage(IdentificationResultEnum.Success, model.Username, model.Name, model.Mail, model.Token, GetServices(model)));
                }
            } catch
            {
                result = IdentificationResultEnum.InternalError;
            }
            return (new IdentificationResultMessage(result));
        }

        private RegisterResultEnum registerCheckSyntax(RegisterMessage msg)
        {
            if (!msg.Mail.Contains(".") || !msg.Mail.Contains("@"))
                return (RegisterResultEnum.InvalidMail);
            if (msg.Password.Length < 5)
                return (RegisterResultEnum.InvalidPassword);
            if (msg.Username.Length < 5)
                return (RegisterResultEnum.InvalidUsername);
            return (RegisterResultEnum.Success);
        }

        public object HandleRegisterMessage(RegisterMessage msg)
        {
            Logger.Debug("RegisterMessage");
            RegisterResultEnum result = RegisterResultEnum.None;

            try
            {
                UserModel modelUsername = UserTable.Cache.Find(f => f.Username == msg.Username);
                UserModel modelMail = UserTable.Cache.Find(f => f.Mail == msg.Mail);

                if (modelUsername == null || modelUsername == default(UserModel))
                {
                    if (modelMail == null || modelMail == default(UserModel))
                    {
                        result = registerCheckSyntax(msg);
                    }
                    else
                        result = RegisterResultEnum.MailAlreadyRegistered;
                } else
                    result = RegisterResultEnum.UsernameAlreadyRegistered;
            }
            catch
            {
                result = RegisterResultEnum.InternalError;
            }
            if (result == RegisterResultEnum.Success)
            {
                UserModel model = new UserModel(msg.Username, msg.Name, msg.Mail, msg.Password, "");
                UserTable.InsertModel(model);
            }
            return (new RegisterResultMessage(result));
        }

    }
}

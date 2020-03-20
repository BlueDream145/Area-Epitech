using Area.Server.Database.Models;
using Area.Server.Database.Tables;
using Area.Server.Handlers.Connection;
using Area.Shared.Protocol.Connection;
using Area.Shared.Protocol.Connection.Enums;
using Area.Shared.Protocol.Other;
using Area.Shared.Protocol.Profile;
using Area.Shared.Protocol.Profile.Enums;
using Area.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Server.Handlers.Profile
{
    public class ProfileHandler : IHandler
    {
        private ProfileResultEnum profileCheckSyntax(ProfileUpdateRequestMessage msg)
        {
            if (!msg.Mail.Contains(".") || !msg.Mail.Contains("@"))
                return (ProfileResultEnum.InvalidMail);
            if (msg.Password.Length < 5)
                return (ProfileResultEnum.InvalidPassword);
            if (msg.Username.Length < 5)
                return (ProfileResultEnum.InvalidUsername);
            return (ProfileResultEnum.Success);
        }

        public object HandleProfileUpdateRequestMessage(ProfileUpdateRequestMessage msg)
        {
            Logger.Debug("ProfileUpdateRequestMessage");
            UserModel model = UserTable.GetModelByToken(msg.Token);
            if (model == null)
                return new UnknowBehaviourMessage();
            ProfileResultEnum result = ProfileResultEnum.None;

            try
            {
                UserModel modelUsername = UserTable.Cache.Find(f => f.Username == msg.Username);
                UserModel modelMail = UserTable.Cache.Find(f => f.Mail == msg.Mail);

                if ((modelUsername != null && modelUsername == model) || modelUsername == null || modelUsername == default(UserModel))
                {
                    if ((modelMail != null && modelMail == model) || modelMail == null || modelMail == default(UserModel))
                    {
                        result = profileCheckSyntax(msg);
                    }
                    else
                        result = ProfileResultEnum.MailAlreadyRegistered;
                }
                else
                    result = ProfileResultEnum.UsernameAlreadyRegistered;
            }
            catch
            {
                result = ProfileResultEnum.InternalError;
            }
            if (result == ProfileResultEnum.Success)
            {
                model.Username = msg.Username;
                model.Name = msg.Name;
                model.Mail = msg.Mail;
                model.Password = msg.Password;
                model.Update();
                 return (new IdentificationResultMessage(IdentificationResultEnum.Success, model.Username,
                    model.Name, model.Mail, model.Token, ConnectionHandler.GetServices(model)));
            }
            return (new ProfileUpdateResultMessage(result));
        }

    }
}

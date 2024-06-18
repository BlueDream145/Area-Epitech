using Area.Server.Database.Models;
using Area.Server.Database.Tables;
using Area.Shared.Entities;
using Area.Shared.Protocol.Actions;
using Area.Shared.Protocol.Other;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Newtonsoft.Json;
using PastebinAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Area.Server.Services
{
    public class PastebinService : IService
    {

        public static object Paste(ServiceModel service, AccountModel account, ActionRequestMessage msg)
        {
            switch (msg.ActionId)
            {
                case ((int)ActionEnum.GetAllPastes):
                    return (new ActionResultMessage(Shared.Protocol.Actions.Enums.ActionResultEnum.Success, service.Id, msg.ActionId, PastebinService.GetAllPastes(account.Username, account.Password), msg.Params));
                case ((int)ActionEnum.CreatePaste):
                    string[] parts = msg.Params.Split("|", StringSplitOptions.RemoveEmptyEntries);
                    string r = "";

                    if (parts.Length < 2)
                        return (new ActionResultMessage(Shared.Protocol.Actions.Enums.ActionResultEnum.BadParams, service.Id, msg.ActionId, r, msg.Params));
                    return (new ActionResultMessage(Shared.Protocol.Actions.Enums.ActionResultEnum.Success, service.Id, msg.ActionId, PastebinService.CreatePaste(account.Username, account.Password, parts[0], parts[1]), msg.Params));
                default:
                    return new UnknowBehaviourMessage();
            }
        }

        public static string GetAllPastes(string username, string password)
        {
            Pastebin.DevKey = "92a8ac9492c0f44bc662cc1a7fefb472";

            try
            {
                User me = Pastebin.LoginAsync(username, password).Result;

                var res = me.ListPastesAsync(30).Result;

                //foreach (Paste paste in res) // we limmit the results to 30
                //{
                //    Console.WriteLine(paste.Title);
                //}

                return (JsonConvert.SerializeObject(res));
            }
            catch (PastebinException ex) //api throws PastebinException
            {
                if (ex.Parameter == PastebinException.ParameterType.Login)
                    return ("Invalid username/password");
                else
                    return ("Internal error");
            }
        }

        public static string CreatePaste(string title, string content)
        {
            AccountModel account = AccountTable.GetModelByServiceId((int)ServiceEnum.Pastebin);
            if (account == null)
                return (null);
            return (CreatePaste(account.Username, account.Password, title, content));
        }


        public static string CreatePaste(string username, string password, string title, string content)
        {
            Pastebin.DevKey = "92a8ac9492c0f44bc662cc1a7fefb472";

            try
            {
                User me = Pastebin.LoginAsync(username, password).Result;

                //creates a new paste and get paste object
                Paste newPaste = me.CreatePasteAsync(content, title, Language.HTML5, Visibility.Public, Expiration.OneDay).Result;

                string result = "{ \"data\":{ \"URL\":\"" + newPaste.Url + "\", \"Paste key\":\"" + newPaste.Key + "\", \"Content\":\"" + content + "\" }}";
                return (result);
            }
            catch (PastebinException ex) //api throws PastebinException
            {
                if (ex.Parameter == PastebinException.ParameterType.Login)
                    return ("Invalid username/password");
                else
                    return ("Internal error");
            }
        }

    }
}

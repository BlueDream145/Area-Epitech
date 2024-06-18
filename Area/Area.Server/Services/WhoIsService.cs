using Area.Server.Database.Models;
using Area.Shared.Entities;
using Area.Shared.Protocol.Actions;
using Area.Shared.Protocol.Other;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Server.Services
{
    public class WhoIsService : IService
    {

        public static object WhoIs(ServiceModel service, AccountModel account, ActionRequestMessage msg)
        {
            switch (msg.ActionId)
            {
                case (int)ActionEnum.CheckDomainInfos:
                    return (new ActionResultMessage(Shared.Protocol.Actions.Enums.ActionResultEnum.Success, service.Id, msg.ActionId, WhoIsService.GetWhoIs(msg.Params), msg.Params));
                default:
                    return new UnknowBehaviourMessage();
            }
        }

        public static string GetWhoIs(string tag)
        {
            if (tag == "")
                tag = "google.com";
            var url = "https://api.jsonwhois.io/whois/domain?key=JbC8ogjOImzob3SukEBL8Q2Fss3nacGY&domain=" + tag;
            return (Service.MakeRequest(url));
        }

    }
}

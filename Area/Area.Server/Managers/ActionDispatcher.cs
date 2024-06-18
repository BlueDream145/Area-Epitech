using Area.Server.Database.Models;
using Area.Server.Services;
using Area.Shared.Entities;
using Area.Shared.Protocol.Actions;
using Area.Shared.Protocol.Other;
using Newtonsoft.Json;
using Steam.Models.SteamCommunity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Area.Server.Managers
{
    public static class ActionDispatcher
    {

        public static object DispatchAction(UserModel model, ActionModel action, ServiceModel service, AccountModel account, ActionRequestMessage msg)
        {
            if (msg.ActionId < 0 || msg.ActionId >= (int)ActionEnum.Max)
                return new UnknowBehaviourMessage();
            switch ((ServiceEnum)service.Id)
            {
                case ServiceEnum.Test:
                    return new UnknowBehaviourMessage();
                case ServiceEnum.Imgur:
                    return ImgurService.Imgur(service, account, msg);
                case ServiceEnum.Currency:
                    return CurrencyService.Currency(service, account, msg);
                case ServiceEnum.News:
                    return NewsService.News(service, account, msg);
                case ServiceEnum.Weather:
                    return WeatherService.Weather(service, account, msg);
                case ServiceEnum.WhoIs:
                    return WhoIsService.WhoIs(service, account, msg);
                case ServiceEnum.Steam:
                    return SteamService.Steam(service, account, msg);
                case ServiceEnum.Gmail:
                    return GmailServ.Gmail(service, account, msg);
                case ServiceEnum.Dailymotion:
                    return DailymotionService.Dailymotion(service, account, msg);
                case ServiceEnum.Pastebin:
                    return PastebinService.Paste(service, account, msg);
                default:
                    return new UnknowBehaviourMessage();
            }
        }

    }
}

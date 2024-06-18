using Area.Server.Database.Models;
using Area.Shared.Entities;
using Area.Shared.Protocol.Actions;
using Area.Shared.Protocol.Other;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Server.Services
{
    public class DailymotionService : IService
    {

        public string AccessToken;

        public int Step;

        public bool Connected;

        public DailymotionService()
        {
            Step = 0;
            Connected = false;
        }

        public DailymotionService(string _token)
        {
            AccessToken = _token;
            Step = 0;
            Connected = false;
        }

        public string Authenticate()
        {
            if (Step == 0)
                return "";
            var values = new Dictionary<string, string>
                {
                    { "Authorization", "Bearer " + AccessToken }
                };
            string url = "https://api.dailymotion.com/authorize?token=" + AccessToken;
            string resp = Service.MakeGetRequest(values, url);
            return (resp);
        }

        public string GetVideos()
        {
            return (Service.MakeRequest("https://api.dailymotion.com/videos?channel=news&limit=20&search=france"));
        }

        public string GetVideosByTag(string tag)
        {
            return (Service.MakeRequest("https://api.dailymotion.com/videos?channel=news&limit=20&search=" + tag));
        }

        public static object Dailymotion(ServiceModel service, AccountModel account, ActionRequestMessage msg)
        {
            switch (msg.ActionId)
            {
                case (int)ActionEnum.GetVideos:
                    return (new ActionResultMessage(Shared.Protocol.Actions.Enums.ActionResultEnum.Success, service.Id, msg.ActionId, new DailymotionService().GetVideos(), msg.Params));
                case (int)ActionEnum.GetVideosByTag:
                    return (new ActionResultMessage(Shared.Protocol.Actions.Enums.ActionResultEnum.Success, service.Id, msg.ActionId, new DailymotionService().GetVideosByTag(msg.Params), msg.Params));
                default:
                    return new UnknowBehaviourMessage();
            }
        }

    }
}

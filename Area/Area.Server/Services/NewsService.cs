using Area.Server.Database.Models;
using Area.Shared.Entities;
using Area.Shared.Protocol.Actions;
using Area.Shared.Protocol.Other;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Area.Server.Services
{
    public class NewsService : IService
    {

        public static object News(ServiceModel service, AccountModel account, ActionRequestMessage msg)
        {
            switch (msg.ActionId)
            {
                case (int)ActionEnum.GetNewsByTag:
                    return (new ActionResultMessage(Shared.Protocol.Actions.Enums.ActionResultEnum.Success, service.Id, msg.ActionId, NewsService.GetNewsByTag(msg.Params), msg.Params));
                case (int)ActionEnum.GetNews:
                    return (new ActionResultMessage(Shared.Protocol.Actions.Enums.ActionResultEnum.Success, service.Id, msg.ActionId, NewsService.GetLatestNews(), msg.Params));
                default:
                    return new UnknowBehaviourMessage();
            }
        }

        private static string GetNews(string tag)
        {
            if (tag == "")
                tag = "Trump";
            var url = "https://newsapi.org/v2/everything?q=" + tag + "&from=&sortBy=publishedAt&apiKey=66e02ba63b254fc0abbb965d3045130e";
            return (Service.MakeRequest(url));
        }

        public static string GetLatestNews()
        {
            return (GetNews(""));
        }

        public static string GetNewsByTag(string tag)
        {
            return (GetNews(tag));
        }
    }
}

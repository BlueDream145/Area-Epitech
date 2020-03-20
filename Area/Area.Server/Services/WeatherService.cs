using Area.Server.Database.Models;
using Area.Shared.Entities;
using Area.Shared.Protocol.Actions;
using Area.Shared.Protocol.Other;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Area.Server.Services
{
    public class WeatherService : IService
    {

        public static object Weather(ServiceModel service, AccountModel account, ActionRequestMessage msg)
        {
            switch (msg.ActionId)
            {
                case (int)ActionEnum.GetLocalWeather:
                    return (new ActionResultMessage(Shared.Protocol.Actions.Enums.ActionResultEnum.Success, service.Id, msg.ActionId, WeatherService.GetLocalWeather(), msg.Params));
                case (int)ActionEnum.GetWeatherByLocation:
                    return (new ActionResultMessage(Shared.Protocol.Actions.Enums.ActionResultEnum.Success, service.Id, msg.ActionId, WeatherService.GetWeatherByLocation(msg.Params), msg.Params));
                default:
                    return new UnknowBehaviourMessage();
            }
        }

        private static string GetWeather(string tag)
        {
            if (tag == "")
                tag = "France";
            var url = "http://api.weatherstack.com/current?access_key=07308420b6d048c704fd499df8a3be30&query=" + tag;
            return (Service.MakeRequest(url));
        }

        public static string GetLocalWeather()
        {
            return (GetWeather(""));
        }

        public static string GetWeatherByLocation(string tag)
        {
            return (GetWeather(tag));
        }
    }
}

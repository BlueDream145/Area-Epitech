using Area.Server.Database.Models;
using Area.Shared.Entities;
using Area.Shared.Protocol.Actions;
using Area.Shared.Protocol.Other;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Server.Services
{
    public class CurrencyService : IService
    {
        public static object Currency(ServiceModel service, AccountModel account, ActionRequestMessage msg)
        {
            switch (msg.ActionId)
            {
                case (int)ActionEnum.GetCurrenciesValues:
                    return (new ActionResultMessage(Shared.Protocol.Actions.Enums.ActionResultEnum.Success, service.Id, msg.ActionId, CurrencyService.GetSpecificCurrencyValue(msg.Params), msg.Params));
                case (int)ActionEnum.GetSpecificCurrencyValue:
                    return (new ActionResultMessage(Shared.Protocol.Actions.Enums.ActionResultEnum.Success, service.Id, msg.ActionId, CurrencyService.GetCurrenciesValues(), msg.Params));
                default:
                    return new UnknowBehaviourMessage();
            }
        }

        private static string GetCurrencies(string tag)
        {
            var url = "https://api.exchangeratesapi.io/latest?base=" + tag;
            return (Service.MakeRequest(url));
        }

        public static string GetCurrenciesValues()
        {
            return (GetCurrencies(""));
        }

        public static string GetSpecificCurrencyValue(string tag)
        {
            return (GetCurrencies(tag));
        }

    }
}

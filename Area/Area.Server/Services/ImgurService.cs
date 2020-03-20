using Area.Server.Database.Models;
using Area.Shared.Entities;
using Area.Shared.Protocol.Actions;
using Area.Shared.Protocol.Other;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Server.Services
{
    public class ImgurService : IService
    {

        public static object Imgur(ServiceModel service, AccountModel account, ActionRequestMessage msg)
        {
            switch (msg.ActionId)
            {
                case (int)ActionEnum.SearchInGallery:
                    return (new ActionResultMessage(Shared.Protocol.Actions.Enums.ActionResultEnum.Success, service.Id, msg.ActionId, ImgurService.GallerySearch(account.Username, msg.Params), msg.Params));
                case (int)ActionEnum.GetFavoritesImage:
                    return (new ActionResultMessage(Shared.Protocol.Actions.Enums.ActionResultEnum.Success, service.Id, msg.ActionId, ImgurService.FavoritesImage(account.Username), msg.Params));
                case (int)ActionEnum.GetAccountImages:
                    return (new ActionResultMessage(Shared.Protocol.Actions.Enums.ActionResultEnum.Success, service.Id, msg.ActionId, ImgurService.AccountImages(account.Username), msg.Params));
                default:
                    return new UnknowBehaviourMessage();
            }
        }

        public static string GallerySearch(string token, string tag)
        {
            var values = new Dictionary<string, string>
                {
                    { "Authorization", "Bearer " + token }
                };
            string url = "https://api.imgur.com/3/gallery/search/time/all/1?q=" + tag;
            string resp = Service.MakeGetRequest(values, url);
            return (resp);
        }

        public static string FavoritesImage(string token)
        {
            var values = new Dictionary<string, string>
                {
                    { "Authorization", "Bearer " + token }
                };
            string url = "https://api.imgur.com/3/account/me/favorites";
            string resp = Service.MakeGetRequest(values, url);
            return (resp);
        }

        public static string AccountImages(string token)
        {
            var values = new Dictionary<string, string>
                {
                    { "Authorization", "Bearer " + token }
                };
            string url = "https://api.imgur.com/3/account/me/images";
            string resp = Service.MakeGetRequest(values, url);
            return (resp);
        }

    }
}

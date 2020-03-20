using Area.MobileClient.Client;
using Area.MobileClient.View.Pages;
using Area.Shared.Entities;
using Area.Shared.Protocol.Actions;
using Area.Shared.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Area.MobileClient.Handlers.Action
{
    public class ActionHandler : IHandler
    {
        public void HandleActionResultMessage(SimpleClient client, ActionResultMessage msg)
        {
            Logger.Debug("ActionResultMessage");
            if (msg.Result == Shared.Protocol.Actions.Enums.ActionResultEnum.Success)
            {
                if (msg.ActionId == (int)ActionEnum.SearchInGallery || msg.ActionId == (int)ActionEnum.GetAccountImages || msg.ActionId == (int)ActionEnum.GetFavoritesImage)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        App.masterPage.Detail = new NavigationPage(new MasterImgurPageDetail(msg.ServiceId, msg.ActionId, msg.Output));
                    });
                }
                else
                {
                    JObject json = null;
                    try
                    {
                        if (msg.ServiceId == (int)ServiceEnum.Steam || msg.ActionId == (int)ActionEnum.GetAllPastes)
                            msg.Output = "{ \"data\":" + msg.Output + " }";
                        json = JObject.Parse(msg.Output);
                    } catch { }
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        App.masterPage.Detail = new NavigationPage(new MasterResultPageDetail(msg.ServiceId, msg.ActionId, msg.Param, json));
                    });
                }
            }
        }
    }
}

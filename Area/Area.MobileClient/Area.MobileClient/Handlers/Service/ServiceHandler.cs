using Area.MobileClient.Client;
using Area.Shared.Protocol.Services;
using Area.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Area.MobileClient.Handlers.Service
{
    public class ServiceHandler : IHandler
    {

        public void HandleServiceListMessage(SimpleClient client, ServiceListMessage msg)
        {
            Logger.Debug("ServiceListMessage");
            Device.BeginInvokeOnMainThread(() =>
            {
                App.engine.Data.Update(msg);
            });
        }

    }
}

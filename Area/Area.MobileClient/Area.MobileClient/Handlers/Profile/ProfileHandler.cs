using Area.MobileClient.Client;
using Area.Shared.Protocol.Profile;
using Area.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Area.MobileClient.Handlers.Profile
{
    public class ProfileHandler : IHandler
    {

        public void HandleProfileUpdateResultMessage(SimpleClient client, ProfileUpdateResultMessage msg)
        {
            Logger.Debug("ProfileUpdateResultMessage");

            Device.BeginInvokeOnMainThread(() =>
            {
                App.engine.Data.Update(msg.Result);
            });
        }

    }
}

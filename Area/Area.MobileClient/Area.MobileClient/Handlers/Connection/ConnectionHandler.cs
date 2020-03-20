using Area.MobileClient.Client;
using Area.Shared.Protocol.Connection;
using Area.Shared.Protocol.Handshake;
using Area.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Area.MobileClient.Handlers.Connection
{
    public class ConnectionHandler : IHandler
    {

        public void HandleHelloConnectMessage(SimpleClient client, HelloConnectMessage msg)
        {
            Logger.Debug("HelloConnectMessage");
            client.Send(new HelloConnectMessage());
        }

        public void HandleIdentificationResultMessage(SimpleClient client, IdentificationResultMessage msg)
        {
            Logger.Debug("IdentificationResultMessage");
            Device.BeginInvokeOnMainThread(() =>
            {
                if (msg.Result != Shared.Protocol.Connection.Enums.IdentificationResultEnum.Success)
                {
                    App.mainPage.UpdateState(msg.Result.ToString());
                }
                else
                {
                    App.engine.Data.Update(msg);
                    App.engine.State = EngineState.Connected;
                }
            });
        }

        public void HandleRegisterResultMessage(SimpleClient client, RegisterResultMessage msg)
        {
            Logger.Debug("RegisterResultMessage");
            Device.BeginInvokeOnMainThread(() =>
            {
                App.registerPage.UpdateState(msg.Result.ToString());
            });
        }

    }
}

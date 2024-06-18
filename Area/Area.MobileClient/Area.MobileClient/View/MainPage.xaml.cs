using Area.MobileClient;
using Area.MobileClient.Client;
using Area.Shared.Protocol.Connection;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Area
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {

        #region "Variables"

        private Engine engine;

        #endregion

        #region "Builder"

        public MainPage()
        {
            engine = App.engine;
            InitializeComponent();
            engine.Network = new SimpleClient();
        }

        #endregion

        #region "Methods"

        public void UpdateState(string text)
        {
            State.Text = text;
        }

        #endregion

        #region "Events"

        private void Button_Register_Clicked(object obj, EventArgs args)
        {
            engine.State = EngineState.Registration;
        }

        private void Button_Connection_Clicked(object obj, EventArgs args)
        {
            engine.Network.Send(new IdentificationMessage(Username.Text, Password.Text));
        }

        private void Button_Settings_Clicked(object obj, EventArgs args)
        {
            engine.State = EngineState.Config;
        }

        #endregion

    }
}

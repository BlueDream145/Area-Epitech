using Area.Shared.Protocol.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Area.MobileClient.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {

        #region "Variables"

        private Engine engine;

        #endregion

        #region "Builder"

        public RegisterPage()
        {
            engine = App.engine;
            InitializeComponent();
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
            engine.Network.Send(new RegisterMessage(Username.Text, Name.Text, Mail.Text, Password.Text));
        }

        private void Button_Connection_Clicked(object obj, EventArgs args)
        {
            engine.State = EngineState.Disconnected;
        }

        #endregion

    }
}
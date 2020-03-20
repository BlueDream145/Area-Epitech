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
    public partial class ConfigPage : ContentPage
    {

        private Engine engine;

        public ConfigPage()
        {
            engine = App.engine;
            InitializeComponent();
            displayDefaultValues();
        }

        private void displayDefaultValues()
        {
            ServerAddress.Text = Constants.Server_Address;
            ServerPort.Text = Constants.Server_Port.ToString();
        }

        private void Button_Update_Clicked(object obj, EventArgs args)
        {
            if (ServerPort.Text.CompareTo(Constants.Server_Port.ToString()) == 0
                && ServerAddress.Text.CompareTo(Constants.Server_Address.ToString()) == 0)
                return;
            Constants.Server_Address = ServerAddress.Text;
            try
            {
                Constants.Server_Port = Convert.ToInt32(ServerPort.Text);
            } catch { }
        }

        private void Button_Back_Clicked(object obj, EventArgs args)
        {
            engine.State = EngineState.Disconnected;
        }
    }
}
using Area.MobileClient;
using Area.Shared.Protocol.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Area.MobileClient.Client.ClientData;

namespace Area.View.Master
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterHomePageDetail : ContentPage
    {

        #region "Variables"

        private Engine engine;

        private Picker picker;

        Dictionary<string, Color> nameToColor = new Dictionary<string, Color> { };

        private WebView loginViewer;

        #endregion

        #region "Builder"

        public MasterHomePageDetail()
        {
            InitializeComponent();
            init(App.engine);
        }

        #endregion

        #region "Methods"

        private void updateServices()
        {
            if (engine.Data.Services == null)
                return;
            nameToColor.Clear();
            foreach (ServiceMessage service in engine.Data.Services.Services)
            {
                if (service.Registered)
                    continue;
                nameToColor.Add(service.Name, Color.Aqua);
            }
            picker.Items.Clear();
            foreach (string serviceName in nameToColor.Keys)
            {
                picker.Items.Add(serviceName);
            }
        }

        public void init(Engine _engine)
        {
            try
            {
                engine = _engine;
                engine.Data.OnDataChanged += OnDataChanged;
                loginViewer = (WebView)FindByName("LoginViewer");
                LoginViewer.Navigated += LoginViewer_Navigated;

                picker = new Picker
                {
                    Title = "Choose a service",
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };

                updateServices();

                picker.SelectedIndexChanged += (sender, args) =>
                {
                    if (picker.SelectedIndex == -1)
                    {
                        UsernameHeader.IsVisible = false;
                        Username.IsVisible = false;
                        PasswordHeader.IsVisible = false;
                        Password.IsVisible = false;
                        ValidateButton.IsVisible = false;
                        loginViewer.IsVisible = false;
                    }
                    else
                    {
                        string serviceName = picker.Items[picker.SelectedIndex];
                        
                        if (serviceName == "Steam" || serviceName == "Gmail" || serviceName == "Dailymotion" || serviceName == "Pastebin")
                        {
                            UsernameHeader.IsVisible = true;
                            Username.IsVisible = true;
                            PasswordHeader.IsVisible = true;
                            Password.IsVisible = true;
                        }
                        ValidateButton.IsVisible = true;
                        if (serviceName == "Imgur")
                        {
                            loginViewer.IsVisible = true;
                            ValidateButton.IsVisible = false;
                            loadNaviguation();
                        }
                    }
                };

                // Accomodate iPhone status bar.
                this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

                // Build the page.
                this.Content = new StackLayout
                {
                    Children =
                    {
                        LabelHeader,
                        picker,
                        UsernameHeader,
                        Username,
                        PasswordHeader,
                        Password,
                        loginViewer,
                        ValidateButton

                    }
                };
            }
            catch { }
        }

        public void loadNaviguation()
        {
            //string val = engine.Network.Send("https://api.imgur.com/oauth2/authorize");
            //if (val == null || !val.Contains("<!DOCTYPE html PUBLIC "))
            //    return;
            var values = new Dictionary<string, string>
                {
                    { "client_id", "91cb5e70c2f40df" },
                    { "response_type", "token" }
                };
            loginViewer.Source = engine.Network.ConcatUri(values, "https://api.imgur.com/oauth2/authorize");
        }

        private void LoginViewer_Navigated(object sender, WebNavigatedEventArgs e)
        {
            UrlWebViewSource src = (UrlWebViewSource)loginViewer.Source;

            ServiceMessage service = engine.Data.Services.Services.FirstOrDefault(f => f.Name == picker.Items[picker.SelectedIndex]);
            if (service == null || service == default(ServiceMessage))
                return;

            if (src.Url.StartsWith("https://app.getpostman.com/oauth2/"))
            {
                //engine.Data.HandleData(DataEnum.Authentification, src.Url);
                //engine.State = EngineState.Connected;
                Console.WriteLine("Connected !");
                Uri myUri = new Uri(src.Url);
                string query = myUri.Fragment.Remove(0, 1);
                string AccessToken = HttpUtility.ParseQueryString(query).Get("access_token");
                engine.Network.Send(new RegisterServiceMessage(service.Id, AccessToken, "Token", engine.Data.Account.Token));
                picker.SelectedIndex = -1;
            }
            else if (src.Url.StartsWith("https://api.imgur.com/oauth2/authorize"))
                return;
            else
                loadNaviguation();
        }

        #endregion

        #region "Events"

        private void OnDataChanged(object sender, DataChangedEventArgs e)
        {
            switch (e.Data)
            {
                case DataChangedEnum.Identification:
                    if (engine.Data.Account == null)
                        return;
                    LabelHeader.Text = string.Format("Username: {0} | Welcome {1}, you are connected !", engine.Data.Account.Username, engine.Data.Account.Name);
                    updateServices();
                    break;
                case DataChangedEnum.Services:
                    if (engine.Data.Services == null)
                        return;
                    updateServices();
                    break;
            }
        }
        private void Button_Validate_Clicked(object obj, EventArgs args)
        {
            if (engine.Data.Services == null || engine.Network == null)
                return;
            string serviceName = picker.Items[picker.SelectedIndex];
            if ((serviceName == "Steam" || serviceName == "Gmail" || serviceName == "Dailymotion" ||serviceName == "Pastebin") && (Username.Text.Length < 3 || Password.Text.Length < 3))
                return;
            ServiceMessage service = engine.Data.Services.Services.FirstOrDefault(f => f.Name == picker.Items[picker.SelectedIndex]);
            if (service == null || service == default(ServiceMessage))
                return;
            engine.Network.Send(new RegisterServiceMessage(service.Id, Username.Text, Password.Text, engine.Data.Account.Token));
            Username.Text = "";
            Password.Text = "";
            picker.SelectedIndex = -1;
        }

        #endregion
    }
}
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Area.MobileClient.Client;
using Area.MobileClient.Managers;
using Area.Shared.Managers;
using static Area.MobileClient.Engine;
using Area.View.Master;
using Area.MobileClient.View;
using static Area.MobileClient.Client.ClientData;

namespace Area.MobileClient
{
    public partial class App : Application
    {
        public static Engine engine;

        public static MasterHomePage masterPage;

        public static MainPage mainPage;

        public static RegisterPage registerPage;

        public static ConfigPage configPage;

        public App()
        {
            try
            {
                InitializeComponent();
                engine = new Engine();
                engine.OnStateChanged += OnStateChanged;
                engine.Data.OnDataChanged += OnDataChanged;
                mainPage = new MainPage();
                masterPage = new MasterHomePage();
                registerPage = new RegisterPage();
                configPage = new ConfigPage();
            }
            catch { }
        }

        protected override void OnStart()
        {
            ProtocolManager.Initialize();
            HandlersManager.Initialize();
            MainPage = mainPage;
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {

        }

        private void OnDataChanged(object sender, DataChangedEventArgs e)
        {
            switch(e.Data)
            {
                case DataChangedEnum.Identification:
                    break;
                case DataChangedEnum.Services:
                    break;
            }
        }

        private void OnStateChanged(object sender, StateChangedEventArgs e)
        {
            switch (engine.State)
            {
                case EngineState.Disconnected:
                    MainPage = mainPage;
                    break;
                case EngineState.Registration:
                    MainPage = registerPage;
                    break;
                case EngineState.Connected:
                    MainPage = masterPage;
                    break;
                case EngineState.Config:
                    MainPage = configPage;
                    break;
                default:
                    break;
            }
        }
    }
}

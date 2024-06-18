using Area.Shared.Protocol.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Area.MobileClient.Client.ClientData;

namespace Area.MobileClient.View.Master
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterServicesPageDetail : ContentPage
    {
        #region "Variables"

        private Engine engine;

        ObservableCollection<ServiceMessage> services = new ObservableCollection<ServiceMessage>();
        public ObservableCollection<ServiceMessage> Services { get { return services; } }

        #endregion

        #region "Builder"

        public MasterServicesPageDetail()
        {
            InitializeComponent();
            init(App.engine);
        }

        #endregion

        #region "Methods"

        public void init(Engine _engine)
        {
            try
            {
                engine = _engine;
                engine.Data.OnDataChanged += OnDataChanged;
                ServicesView.ItemSelected += ServicesView_ItemSelected;
                initComponents();
            }
            catch { }
        }

        private void initComponents()
        {
            OnDataChanged(null, new DataChangedEventArgs(DataChangedEnum.Services));
            ServicesView.ItemsSource = services;
            updateServices();
        }

        private void updateServices()
        {
            if (engine.Data.Services == null)
                return;
            services.Clear();
            foreach (ServiceMessage msg in engine.Data.Services.Services)
                if (msg.Registered)
                    services.Add(msg);
        }

        #endregion

        #region "Events"

        private void ServicesView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (!(e.SelectedItem is ServiceMessage))
                return;
            ServiceMessage serv = (ServiceMessage)e.SelectedItem;
            App.masterPage.Detail = new NavigationPage(new MasterServicePageDetail(serv.Id));
        }

        private void OnDataChanged(object sender, DataChangedEventArgs e)
        {
            switch (e.Data)
            {
                case DataChangedEnum.Identification:
                    if (engine.Data.Account == null)
                        return;
                    break;
                case DataChangedEnum.Services:
                    if (engine.Data.Services == null)
                        return;
                    break;
            }
            updateServices();
        }

        #endregion
    }
}
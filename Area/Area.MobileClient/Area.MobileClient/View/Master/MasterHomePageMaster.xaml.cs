using Area.MobileClient;
using Area.MobileClient.View.Master;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Area.View.Master
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterHomePageMaster : ContentPage
    {

        #region "Variables"

        private Engine engine;

        public ListView ListView;

        #endregion

        #region "Builder"

        public MasterHomePageMaster()
        {
            InitializeComponent();

            BindingContext = new MasterHomePageMasterViewModel();
            ListView = MenuItemsListView;
            init(App.engine);
        }

        #endregion

        #region "Methods"

        public void init(Engine _engine)
        {
            try
            {
                engine = _engine;
                //engine.Data.OnDataChanged += onDataChanged;
                initComponents();
                loadNaviguation();
            } catch { }
        }

        private void initComponents()
        {
        }

        private void loadNaviguation()
        {

        }

        #endregion

        #region "Events"

        //private void onDataChanged(object sender, DataChangedEventArgs args)
        //{
        //}

        #endregion

        class MasterHomePageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MasterHomePageMasterMenuItem> MenuItems { get; set; }

            public MasterHomePageMasterViewModel()
            {
                MenuItems = new ObservableCollection<MasterHomePageMasterMenuItem>(new[]
                {
                    new MasterHomePageMasterMenuItem { Id = 1, Title = "Home", TargetType = typeof(MasterHomePageDetail) },
                    new MasterHomePageMasterMenuItem { Id = 2, Title = "Profile", TargetType = typeof(MasterProfilePageDetail) },
                    new MasterHomePageMasterMenuItem { Id = 3, Title = "Services", TargetType = typeof(MasterServicesPageDetail) },
                    new MasterHomePageMasterMenuItem { Id = 4, Title = "Triggers", TargetType = typeof(MasterTriggersPageDetail) },
                    new MasterHomePageMasterMenuItem { Id = 5, Title = "Disconnect", TargetType = typeof(MainPage) },
                }); ;
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}
using Area.MobileClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Area.View.Master
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterHomePage : MasterDetailPage
    {

        #region "Variables"

        private Engine engine;

        #endregion

        #region "Builder"

        public MasterHomePage()
        {
            engine = App.engine;
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        #endregion

        #region "Methods"

        #endregion

        #region "Events"

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var item = e.SelectedItem as MasterHomePageMasterMenuItem;
                if (item == null)
                    return;

                if (item.TargetType == typeof(MainPage))
                {
                    engine.State = EngineState.Disconnected;
                    return;
                }
                if (((NavigationPage)Detail).RootPage.GetType() == item.TargetType)
                    return;
                var page = (Page)Activator.CreateInstance(item.TargetType);
                page.Title = item.Title;

                Detail = new NavigationPage(page);
                IsPresented = false;

                MasterPage.ListView.SelectedItem = null;
            }
            catch { }
        }

        #endregion
    }
}
using Area.Shared.Protocol.Reactions;
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
    public partial class MasterTriggersPageDetail : ContentPage
    {
        #region "Variables"

        private Engine engine;

        private Picker picker;

        private Picker pickerReactions;

        Dictionary<string, Color> nameToColor = new Dictionary<string, Color> { { "News", Color.Blue }, { "Currency", Color.Blue }, { "Steam", Color.Blue }, { "Weather", Color.Blue }, { "Domain", Color.Blue } };

        #endregion

        #region "Builder"

        public MasterTriggersPageDetail()
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

                picker = new Picker
                {
                    Title = "Choose a trigger",
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };

                pickerReactions = new Picker
                {
                    Title = "Choose a reaction",
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };

                // Accomodate iPhone status bar.
                this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

                foreach (string serviceName in nameToColor.Keys)
                {
                    picker.Items.Add(serviceName);
                }

                pickerReactions.Items.Add("Pastebin");
                pickerReactions.Items.Add("Gmail");

                // Build the page.
                this.Content = new StackLayout
                {
                    Children =
                    {
                        LabelHeader,
                        LabelName,
                        picker,
                        ValueHeader,
                        ValueEntry,
                        pickerReactions,
                        Password,
                        RunButton,
                        BackButton

                    }
                };
            }
            catch { }
        }

        private void initComponents()
        {
            OnDataChanged(null, new DataChangedEventArgs(DataChangedEnum.Services));
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
        }
        private void Button_Run_Clicked(object obj, EventArgs args)
        {
            if (engine.Network == null || picker.SelectedIndex == -1 || pickerReactions.SelectedIndex == -1 || ValueEntry.Text.Length == 0)
                return;
            engine.Network.Send(new ReactionRequestMessage(picker.SelectedIndex, pickerReactions.SelectedIndex.ToString() + "|" + ValueEntry.Text, engine.Data.Account.Token));
        }
        private void Button_Back_Clicked(object obj, EventArgs args)
        {
            App.masterPage.Detail = App.masterPage;
        }

        #endregion

    }
}
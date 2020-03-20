using Area.MobileClient.View.Pages;
using Area.Shared.Protocol.Entities;
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
    public partial class MasterServicePageDetail : ContentPage
    {
        #region "Variables"

        private Engine engine;

        private int ServiceId;

        ObservableCollection<ActionMessage> actions = new ObservableCollection<ActionMessage>();

        ObservableCollection<ReactionMessage> reactions = new ObservableCollection<ReactionMessage>();

        public ObservableCollection<ActionMessage> Actions { get { return actions; } }
        public ObservableCollection<ReactionMessage> Reactions { get { return reactions; } }

        #endregion

        #region "Builder"

        public MasterServicePageDetail(int serviceId)
        {
            ServiceId = serviceId;
            InitializeComponent();
            init(App.engine);
        }

        #endregion

        #region "Methods"

        public void init(Engine _engine)
        {
            engine = _engine;
            engine.Data.OnDataChanged += OnDataChanged;
            ActionsView.ItemSelected += ActionsView_ItemSelected;
            ActionsView.ItemsSource = actions;
            ReactionsView.ItemSelected += ReactionsView_ItemSelected;
            ReactionsView.ItemsSource = reactions;
            updateService();
        }

        private void updateService()
        {
            ServiceMessage service = engine.Data.Services.Services.FirstOrDefault(f => f.Id == ServiceId);

            if (service != null && service != default(ServiceMessage))
            {
                ServiceName.Text = "Service name: " + service.Name + " (ID=" + service.Id + ")";
                ServiceDescription.Text = "Service description: " + service.Description;
                if (service.Registered)
                {
                    UnregisterButton.IsVisible = true;
                    ActionsView.IsVisible = true;
                    ReactionsView.IsVisible = true;
                }
                else
                {
                    UnregisterButton.IsVisible = false;
                    ActionsView.IsVisible = false;
                    ReactionsView.IsVisible = false;
                }
                actions.Clear();
                foreach (ActionMessage msg in service.Actions)
                    actions.Add(msg);
                reactions.Clear();
                foreach (ReactionMessage msg in service.Reactions)
                    reactions.Add(msg);
            }
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
                    break;
                case DataChangedEnum.Services:
                    if (engine.Data.Services == null)
                        return;
                    updateService();
                    break;
            }
        }

        private void ReactionsView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (!(e.SelectedItem is ReactionMessage))
                return;
            ReactionMessage reaction = (ReactionMessage)e.SelectedItem;
            App.masterPage.Detail = new NavigationPage(new MasterReactionPageDetail(ServiceId, reaction.Id));
        }

        private void ActionsView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (!(e.SelectedItem is ActionMessage))
                return;
            ActionMessage action = (ActionMessage)e.SelectedItem;
            App.masterPage.Detail = new NavigationPage(new MasterActionPageDetail(ServiceId, action.Id));
        }

        private void Button_Unregister_Clicked(object obj, EventArgs args)
        {
            engine.Network.Send(new DeleteServiceMessage(ServiceId, engine.Data.Account.Token));
        }

        #endregion
    }
}
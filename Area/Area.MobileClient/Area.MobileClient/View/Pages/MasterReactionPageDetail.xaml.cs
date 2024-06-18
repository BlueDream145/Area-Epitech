using Area.MobileClient.View.Master;
using Area.Shared.Protocol.Entities;
using Area.Shared.Protocol.Reactions;
using Area.Shared.Protocol.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Area.MobileClient.Client.ClientData;

namespace Area.MobileClient.View.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterReactionPageDetail : ContentPage
    {
        #region "Variables"

        private Engine engine;

        private int ReactionId;

        private int ServiceId;

        #endregion

        #region "Builder"

        public MasterReactionPageDetail(int serviceId, int reactionId)
        {
            ServiceId = serviceId;
            ReactionId = reactionId;
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
                initComponents();
            }
            catch { }
        }

        private void initComponents()
        {
            OnDataChanged(null, new DataChangedEventArgs(DataChangedEnum.Services));
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
                    ServiceMessage service = engine.Data.Services.Services.FirstOrDefault(f => f.Id == ServiceId);
                    if (service == null || service == default(ServiceMessage))
                        return;
                    ReactionMessage reaction = service.Reactions.FirstOrDefault(f => f.Id == ReactionId);
                    if (reaction == null || reaction == default(ReactionMessage))
                        return;

                    LabelName.Text = "Reaction name: " + reaction.Name;
                    LabelDescription.Text = "Reaction description: " + reaction.Description;
                    break;
            }
        }

        //private void Button_Run_Clicked(object obj, EventArgs args)
        //{
        //    if (engine.Network == null)
        //        return;

        //    engine.Network.Send(new ReactionRequestMessage(ReactionId, new List<string>(), engine.Data.Account.Token));
        //}

        private void Button_Back_Clicked(object obj, EventArgs args)
        {
            App.masterPage.Detail = new NavigationPage(new MasterServicePageDetail(ServiceId));
        }

        #endregion
    }
}
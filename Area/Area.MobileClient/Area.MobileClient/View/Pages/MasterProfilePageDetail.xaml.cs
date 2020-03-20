using Area.Shared.Protocol.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Area.MobileClient.Client.ClientData;

namespace Area.MobileClient.View.Master
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterProfilePageDetail : ContentPage
    {
        #region "Variables"

        private Engine engine;

        #endregion

        #region "Builder"

        public MasterProfilePageDetail()
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
                initComponents();
            }
            catch { }
        }

        private void initComponents()
        {
            OnDataChanged(null, new DataChangedEventArgs(DataChangedEnum.Identification));
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
                    Username.Text = engine.Data.Account.Username;
                    Name.Text = engine.Data.Account.Name;
                    Mail.Text = engine.Data.Account.Mail;
                    Password.Text = "";
                    break;
                case DataChangedEnum.Services:
                    if (engine.Data.Services == null)
                        return;
                    break;
                case DataChangedEnum.ProfileUpdateResult:
                    State.Text = engine.Data.ProfileUpdateResult.ToString();
                    break;
            }
        }

        private void Button_Update_Clicked(object obj, EventArgs args)
        {
            engine.Network.Send(new ProfileUpdateRequestMessage(Username.Text, Name.Text, Mail.Text, Password.Text, engine.Data.Account.Token));
        }

        #endregion
    }
}
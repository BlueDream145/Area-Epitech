using Area.MobileClient.View.Master;
using Area.Shared.Entities;
using Area.Shared.Protocol.Actions;
using Area.Shared.Protocol.Entities;
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
    public partial class MasterActionPageDetail : ContentPage
    {
        #region "Variables"

        private Engine engine;

        private int ActionId;

        private int ServiceId;

        #endregion

        #region "Builder"

        public MasterActionPageDetail(int serviceId, int actionId)
        {
            ServiceId = serviceId;
            ActionId = actionId;
            InitializeComponent();
            init(App.engine);
        }

        #endregion

        #region "Methods"

        public void init(Engine _engine)
        {
            //try
            //{
                engine = _engine;
                engine.Data.OnDataChanged += OnDataChanged;
                initComponents();
            //}
            //catch { }
        }

        private void initComponents()
        {
            Entry tag = new Entry();
            Entry subject = new Entry();
            Entry msg = new Entry();

            switch ((ActionEnum)ActionId)
            {
                case ActionEnum.GetSpecificCurrencyValue:
                    tag.Text = "Currency";
                    StackLayoutMap.Children.Add(tag);
                    break;
                case ActionEnum.GetWeatherByLocation:
                    tag.Text = "Country";
                    StackLayoutMap.Children.Add(tag);
                    break;
                case ActionEnum.GetNewsByTag:
                    tag.Text = "Tag";
                    StackLayoutMap.Children.Add(tag);
                    break;
                case ActionEnum.SearchInGallery:
                    tag.Text = "Cat";
                    StackLayoutMap.Children.Add(tag);
                    break;
                case ActionEnum.CheckDomainInfos:
                    tag.Text = "epitech.eu";
                    StackLayoutMap.Children.Add(tag);
                    break;
                case ActionEnum.GetVideosByTag:
                    tag.Text = "Macron";
                    StackLayoutMap.Children.Add(tag);
                    break;
                case ActionEnum.SendMail:
                    tag.Text = "@@epitech.eu";
                    subject.Text = "Hello !";
                    msg.Text = "Just saying hello !";
                    StackLayoutMap.Children.Add(tag);
                    StackLayoutMap.Children.Add(subject);
                    StackLayoutMap.Children.Add(msg);
                    break;
                case ActionEnum.CreatePaste:
                    subject.Text = "Title";
                    msg.Text = "Msg: Just saying hello !";
                    StackLayoutMap.Children.Add(subject);
                    StackLayoutMap.Children.Add(msg);
                    break;
            }
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
                    ActionMessage action = service.Actions.FirstOrDefault(f => f.Id == ActionId);
                    if (action == null || action == default(ActionMessage))
                        return;

                    LabelName.Text = "Action name: " + action.Name;
                    LabelDescription.Text = "Action description: " + action.Description;
                    break;
            }
        }
        private void Button_Run_Clicked(object obj, EventArgs args)
        {
            if (engine.Network == null)
                return;

            string _params = "";
            switch ((ActionEnum)ActionId)
            {
                case ActionEnum.GetWeatherByLocation:
                case ActionEnum.GetNewsByTag:
                case ActionEnum.GetSpecificCurrencyValue:
                case ActionEnum.SearchInGallery:
                case ActionEnum.CheckDomainInfos:
                case ActionEnum.GetVideosByTag:
                    if (StackLayoutMap.Children.Count == 0)
                        return;
                    _params = ((Entry)StackLayoutMap.Children[0]).Text;
                    break;
                case ActionEnum.SendMail:
                    if (StackLayoutMap.Children.Count == 0)
                        return;
                    string receiver = ((Entry)StackLayoutMap.Children[0]).Text;
                    string subject = ((Entry)StackLayoutMap.Children[1]).Text;
                    string msg = ((Entry)StackLayoutMap.Children[2]).Text;
                    _params = receiver + "|" + subject + "|" + msg;
                    break;
                case ActionEnum.CreatePaste:
                    if (StackLayoutMap.Children.Count == 0)
                        return;
                    string _subject = ((Entry)StackLayoutMap.Children[0]).Text;
                    string _msg = ((Entry)StackLayoutMap.Children[1]).Text;
                    _params = _subject + "|" + _msg;
                    break;
            }
            engine.Network.Send(new ActionRequestMessage(ActionId, _params, engine.Data.Account.Token));
        }
        private void Button_Back_Clicked(object obj, EventArgs args)
        {
            App.masterPage.Detail = new NavigationPage(new MasterServicePageDetail(ServiceId));
        }

        #endregion
    }
}
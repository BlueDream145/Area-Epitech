using Area.MobileClient.View.Master;
using Area.Shared.Entities;
using Area.Shared.Protocol.Actions;
using Area.Shared.Protocol.Entities;
using Area.Shared.Protocol.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Area.MobileClient.Client.ClientData;

namespace Area.MobileClient.View.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public class Element
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }

        public Element(string title, string description, string imageurl, string url)
        {
            Title = title;
            Description = description;
            Url = url;
        }
    }

    public partial class MasterResultPageDetail : ContentPage
    {

        #region "Variables"

        private Engine engine;

        private int ActionId;

        private int ServiceId;

        private JObject Data;

        ObservableCollection<Element> elements = new ObservableCollection<Element>();
        public ObservableCollection<Element> Elements { get { return elements; } }

        private string Param;

        #endregion

        #region "Builder"

        public MasterResultPageDetail(int serviceId, int actionId, string param, JObject data)
        {
            ServiceId = serviceId;
            ActionId = actionId;
            Data = data;
            Param = param;
            engine = App.engine;
            InitializeComponent();
            initComponents();
        }

        #endregion

        #region "Methods"

        private void initComponents()
        {
            CollectionScroller.ItemsSource = elements;
            CollectionScroller.SelectionChanged += CollectionScroller_SelectionChanged;

            if (ServiceId != (int)ServiceEnum.Gmail && Data == null)
                return;

            switch ((ActionEnum)ActionId)
            {
                case ActionEnum.GetNewsByTag:
                case ActionEnum.GetNews:
                    if (Data.SelectToken("articles") != null)
                    {
                        JArray articles = (JArray)Data.SelectToken("articles");

                        foreach (JObject obj in articles)
                        {
                            string author = (string)obj.SelectToken("author");
                            string title = (string)obj.SelectToken("title");
                            string description = (string)obj.SelectToken("description");
                            string url = (string)obj.SelectToken("url");
                            string publishedAt = (string)obj.SelectToken("publishedAt");
                            string content = (string)obj.SelectToken("content");
                            Element elem = new Element(title, description, "", url);
                            elements.Add(elem);
                            
                        }
                    }
                    break;
                case ActionEnum.GetWeatherByLocation:
                case ActionEnum.GetLocalWeather:
                    if (Data.SelectToken("current") != null && Data.SelectToken("location") != null && Data.SelectToken("request") != null)
                    {
                        JObject current = (JObject)Data.SelectToken("current");
                        JObject location = (JObject)Data.SelectToken("location");
                        JObject request = (JObject)Data.SelectToken("request");

                        string urlToImage = ((string)current.SelectToken("weather_icons")[0]).Replace("https", "http");
                        string description = (string)current.SelectToken("weather_descriptions")[0];
                        string title = (string)current.SelectToken("title");
                        string temperature = (string)current.SelectToken("temperature");
                        string weatherCode = (string)current.SelectToken("weather_code");
                        string location_name = (string)location.SelectToken("name");
                        string windSpeed = (string)current.SelectToken("wind_speed");
                        string windDegree = (string)current.SelectToken("wind_degree");
                        string windDir = (string)current.SelectToken("wind_dir");
                        string precip = (string)current.SelectToken("precip");
                        string humidity = (string)current.SelectToken("humidity");
                        string cloudcover = (string)current.SelectToken("cloudcover");
                        string visibility = (string)current.SelectToken("visibility");
                        string is_day = (string)current.SelectToken("is_day");
                        string url = "";

                        elements.Add(new Element(title, "The weather is " + description + " in " + location_name + " (Weather Code " + weatherCode + ")", urlToImage, url));
                        elements.Add(new Element(title, "The temperature is " + temperature + " °C", null, url));
                        if (is_day.CompareTo("no") == 0)
                        {
                            elements.Add(new Element(title, "It's the night.", null, url));
                        } else
                        {
                            elements.Add(new Element(title, "It's the day.", null, url));
                        }
                        elements.Add(new Element(title, "The wind speed is " + windSpeed + " km/h, and wind direction is " + windDir + " (" + windDegree + "°)", null, url));
                        elements.Add(new Element(title, "Precipitation " + precip + "%", null, url));
                        elements.Add(new Element(title, "Humidity " + humidity + "%", null, url));
                        elements.Add(new Element(title, "Cloud cover " + cloudcover + "%", null, url));
                    }
                    break;
                case ActionEnum.GetCurrenciesValues:
                case ActionEnum.GetSpecificCurrencyValue:
                    if (Data.SelectToken("rates") != null && Data.SelectToken("base") != null && Data.SelectToken("date") != null)
                    {
                        IEnumerable<JToken> current = (IEnumerable<JToken>)Data.SelectToken("rates");
                        string _base = (string)Data.SelectToken("base");
                        string _date = (string)Data.SelectToken("date");

                        elements.Add(new Element(_date, null, null, null));
                        elements.Add(new Element("Base : " + _base, null, null, null));
                        foreach (JToken item in current)
                        {
                            if ((ActionEnum)ActionId == ActionEnum.GetSpecificCurrencyValue && !item.ToString().ToLower().Contains(Param.ToLower()))
                                continue;
                            elements.Add(new Element(item.ToString(), null, null, null));
                        }
                    }
                    break;
                case ActionEnum.CheckDomainInfos:

                    if (Data.SelectToken("result") != null)
                    {
                        JObject obj = (JObject)Data.SelectToken("result");
                        bool registered = (bool)obj.SelectToken("registered");
                        bool dnssec = (bool)obj.SelectToken("dnssec");
                        IEnumerable<JToken> nameservers = (IEnumerable<JToken>)obj.SelectToken("nameservers");
                        IEnumerable<JToken> status = (IEnumerable<JToken>)obj.SelectToken("status");
                        IEnumerable<JToken> registrar = (IEnumerable<JToken>)obj.SelectToken("registrar");

                        if (registered)
                        {
                            elements.Add(new Element("The domain is registered.", null, null, null));
                            if (dnssec)
                                elements.Add(new Element("The domain is protected (dnssec).", null, null, null));
                            else
                                elements.Add(new Element("The domain is not protected (dnssec).", null, null, null));
                            elements.Add(new Element("Name servers: ", null, null, null));
                            foreach (JToken item in nameservers)
                            {
                                elements.Add(new Element(item.ToString(), null, null, null));
                            }
                            elements.Add(new Element("Status: ", null, null, null));
                            foreach (JToken item in status)
                            {
                                elements.Add(new Element(item.ToString(), null, null, null));
                            }
                            elements.Add(new Element("Registration: ", null, null, null));
                            foreach (JToken item in registrar)
                            {
                                elements.Add(new Element(item.ToString(), null, null, null));
                            }

                        }
                        else
                            elements.Add(new Element("The domain is not registered.", null, null, null));
                    }
                    break;
                case ActionEnum.GetFriends:
                    if (Data.SelectToken("data") != null)
                    {
                        IEnumerable<JToken> c = (IEnumerable<JToken>)Data.SelectToken("data");

                        foreach (JToken item in c)
                        {
                            string username = (string)item.SelectToken("Username");
                            string id = (string)item.SelectToken("SteamId");
                            elements.Add(new Element(username, "Steam Id: " + id, null, null));
                        }
                    }
                    break;
                case ActionEnum.SendMail:
                    elements.Add(new Element("Email sent.", null, null, null));
                    break;
                case ActionEnum.GetVideos:
                case ActionEnum.GetVideosByTag:

                    if (Data.SelectToken("list") != null)
                    {
                        IEnumerable<JToken> list = (IEnumerable<JToken>)Data.SelectToken("list");

                        foreach (JToken item in list)
                        {
                            string title = (string)item.SelectToken("title");
                            string id = (string)item.SelectToken("id");
                            elements.Add(new Element(title, null, null, "https://www.dailymotion.com/video/" + id));
                        }
                    }

                    break;
                case ActionEnum.CreatePaste:

                    if (Data.SelectToken("data") != null)
                    {
                        JObject obj = (JObject)Data.SelectToken("data");
                        string URL = (string)obj.SelectToken("URL");

                        elements.Add(new Element(URL, null, null, URL));
                    }

                    break;
                case ActionEnum.GetAllPastes:

                    if (Data.SelectToken("data") != null)
                    {
                        IEnumerable<JToken> pastes = (IEnumerable<JToken>)Data.SelectToken("data");

                        foreach (JToken item in pastes)
                        {
                            string title = (string)item.SelectToken("Title");
                            string url = (string)item.SelectToken("Url");
                            string date = (string)item.SelectToken("CreateDate");
                            elements.Add(new Element(title, "Created " + date, null, url));
                        }
                    }

                    break;
            }
        }

        private void CollectionScroller_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0)
                return;
            Element elem = (Element)e.CurrentSelection[0];
            if (elem == null || elem.Url == null || elem.Url == "")
                return;
            if (ServiceId == (int)ServiceEnum.News)
            {
                Device.OpenUri(new Uri(elem.Url));
            } else if (ServiceId == (int)ServiceEnum.Currency && elem.Title.Contains("\""))
            {
                string[] parts = elem.Title.Split(new char[] { '\"' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 0)
                    return;
                Device.OpenUri(new Uri("https://www.cnbc.com/quotes/?symbol=" + parts[0] + "="));
            }
            else if (ServiceId == (int)ServiceEnum.Steam)
            {
                string id = elem.Description.Remove(0, 10);

                Device.OpenUri(new Uri("http://steamcommunity.com/id/" + id + "/"));
            }
            else if (ServiceId == (int)ServiceEnum.Dailymotion)
            {
                Device.OpenUri(new Uri(elem.Url));
            }
            else if (ServiceId == (int)ServiceEnum.Pastebin)
            {
                Device.OpenUri(new Uri(elem.Url));
            }
        }

        #endregion

        #region "Events"

        private void Button_Back_Clicked(object obj, EventArgs args)
        {
            App.masterPage.Detail = new NavigationPage(new MasterActionPageDetail(ServiceId, ActionId));
        }

        #endregion
    }
}
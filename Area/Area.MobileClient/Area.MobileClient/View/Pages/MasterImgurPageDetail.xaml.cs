using Area.MobileClient.Services.Imgur;
using Area.MobileClient.View.Master;
using Area.Shared.Entities;
using Area.Shared.Protocol.Actions;
using Area.Shared.Protocol.Entities;
using Area.Shared.Protocol.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using static Area.MobileClient.Client.ClientData;
using System.Xml.XPath;

namespace Area.MobileClient.View.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterImgurPageDetail : ContentPage
    {
        #region "Variables"

        private Engine engine;

        private Image img;

        private int position = 0;

        public bool running = false;

        public Task task;

        private int ActionId;

        private int ServiceId;

        private string Result;

        List<ClientImage> imgs = new List<ClientImage>();

        #endregion

        #region "Builder"

        public MasterImgurPageDetail(int serviceId, int actionId, string result)
        {
            ServiceId = serviceId;
            ActionId = actionId;
            Result = result;
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
                initComponents();

            switch((ActionEnum)ActionId)
            {
                case ActionEnum.GetAccountImages:
                    HandleAccountImagesCallBack(Result);
                    break;
                case ActionEnum.GetFavoritesImage:
                    HandleAccountFavoritesCallBack(Result);
                    break;
                case ActionEnum.SearchInGallery:
                    HandleSearchImagesCallBack(Result);
                    break;
            }
            loadImage();
            //}
            //catch { }
        }
        private static ClientImage searchImageArray(XElement root)
        {
            string link = "";
            bool found = false;
            ClientImage img = new ClientImage();
            try
            {
                IEnumerable<XElement> items = root.XPathSelectElement("//images").Elements();
                foreach (XElement item in items)
                {
                    foreach (XElement attr in items.Elements())
                    {
                        if (attr.Name == "link" && !attr.Value.Contains("."))
                            break;
                        if (attr.Name == "link")
                        {
                            img.Source = attr.Value;
                            found = true;
                        }
                        if (attr.Name == "id")
                            img.Id = attr.Value;
                        if (attr.Name == "title")
                            img.Title = attr.Value;
                        if (attr.Name == "description")
                            img.Description = attr.Value;
                        if (attr.Name == "datetime")
                            img.Time = new DateTime(Convert.ToInt32(attr.Value));
                        if (attr.Name == "favorite")
                        {
                            if (attr.Value == "true")
                                img.Favorite = true;
                            else
                                img.Favorite = false;
                        }
                        if (attr.Name == "type")
                            img.Type = attr.Value;
                    }
                    if (found)
                        break;
                }
            }
            catch
            {
                found = false;
            }
            return (img);
        }

        public void HandleSearchImagesCallBack(string value)
        {
            try
            {
                JsonConvert.DeserializeObject(value);

                value = value.Remove(0, 9);
                string[] entries = value.Split(new string[] { ",\"showsAds\":false}},", ",\"showsAds\":true}},", ",\"is_album\":false},", ",\"is_album\":true}," }, StringSplitOptions.RemoveEmptyEntries);
                if (entries.Length == 0 || !entries[entries.Length - 1].Contains("success"))
                {
                    Console.WriteLine("Can't handle search images.");
                    return;
                }
                for (int i = 1; i < entries.Length - 1; i++)
                {
                    string tmp = entries[i] + "}";
                    XmlDictionaryReader jsonReader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(tmp),
                        new System.Xml.XmlDictionaryReaderQuotas());
                    XElement root = XElement.Load(jsonReader);
                    ClientImage img = searchImageArray(root);
                    if (img == null)
                    {
                        img = new ClientImage(root.XPathSelectElement("//id").Value,
                            root.XPathSelectElement("//title").Value,
                            root.XPathSelectElement("//description").Value,
                            root.XPathSelectElement("//datetime").Value,
                            root.XPathSelectElement("//link").Value,
                            root.XPathSelectElement("//favorite").Value,
                            root.XPathSelectElement("//type").Value);
                    }
                    imgs.Add(img);
                }
            }
            catch { return; }
        }

        public void HandleAccountImagesCallBack(string value)
        {
            try
            {
                string[] entries = value.Split(new char[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries);
                if (entries.Length == 0 || !entries[entries.Length - 1].Contains("success"))
                {
                    Console.WriteLine("Can't handle account images.");
                    return;
                }
                for (int i = 1; i < entries.Length - 1; i += 2)
                {
                    var jsonReader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes("{ " + entries[i] + " }"),
                        new System.Xml.XmlDictionaryReaderQuotas());
                    var root = XElement.Load(jsonReader);
                    ClientImage img = new ClientImage(root.XPathSelectElement("//id").Value,
                        root.XPathSelectElement("//type").Value,
                        root.XPathSelectElement("//datetime").Value,
                        root.XPathSelectElement("//link").Value,
                        root.XPathSelectElement("//width").Value,
                        root.XPathSelectElement("//height").Value,
                        root.XPathSelectElement("//deletehash").Value);
                    imgs.Add(img);
                }
            }
            catch { }
        }
        private ClientImage favoriteImageArray(XElement root)
        {
            string link = "";
            bool found = false;
            ClientImage img = new ClientImage();
            try
            {
                IEnumerable<XElement> items = root.XPathSelectElement("//images").Elements();
                foreach (XElement item in items)
                {
                    foreach (XElement attr in items.Elements())
                    {
                        if ((attr.Name == "link" || attr.Value.StartsWith("https://i.imgur.com/")) && !attr.Value.Contains("."))
                            break;
                        if (attr.Name == "link" || attr.Value.StartsWith("https://i.imgur.com/"))
                        {
                            img.Source = attr.Value;
                            found = true;
                        }
                        if (attr.Name == "id")
                            img.Id = attr.Value;
                        if (attr.Name == "datetime")
                            img.Time = new DateTime(Convert.ToInt32(attr.Value));
                        if (attr.Name == "type")
                            img.Type = attr.Value;
                    }
                    if (found)
                        break;
                }
            }
            catch
            {
                found = false;
            }
            return (img);
        }


        public void HandleAccountFavoritesCallBack(string value)
        {
            try
            {
                if (!value.Contains("success"))
                {
                    Console.WriteLine("Can't handle favorites images.");
                    return;
                }
                value = value.Remove(0, 37);
                value = value.Remove(value.Length - 4, 4);
                string[] entries = value.Split(new string[] { ",\"size\":0}," }, StringSplitOptions.RemoveEmptyEntries);
                if (entries.Length == 0)
                {
                    Console.WriteLine("Can't handle favorites images.");
                    return;
                }
                for (int i = 0; i < entries.Length; i++)
                {
                    XmlDictionaryReader jsonReader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(entries[i] + "}"),
                        new System.Xml.XmlDictionaryReaderQuotas());
                    XElement root = XElement.Load(jsonReader);
                    ClientImage img = favoriteImageArray(root);
                    if ((img == null || img.Source == null || img.Source == string.Empty) && entries[i].Contains("cover"))
                    {
                        string url = "https://i.imgur.com/" + root.XPathSelectElement("//cover").Value + ".png";
                        img = new ClientImage(root.XPathSelectElement("//id").Value,
                        root.XPathSelectElement("//type").Value,
                        root.XPathSelectElement("//datetime").Value,
                        url,
                        root.XPathSelectElement("//width").Value,
                        root.XPathSelectElement("//height").Value, "");
                    }
                    if (img.Source == null)
                        continue;
                    imgs.Add(img);
                }
            }
            catch { }
        }

        private void loadImage()
        {
            try
            {
                if (running)
                    return;
                running = true;
                task = new Task(() => loadImageProcess());
                task.Start();
            }
            catch { }
        }

        private void loadImageProcess()
        {
            try
            {
                if (imgs.Count == 0)
                {
                    running = false;
                    return;
                }
                if (position > imgs.Count - 1)
                    position = 0;
                else if (position < 0)
                    position = imgs.Count - 1;
                if (!imgs[position].Source.Contains("http"))
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        img.Source = ImageSource.FromFile(imgs[position].Source);
                    });
                else
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        img.Source = ImageSource.FromStream(() =>
                        {
                            WebClient client = new WebClient();
                            Stream stream = client.OpenRead(imgs[position].Source);
                            client.Dispose();
                            return (stream);
                        });
                    });
            }
            catch
            {
                running = false;
            }
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    PreviousButton.IsVisible = true;
                    NextButton.IsVisible = true;
                });
            }
            catch { }
            running = false;
        }

        private void initComponents()
        {
            try
            {
                img = (Image)FindByName("ViewerImage");
                loadImage();
            }
            catch { }
        }

        #endregion

        #region "Events"

        private void onPreviousClicked(object obj, EventArgs args)
        {
            if (running)
                return;
            PreviousButton.IsVisible = false;
            NextButton.IsVisible = false;
            position--;
            loadImage();
        }

        private void onNextClicked(object obj, EventArgs args)
        {
            if (running)
                return;
            PreviousButton.IsVisible = false;
            NextButton.IsVisible = false;
            position++;
            loadImage();
        }
        private void Button_Back_Clicked(object obj, EventArgs args)
        {
            App.masterPage.Detail = new NavigationPage(new MasterActionPageDetail(ServiceId, ActionId));
        }

        #endregion
    }
}
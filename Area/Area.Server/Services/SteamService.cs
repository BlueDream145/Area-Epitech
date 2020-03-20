using Area.Server.Database.Models;
using Area.Shared.Entities;
using Area.Shared.Protocol.Actions;
using Area.Shared.Protocol.Other;
using Newtonsoft.Json;
using Steam.Models.SteamCommunity;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Area.Server.Services
{
    public class SteamFriend
    {
        public ulong SteamId;

        public string Username;

        public string Password;
    }

    public class SteamService : IService
    {

        public int Step;

        public string AccessToken;

        public SteamService()
        {
            Step = 0;
            AccessToken = "";
        }

        public static object Steam(ServiceModel service, AccountModel account, ActionRequestMessage msg)
        {
            switch (msg.ActionId)
            {
                case (int)ActionEnum.GetFriends:
                    SteamService serv = new SteamService();
                    Task<IReadOnlyCollection<FriendModel>> models = serv.GetFriends(account.Username);
                    List<SteamFriend> steamFriends = new List<SteamFriend>();
                    foreach (FriendModel friend in models.Result)
                    {
                        try
                        {
                            SteamFriend f = new SteamFriend();
                            f.SteamId = friend.SteamId;
                            f.Username = serv.GetUsername(f.SteamId);
                            steamFriends.Add(f);
                        }
                        catch { continue; }
                    }
                    string responseString = JsonConvert.SerializeObject(steamFriends);
                    return (new ActionResultMessage(Shared.Protocol.Actions.Enums.ActionResultEnum.Success, service.Id, msg.ActionId, responseString, msg.Params));
                default:
                    return new UnknowBehaviourMessage();
            }
        }

        public string Authenticate()
        {
            if (Step == 0)
                return "";
            var values = new Dictionary<string, string>
                {
                    { "Authorization", "Bearer " + AccessToken }
                };
            string url = "http://steamrep.com/authorize?token=" + AccessToken;
            string resp = Service.MakeGetRequest(values, url);
            return (resp);
        }

        public ulong GetSteamId(string username)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://steamrep.com/search?q=" + username);

            request.MaximumAutomaticRedirections = 4;
            request.MaximumResponseHeadersLength = 4;
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream receiveStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

            string res = readStream.ReadToEnd();
            response.Close();
            readStream.Close();

            if (res.Contains("<a href=\"steam://friends/add/"))
            {
                string[] parts = res.Split("<a href=\"steam://friends/add/", StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2)
                {
                    string[] sparts = parts[1].Split("\"", StringSplitOptions.RemoveEmptyEntries);
                    if (sparts.Length >= 1)
                    {
                        decimal d = Decimal.Parse(sparts[0]);
                        ulong u = (ulong)(d * 1m);
                        return (u);
                    }
                }
            }
            return 76561198056981711;
        }

        public async Task<IReadOnlyCollection<FriendModel>> GetFriends(string username)
        {
            var webInterfaceFactory = new SteamWebInterfaceFactory("F7697DB84F1C3B67CA83434F0110190E"); // < dev api key here >);
            var steamInterface = webInterfaceFactory.CreateSteamWebInterface<SteamUser>(new HttpClient());

            ulong steamId = GetSteamId(username);
            var playerSummaryResponse = await steamInterface.GetPlayerSummaryAsync(steamId); //< steamIdHere >);
            var playerSummaryData = playerSummaryResponse.Data;
            var playerSummaryLastModified = playerSummaryResponse.LastModified;

            var friendsListResponse = await steamInterface.GetFriendsListAsync(steamId); //< steamIdHere >);
            IReadOnlyCollection<FriendModel> friendsList = friendsListResponse.Data;

            return (friendsList);
        }

        public string GetUsername(ulong steamId)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://steamrep.com/search?q=" + steamId.ToString());

            request.MaximumAutomaticRedirections = 4;
            request.MaximumResponseHeadersLength = 4;
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream receiveStream = response.GetResponseStream();

            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

            string res = readStream.ReadToEnd();
            response.Close();
            readStream.Close();

            if (res.Contains("<div id=\"steamnameblock\"><span id=\"steamname\" title=\""))
            {
                string[] parts = res.Split("<div id=\"steamnameblock\"><span id=\"steamname\" title=\"", StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2)
                {
                    string[] sparts = parts[1].Split("\"", StringSplitOptions.RemoveEmptyEntries);
                    if (sparts.Length >= 1)
                    {
                        if (sparts[0] == " class=")
                            return "Profile is not public.";
                        return (sparts[0]);
                    }
                }
            }
            return "Unknow";
        }

    }
}

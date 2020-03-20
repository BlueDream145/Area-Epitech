using Area.Server.Database.Models;
using Area.Server.Database.Tables;
using Area.Server.Services;
using Area.Shared.Entities;
using Area.Shared.Utils;
using Newtonsoft.Json;
using Steam.Models.SteamCommunity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Area.Server.Triggers
{
    public class SteamTrigger : ITrigger
    {

        #region Variables

        public override ReactionEnum Reaction { get; set; }

        public override string Params { get; set; }

        private Timer timer;

        private bool running;

        private const int updateDelay = 60000;

        #endregion

        #region Builder

        public SteamTrigger(ReactionEnum _reaction, string _params)
        {
            Reaction = _reaction;
            Params = _params;
            running = false;
        }

        #endregion

        #region Private Methods

        private void Work()
        {
            AccountModel account = AccountTable.GetModelByServiceId((int)ServiceEnum.Steam);
            if (account == null)
                return;
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
            string data = JsonConvert.SerializeObject(steamFriends);
            string title = "Steam Update " + DateTime.Now.ToString();

            TriggerManager.Make(Reaction, title, data);
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            if (running)
                return;
            Logger.Debug("Steam trigger started.");
            timer = new Timer(updateDelay);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            running = true;
            Work();
        }

        public override void Stop()
        {
            if (!running)
                return;
            timer.Stop();
            running = false;
        }

        #endregion

        #region Events

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Work();
        }

        #endregion
    }
}
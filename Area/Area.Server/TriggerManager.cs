using Area.Server.Services;
using Area.Server.Triggers;
using Area.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Server
{
    public static class TriggerManager
    {

        #region Variables

        private static List<ITrigger> Triggers;

        private static object Check;

        #endregion

        #region Builder

        static TriggerManager()
        {
            Triggers = new List<ITrigger>();
            Check = new object();
        }

        #endregion

        #region Methods

        public static void HandleTrigger(TriggerEnum trigger, ReactionEnum reaction, string _params)
        {
            ITrigger inst = null;

            switch (trigger)
            {
                case TriggerEnum.CurrencyUpdate:
                    inst = new CurrencyTrigger(reaction, _params);
                    break;
                case TriggerEnum.DomainUpdate:
                    inst = new DomainTrigger(reaction, _params);
                    break;
                case TriggerEnum.NewsUpdate:
                    inst = new NewsTrigger(reaction, _params);
                    break;
                case TriggerEnum.SteamUpdate:
                    inst = new SteamTrigger(reaction, _params);
                    break;
                case TriggerEnum.WeatherUpdate:
                    inst = new WeatherTrigger(reaction, _params);
                    break;
            }
            lock(Check)
            {
                if (inst != null)
                    Triggers.Add(inst);
            }
            inst.Start();
        }

        private static void StopTriggers()
        {
            lock (Check)
            {
                while (Triggers.Count != 0)
                {
                    ITrigger inst = Triggers[0];
                    inst.Stop();
                    Triggers.Remove(inst);
                }
            }
        }

        public static void Make(ReactionEnum reaction, string title, string data)
        {
            switch (reaction)
            {
                case ReactionEnum.Gmail:
                    GmailServ.Send(title, data);
                    break;
                case ReactionEnum.Pastebin:
                    PastebinService.CreatePaste(title, data);
                    break;
            }
        }

        #endregion

    }
}

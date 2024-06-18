using Area.Server.Services;
using Area.Shared.Entities;
using Area.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace Area.Server.Triggers
{
    public class DomainTrigger : ITrigger
    {

        #region Variables

        public override ReactionEnum Reaction { get; set; }

        public override string Params { get; set; }

        private Timer timer;

        private bool running;

        private const int updateDelay = 60000;

        #endregion

        #region Builder

        public DomainTrigger(ReactionEnum _reaction, string _params)
        {
            Reaction = _reaction;
            Params = _params;
            running = false;
        }

        #endregion

        #region Private Methods

        private void Work()
        {
            string data = WhoIsService.GetWhoIs(Params);
            string title = "Domain Update " + DateTime.Now.ToString();

            TriggerManager.Make(Reaction, title, data);
        }

        #endregion

        #region Public Methods

        public override void Start()
        {
            if (running)
                return;
            Logger.Debug("Domain trigger started.");
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
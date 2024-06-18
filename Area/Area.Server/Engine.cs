using Area.Server.Database;
using Area.Server.Managers;
using Area.Server.Network;
using Area.Server.Services;
using Area.Shared.Managers;
using Area.Shared.Protocol;
using Area.Shared.Protocol.Handshake;
using Area.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Server
{
    public class Engine
    {

        #region "Variables"

        public HttpServer Http { get; set; }

        #endregion

        #region "Builder"

        public Engine()
        {
            Http = new HttpServer();
        }

        #endregion

        #region "Methods"

        public void StartProgram()
        {
            var dbInst = DatabaseManager.Instance();
            Logger.Debug(string.Format("{0} - {1}", Constants.App_Name, Constants.App_Version));
            ProtocolManager.Initialize();
            HandlersManager.Initialize();
            dbInst.RefreshAllDatabase();
            Http.Start();
            Logger.Info("Waiting for user action...");
            //new InstagramService().AsyncConnect();
            //new MailService().Connect();
            //new YoutubeService().Run();
        }

        public void StopProgram()
        {
            var dbInst = DatabaseManager.Instance();
            Http.Abort();
            dbInst.ClearAllDatabase();
        }

        public void Dispose()
        {
            if (Http != null)
            {
                Http.Dispose();
                Http = null;
            }
        }

        #endregion

    }
}

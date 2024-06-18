using Area.Server.Database.Tables;
using Area.Shared.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Server.Database
{
    public class DatabaseManager
    {

        #region "Variables"

        private MySqlConnection connection = null;
        public MySqlConnection Connection
        {
            get { return connection; }
        }

        private static DatabaseManager _instance = null;
        public static DatabaseManager Instance()
        {
            if (_instance == null)
                _instance = new DatabaseManager();
            return (_instance);
        }

        #endregion

        #region "Builder"

        public DatabaseManager()
        { }

        #endregion

        #region "Methods"

        public bool IsConnected()
        {
            try
            {
                if (Connection == null)
                {
                    if (String.IsNullOrEmpty(Constants.Database_Name))
                        return (false);
                    string connstring = string.Empty;
                    if (System.Environment.OSVersion.ToString().ToLower().Contains("windows"))
                    {
                        connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}",
                            Constants.Database_Host, Constants.Database_Name,
                            Constants.Database_Username, Constants.Database_Password);
                    } else
                    {
                        connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}",
                            Constants.Database_Docker, Constants.Database_Name,
                            Constants.Database_Username, Constants.Database_Password);
                    }
                    connection = new MySqlConnection(connstring);
                    connection.Open();
                }
            } catch (MySqlException ex) {
                Logger.Error("Error while trying to connect to MySql database.");
                Logger.Error(ex.Message);
                return (false);
            }

            return (true);
        }

        public void RefreshAllDatabase()
        {
            if (DatabaseManager.Instance().IsConnected())
            {
                ActionTable.LoadTable();
                ReactionTable.LoadTable();
                UserTable.LoadTable();
                ServiceTable.LoadTable();
                AccountTable.LoadTable();
                Logger.Info("Database loaded.");
            } else
                Logger.Error("Connection to database failed.");
        }

        public void ClearAllDatabase()
        {
            UserTable.Clear();
            ServiceTable.Clear();
        }

        public void Close()
        {
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
                connection = null;
            }
        }

        #endregion

    }
}

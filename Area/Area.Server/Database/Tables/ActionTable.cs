using Area.Server.Database.Models;
using Area.Shared.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Server.Database.Tables
{
    public static class ActionTable
    {

        #region "Variables"

        public static List<ActionModel> Cache;

        #endregion

        #region "Builder"

        static ActionTable()
        {
            Cache = new List<ActionModel>();
        }

        #endregion

        #region "Methods"

        public static void Clear()
        {
            if (Cache != null)
            {
                Cache.Clear();
            }
        }

        public static void LoadTable()
        {
            Cache.Clear();
            var dbCon = DatabaseManager.Instance();
            if (dbCon.IsConnected())
            {
                string query = "SELECT * FROM actions";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ActionModel model = new ActionModel(
                        reader.GetInt16(0),
                        reader.GetString(1),
                        reader.GetString(2));
                    Cache.Add(model);
                }
                reader.Dispose();
            }
            else
                Logger.Error("Can't load database 'Actions'.");
        }

        public static void UpdateModel(ActionModel model)
        {
            var dbCon = DatabaseManager.Instance();
            if (dbCon.IsConnected())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = dbCon.Connection;
                cmd.CommandText = string.Format("UPDATE `area`.`actions` SET `Name` = '{0}', `Description` = '{1}' WHERE `ID` = " + model.Id + ";",
                    model.Name, model.Description);
                int numRowsUpdated = cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            else
                Logger.Error("Can't update database 'Actions'.");
        }

        public static void InsertModel(ActionModel model)
        {
            var dbCon = DatabaseManager.Instance();
            if (dbCon.IsConnected())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = dbCon.Connection;
                cmd.CommandText = string.Format("INSERT INTO `area`.`actions` (`ID`, `Name`, `Description`) VALUES ('{0}', '{1}', '{2}');",
                    model.Id, model.Name, model.Description);
                int numRowsUpdated = cmd.ExecuteNonQuery();
                cmd.Dispose();
                Cache.Add(model);
            }
            else
                Logger.Error("Can't update database 'Actions'.");
        }

        public static List<ActionModel> Parse(string data)
        {
            List<ActionModel> list = new List<ActionModel>();
            if (data == null)
                return (list);
            string[] parts = data.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0)
                return (list);
            foreach (string part in parts)
            {
                int id = -1;
                try
                {
                    id = Convert.ToInt32(part);
                }
                catch { continue; }
                var tmp = GetModelById(id);
                if (tmp != null)
                    list.Add(tmp);
            }
            return (list);
        }

        public static ActionModel GetModelById(int id)
        {
            ActionModel model = null;

            model = Cache.Find(f => f.Id == id);
            if (model == null || model == default(ActionModel))
                return (null);
            return (model);
        }

        #endregion

    }
}

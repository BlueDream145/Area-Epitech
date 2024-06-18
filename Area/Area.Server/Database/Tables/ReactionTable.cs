using Area.Server.Database.Models;
using Area.Shared.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Server.Database.Tables
{
    public static class ReactionTable
    {

        #region "Variables"

        public static List<ReactionModel> Cache;

        #endregion

        #region "Builder"

        static ReactionTable()
        {
            Cache = new List<ReactionModel>();
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
                string query = "SELECT * FROM reactions";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ReactionModel model = new ReactionModel(
                        reader.GetInt16(0),
                        reader.GetString(1),
                        reader.GetString(2));
                    Cache.Add(model);
                }
                reader.Dispose();
            }
            else
                Logger.Error("Can't load database 'Reactions'.");
        }

        public static void UpdateModel(ReactionModel model)
        {
            var dbCon = DatabaseManager.Instance();
            if (dbCon.IsConnected())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = dbCon.Connection;
                cmd.CommandText = string.Format("UPDATE `area`.`reactions` SET `Name` = '{0}', `Description` = '{1}' WHERE `ID` = " + model.Id + ";",
                    model.Name, model.Description);
                int numRowsUpdated = cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            else
                Logger.Error("Can't update database 'Reactions'.");
        }

        public static void InsertModel(ReactionModel model)
        {
            var dbCon = DatabaseManager.Instance();
            if (dbCon.IsConnected())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = dbCon.Connection;
                cmd.CommandText = string.Format("INSERT INTO `area`.`reactions` (`ID`, `Name`, `Description`) VALUES ('{0}', '{1}', '{2}');",
                    model.Id, model.Name, model.Description);
                int numRowsUpdated = cmd.ExecuteNonQuery();
                cmd.Dispose();
                Cache.Add(model);
            }
            else
                Logger.Error("Can't update database 'Reactions'.");
        }

        public static List<ReactionModel> Parse(string data)
        {
            List<ReactionModel> list = new List<ReactionModel>();
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

        public static ReactionModel GetModelById(int id)
        {
            ReactionModel model = null;

            model = Cache.Find(f => f.Id == id);
            if (model == null || model == default(ReactionModel))
                return (null);
            return (model);
        }

        #endregion

    }
}

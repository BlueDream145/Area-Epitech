using Area.Server.Database.Models;
using Area.Shared.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Server.Database.Tables
{
    public static class UserTable
    {

        #region "Variables"

        public static List<UserModel> Cache;

        #endregion

        #region "Builder"

        static UserTable()
        {
            Cache = new List<UserModel>();
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
                string query = "SELECT * FROM users";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    UserModel model = new UserModel(
                        reader.GetInt16(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetString(4),
                        reader.GetString(5));
                    Cache.Add(model);
                }
                reader.Dispose();
            }
            else
                Logger.Error("Can't load database 'Users'.");
        }

        public static List<UserModel> Parse(string data)
        {
            List<UserModel> list = new List<UserModel>();
            if (data == null)
                return (list);
            string[] parts = data.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0)
                return (list);
            foreach(string part in parts)
            {
                int id = -1;
                try
                {
                    id = Convert.ToInt32(part);
                } catch { continue; }
                var tmp = GetModelById(id);
                if (tmp != null)
                    list.Add(tmp);
            }
            return (list);
        }

        public static UserModel GetModelById(int id)
        {
            UserModel model = null;

            model = Cache.Find(f => f.Id == id);
            if (model == null || model == default(UserModel))
                return (null);
            return (model);
        }

        public static UserModel GetModelByToken(string _token)
        {
            UserModel model = null;

            model = Cache.Find(f => f.Token == _token);
            if (model == null || model == default(UserModel))
                return (null);
            return (model);
        }

        public static void UpdateModel(UserModel model)
        {
            var dbCon = DatabaseManager.Instance();
            if (dbCon.IsConnected())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = dbCon.Connection;
                cmd.CommandText = string.Format("UPDATE `area`.`users` SET `Username` = '{0}', `Name` = '{1}', `Mail` = '{2}', `Password` = '{3}', `Token` = '{4}' WHERE `ID` = " + model.Id + ";",
                    model.Username, model.Name, model.Mail, model.Password, model.Token);
                int numRowsUpdated = cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            else
                Logger.Error("Can't update database 'Users'.");
        }

        public static void InsertModel(UserModel model)
        {
            var dbCon = DatabaseManager.Instance();
            if (dbCon.IsConnected())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = dbCon.Connection;
                cmd.CommandText = string.Format("INSERT INTO `area`.`users` (`ID`, `Username`, `Name`, `Mail`, `Password`, `Token`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
                    model.Id, model.Username, model.Name, model.Mail, model.Password, model.Token);
                int numRowsUpdated = cmd.ExecuteNonQuery();
                cmd.Dispose();
                Cache.Add(model);
            }
            else
                Logger.Error("Can't update database 'Users'.");
        }

        #endregion

    }
}

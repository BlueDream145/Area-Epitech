using Area.Server.Database.Models;
using Area.Shared.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Server.Database.Tables
{
    public class AccountTable
    {


        #region "Variables"

        public static List<AccountModel> Cache;

        #endregion

        #region "Builder"

        static AccountTable()
        {
            Cache = new List<AccountModel>();
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
                string query = "SELECT * FROM accounts";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    AccountModel model = new AccountModel(
                        reader.GetInt16(0),
                        reader.GetInt16(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetInt16(4));
                    Cache.Add(model);
                }
                reader.Dispose();
            }
            else
                Logger.Error("Can't load database 'Accounts'.");
        }

        public static void RemoveModel(AccountModel model)
        {
            var dbCon = DatabaseManager.Instance();
            if (dbCon.IsConnected())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = dbCon.Connection;
                cmd.CommandText = string.Format("DELETE FROM `area`.`accounts` WHERE `ID` = " + model.Id + ";");
                int numRowsUpdated = cmd.ExecuteNonQuery();
                cmd.Dispose();
                if (Cache.Contains(model))
                    Cache.Remove(model);
            }
            else
                Logger.Error("Can't remove row in database 'Accounts'.");
        }

        public static void UpdateModel(AccountModel model)
        {
            var dbCon = DatabaseManager.Instance();
            if (dbCon.IsConnected())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = dbCon.Connection;
                cmd.CommandText = string.Format("UPDATE `area`.`accounts` SET `Service` = '{0}', `Username` = '{1}', `Password` = '{2}', `OwnerId` = '{3}' WHERE `ID` = " + model.Id + ";",
                    model.Service, model.Username, model.Password, model.OwnerId);
                int numRowsUpdated = cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            else
                Logger.Error("Can't update database 'Accounts'.");
        }

        public static void InsertModel(AccountModel model)
        {
            var dbCon = DatabaseManager.Instance();
            if (dbCon.IsConnected())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = dbCon.Connection;
                cmd.CommandText = string.Format("INSERT INTO `area`.`accounts` (`Service`, `Username`, `Password`, `OwnerId`) VALUES ('{0}', '{1}', '{2}', '{3}');",
                    model.Service, model.Username, model.Password, model.OwnerId);
                int numRowsUpdated = cmd.ExecuteNonQuery();
                cmd.Dispose();
                Cache.Add(model);
            }
            else
                Logger.Error("Can't update database 'Accounts'.");
        }

        public static List<AccountModel> Parse(string data)
        {
            List<AccountModel> list = new List<AccountModel>();
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

        public static AccountModel GetModelById(int id)
        {
            AccountModel model = null;

            model = Cache.Find(f => f.Id == id);
            if (model == null || model == default(AccountModel))
                return (null);
            return (model);
        }

        public static AccountModel GetModelByServiceId(int id)
        {
            AccountModel model = null;

            model = Cache.Find(f => f.Service == id);
            if (model == null || model == default(AccountModel))
                return (null);
            return (model);
        }

        #endregion

    }
}

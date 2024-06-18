using Area.Server.Database.Models;
using Area.Shared.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Server.Database.Tables
{
    public static class ServiceTable
    {

        #region "Variables"

        public static List<ServiceModel> Cache;

        #endregion

        #region "Builder"

        static ServiceTable()
        {
            Cache = new List<ServiceModel>();
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
                string query = "SELECT * FROM services";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string actions = "";
                    string reactions = "";
                    string registered = "";

                    try
                    {
                        if (reader.GetString(3) != null)
                            registered = reader.GetString(3);
                    }
                    catch { }
                    try
                    {
                        if (reader.GetString(4) != null)
                            actions = reader.GetString(4);
                    } catch { }
                    try
                    {
                        if (reader.GetString(5) != null)
                        reactions = reader.GetString(5);
                    } catch { }

                    ServiceModel model = new ServiceModel(
                        reader.GetInt16(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        registered,
                        actions,
                        reactions);
                    Cache.Add(model);
                }
                reader.Dispose();
            }
            else
                Logger.Error("Can't load database 'Services'.");
        }

        public static void UpdateModel(ServiceModel model)
        {
            var dbCon = DatabaseManager.Instance();
            if (dbCon.IsConnected())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = dbCon.Connection;
                cmd.CommandText = string.Format("UPDATE `area`.`services` SET `Name` = '{0}', `Description` = '{1}', `RegisteredUsers` = '{2}', `Actions` = '{3}', `Reactions` = '{4}' WHERE `ID` = " + model.Id + ";",
                    model.Name, model.Description, UserModel.Parse(model.RegisteredUsers), ActionModel.Parse(model.Actions), ReactionModel.Parse(model.Reactions));
                int numRowsUpdated = cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            else
                Logger.Error("Can't update database 'Services'.");
        }

        public static void InsertModel(ServiceModel model)
        {
            var dbCon = DatabaseManager.Instance();
            if (dbCon.IsConnected())
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = dbCon.Connection;
                cmd.CommandText = string.Format("INSERT INTO `area`.`services` (`ID`, `Name`, `Description`, `RegisteredUsers`, `Actions`, `Reactions`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
                    model.Id, model.Name, model.Description, UserModel.Parse(model.RegisteredUsers), ActionModel.Parse(model.Actions), ReactionModel.Parse(model.Reactions));
                int numRowsUpdated = cmd.ExecuteNonQuery();
                cmd.Dispose();
                Cache.Add(model);
            }
            else
                Logger.Error("Can't update database 'Services'.");
        }


        public static ServiceModel GetModelById(int id)
        {
            ServiceModel model = null;

            model = Cache.Find(f => f.Id == id);
            if (model == null || model == default(ServiceModel))
                return (null);
            return (model);
        }


        public static ServiceModel GetModelByActionId(int id)
        {
            ServiceModel model = null;

            model = Cache.Find(f => f.Actions.FindAll(s => s.Id == id).Count != 0);
            if (model == null || model == default(ServiceModel))
                return (null);
            return (model);
        }

        #endregion

    }
}

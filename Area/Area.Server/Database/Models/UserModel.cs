using Area.Server.Database.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Server.Database.Models
{
    public class UserModel : IModel
    {

        #region "Variables"

        public override int Id { get; }

        public string Username { get; set; }

        public string Name { get; set; }

        public string Mail { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }

        #endregion

        #region "Builder"

        public UserModel()
        { }

        public UserModel(int id, string username, string name, string mail, string password, string token)
        {
            Id = id;
            Name = name;
            Username = username;
            Password = password;
            Mail = mail;
            Token = token;
        }

        public UserModel(string username, string name, string mail, string password, string token)
        {
            Id = UserTable.Cache.Count;
            Name = name;
            Username = username;
            Password = password;
            Mail = mail;
            Token = token;
        }

        #endregion

        #region "Methods"

        public static string Parse(List<UserModel> models)
        {
            string response = "";

            foreach (UserModel model in models)
                response += model.Id + ";";
            return (response);
        }

        public override void Update()
        {
            UserTable.UpdateModel(this);
        }

        #endregion

    }
}

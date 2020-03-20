using Area.Server.Database.Tables;
using Area.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Server.Database.Models
{
    public class AccountModel : IModel
    {

        #region "Variables"

        public override int Id { get; }

        public int Service { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public int OwnerId { get; set; }

        #endregion

        #region "Builder"

        public AccountModel()
        { }

        public AccountModel(int id, int service, string username, string password, int ownerId)
        {
            Id = id;
            Service = service;
            Username = username;
            Password = password;
            OwnerId = ownerId;
        }

        public AccountModel(ServiceEnum service, string username, string password, int ownerId)
        {
            Id = AccountTable.Cache.Count;
            Service = (int)service;
            Username = username;
            Password = password;
            OwnerId = ownerId;
        }

        #endregion

        #region "Methods"

        public override void Update()
        {
            AccountTable.UpdateModel(this);
        }

        #endregion
    }
}

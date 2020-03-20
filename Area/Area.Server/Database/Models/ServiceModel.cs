using Area.Server.Database.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Server.Database.Models
{
    public class ServiceModel : IModel
    {

        #region "Variables"

        public override int Id { get; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<UserModel> RegisteredUsers { get; set; }

        public List<ActionModel> Actions { get; set; }

        public List<ReactionModel> Reactions { get; set; }

        #endregion

        #region "Builder"

        public ServiceModel()
        { }

        public ServiceModel(int id, string name, string description, string users, string actions, string reactions)
        {
            Id = id;
            Name = name;
            Description = description;
            if (users.Length != 0)
                RegisteredUsers = UserTable.Parse(users);
            else
                RegisteredUsers = new List<UserModel>();
            if (actions.Length != 0)
                Actions = ActionTable.Parse(actions);
            else
                Actions = new List<ActionModel>();
            if (reactions.Length != 0)
                Reactions = ReactionTable.Parse(reactions);
            else
                Reactions = new List<ReactionModel>();
        }

        public ServiceModel(string name, string description, string users, string actions, string reactions)
        {
            Id = ServiceTable.Cache.Count;
            Name = name;
            Description = description;
            if (users.Length != 0)
                RegisteredUsers = UserTable.Parse(users);
            else
                RegisteredUsers = new List<UserModel>();
            if (actions.Length != 0)
                Actions = ActionTable.Parse(actions);
            else
                Actions = new List<ActionModel>();
            if (reactions.Length != 0)
                Reactions = ReactionTable.Parse(reactions);
            else
                Reactions = new List<ReactionModel>();
        }

        #endregion

        #region "Methods"

        public string ToString()
        {
            string value = "{ \"name\": \"" + Name + "\", \"actions\": [";
            foreach(ActionModel action in Actions)
            {
                value += action.ToString();
                if (action != Actions[Actions.Count - 1])
                    value += ", ";
            }
            value += "], \"reactions\": [";
            foreach (ReactionModel reaction in Reactions)
            {
                value += reaction.ToString();
                if (reaction != Reactions[Reactions.Count - 1])
                    value += ", ";
            }
            value += "] }";
            return (value);
        }

        public override void Update()
        {
            ServiceTable.UpdateModel(this);
        }

        #endregion

    }
}

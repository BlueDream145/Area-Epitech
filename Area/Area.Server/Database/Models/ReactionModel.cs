using Area.Server.Database.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Server.Database.Models
{
    public class ReactionModel : IModel
    {

        #region "Variables"

        public override int Id { get; }

        public string Name { get; set; }

        public string Description { get; set; }

        #endregion

        #region "Builder"

        public ReactionModel()
        { }

        public ReactionModel(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public ReactionModel(string name, string description)
        {
            Id = ReactionTable.Cache.Count;
            Name = name;
            Description = description;
        }

        #endregion

        #region "Methods"

        public static string Parse(List<ReactionModel> models)
        {
            string response = "";

            foreach (ReactionModel model in models)
                response += model.Id + ";";
            return (response);
        }

        public string ToString()
        {
            return ("{ \"name\": \"" + Name + "\", \"description\": \"" + Description + "\" }");
        }

        public override void Update()
        {
            ReactionTable.UpdateModel(this);
        }

        #endregion
    }
}

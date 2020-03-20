using Area.Server.Database.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Server.Database.Models
{
    public class ActionModel : IModel
    {

        #region "Variables"

        public override int Id { get; }

        public string Name { get; set; }

        public string Description { get; set; }

        #endregion

        #region "Builder"

        public ActionModel()
        { }

        public ActionModel(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public ActionModel(string name, string description)
        {
            Id = ActionTable.Cache.Count;
            Name = name;
            Description = description;
        }

        #endregion

        #region "Methods"

        public static string Parse(List<ActionModel> models)
        {
            string response = "";

            foreach (ActionModel model in models)
                response += model.Id + ";";
            return (response);
        }

        public string ToString()
        {
            return ("{ \"name\": \"" + Name + "\", \"description\": \"" + Description + "\" }");
        }

        public override void Update()
        {
            ActionTable.UpdateModel(this);
        }

        #endregion
    }
}

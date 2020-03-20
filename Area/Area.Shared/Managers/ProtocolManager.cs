using Area.Shared.Protocol;
using Area.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Area.Shared.Managers
{
    public static class ProtocolManager
    {

        #region "Variables"


        public static Dictionary<int, Type> Cache;

        private static bool initialized = false;

        #endregion

        #region "Builder"

        static ProtocolManager()
        {
            Cache = new Dictionary<int, Type>();
            initialized = false;
        }

        #endregion

        #region "Methods"

        public static void Initialize()
        {
            if (initialized)
                return;
            Assembly ass = Assembly.GetAssembly(typeof(ProtocolManager));
            foreach(Type t in ass.GetTypes())
            {
                if (t.BaseType != typeof(NetworkMessage))
                    continue;
                FieldInfo field = t.GetField("ProtocolId");
                if (field == null)
                    continue;
                object val = field.GetValue(null);
                int id = (int)val;
                if (id >= 0)
                    Cache.Add(id, t);
            }
            Logger.Debug("Network messages initialized (Len=" + Cache.Count + ")");
            initialized = true;
        }

        public static KeyValuePair<Type, NetworkMessage> GetMessageInstance(int id)
        {
            if (!initialized || !Cache.ContainsKey(id))
                return new KeyValuePair<Type, NetworkMessage>(null, null);
            Type t = null;
            Cache.TryGetValue(id, out t);
            if (t == null)
                return new KeyValuePair<Type, NetworkMessage>(null, null);
            NetworkMessage instance = (NetworkMessage)Activator.CreateInstance(t);
            return new KeyValuePair<Type, NetworkMessage>(t, instance);
        }

        public static KeyValuePair<Type, NetworkMessage> GetMessageInstance(string name)
        {
            if (!initialized)
                return new KeyValuePair<Type, NetworkMessage>(null, null);
            Type t = null;
            foreach (KeyValuePair<int, Type> pair in Cache)
            {
                if (pair.Value.Name.ToLower().CompareTo(name.ToLower()) == 0)
                {
                    t = pair.Value;
                    break;
                }
            }
            if (t == null)
                return new KeyValuePair<Type, NetworkMessage>(null, null);
            NetworkMessage instance = (NetworkMessage)Activator.CreateInstance(t);
            return new KeyValuePair<Type, NetworkMessage>(t, instance);
        }

        #endregion

    }
}

using Area.Server.Handlers;
using Area.Shared.Protocol;
using Area.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Area.Server.Managers
{
    public class HandlersManager
    {


        #region "Variables"


        public static Dictionary<Type, KeyValuePair<Type, MethodInfo>> Cache;

        private static bool initialized = false;

        #endregion

        #region "Builder"

        static HandlersManager()
        {
            Cache = new Dictionary<Type, KeyValuePair<Type, MethodInfo>>();
            initialized = false;
        }

        #endregion

        #region "Methods"

        public static void Initialize()
        {
            if (initialized)
                return;
            Assembly ass = Assembly.GetCallingAssembly();
            foreach (Type t in ass.GetTypes())
            {
                if (t.BaseType != typeof(IHandler))
                    continue;
                MethodInfo[] methods = t.GetMethods();
                foreach(MethodInfo method in methods)
                {
                    if (method.Name.ToLower().StartsWith("handle"))
                    {
                        ParameterInfo[] param = method.GetParameters();
                        if (param[0].ParameterType.BaseType != typeof(NetworkMessage))
                            continue;
                        Cache.Add(param[0].ParameterType, new KeyValuePair<Type, MethodInfo> (t, method));
                    }
                }
            }
            Logger.Debug("Handlers initialized (Len=" + Cache.Count + ")");
            initialized = true;
        }

        public static KeyValuePair<Type, MethodInfo> GetMessageHandler(Type t)
        {
            KeyValuePair<Type, MethodInfo> method = new KeyValuePair<Type, MethodInfo>(null, null);

            if (!Cache.ContainsKey(t))
                return (method);
            Cache.TryGetValue(t, out method);
            return (method);
        }

        #endregion


    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Area.Shared.Utils
{
    public static class Logger
    {

        #region "Methods"

        public static void Debug(string message)
        {
            Log(message, "Debug", ConsoleColor.Cyan);
        }

        public static void Error(string message)
        {
            Log(message, "Error", ConsoleColor.Red);
        }

        public static void Info(string message)
        {
            Log(message, "Info", ConsoleColor.Green);
        }

        public static void Log(string message, string prefix, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(string.Format("<{0}> {1}", prefix, message));
            Console.ForegroundColor = ConsoleColor.White;
        }

        #endregion

    }
}

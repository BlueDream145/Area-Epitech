using System;

namespace Area.Server
{
    public class Program
    {
        private static Engine engine;

        static void Main(string[] args)
        {
            engine = new Engine();
            engine.StartProgram();
        }
    }
}

using System;
using Xle.XleSystem;

namespace Xle.Blacksilver.Desktop
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            using (var game = new LegendOfBlacksilverGame(Config.ParseCommandLineArgs(args)))
                game.Run();
        }
    }
}

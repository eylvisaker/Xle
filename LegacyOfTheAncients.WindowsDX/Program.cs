using System;
using Xle.XleSystem;

namespace Xle.Ancients.WindowsDX
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(params string[] args)
        {
            using (var game = new LegacyOfTheAncientsGame(Config.ParseCommandLineArgs(args)))
                game.Run();
        }
    }
#endif
}

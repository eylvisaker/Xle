﻿using System;
using Xle.XleSystem;

namespace Xle.Ancients.Desktop
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
        static void Main(params string[] args)
        {
            using (var game = new LegacyOfTheAncientsGame(Config.ParseCommandLineArgs(args)))
                game.Run();
        }
    }
}

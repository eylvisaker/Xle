using System;
using System.Collections.Generic;
using System.Text;

namespace Xle.XleSystem
{
    public class Config
    {
        public bool FullScreen { get; set; } = true;

        public static Config ParseCommandLineArgs(string[] args)
        {
            Config result = new Config();

            foreach(var arg in args)
            {
                if (arg == "-window")
                    result.FullScreen = false;
            }

            return result;
        }
    }
}

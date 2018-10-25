using AgateLib;

namespace Xle
{
    [Singleton]
    public class XleOptions
    {
        public bool EnhancedUserInterface { get; set; }
        public bool EnhancedGameplay { get; set; }

        public bool DisableOutsideEncounters { get; set; }

        public bool DisableExhibitsRequireCoins { get; set; }

        public bool EnableDebugMode { get; set; }


        public const int myWindowWidth = 640;
        public const int myWindowHeight = 400;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services;

namespace ERY.Xle
{
    public class XleOptions : IXleService
    {
        public bool EnhancedUserInterface { get; set; }
        public bool EnhancedGameplay { get; set; }

        public bool DisableOutsideEncounters { get; set; }

        public bool DisableExhibitsRequireCoins { get; set; }

        public bool EnableDebugMode { get; set; }
    }
}

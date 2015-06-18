using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Services.Implementation
{
    public class SoundManProxy : ISoundMan
    {
        public void Load()
        {
            SoundMan.Load();
        }
    }
}

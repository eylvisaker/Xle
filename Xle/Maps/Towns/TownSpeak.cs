using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Services;
using Xle.Services.Commands.Implementation;

namespace Xle.Maps.Towns
{
    [ServiceName("TownSpeak")]
    public class TownSpeak : Speak
    {
        public override void Execute()
        {
            if (SpeakToEvent())
                return;

            if (CheckSpeakToGuard())
                return;

            PrintNoResponseMessage();
        }

        private bool CheckSpeakToGuard()
        {
            var guards = GameState.Map.Guards;

            for (int j = -1; j < 3; j++)
            {
                for (int i = -1; i < 3; i++)
                {
                    foreach (var guard in guards)
                    {
                        if ((guard.X == Player.X + i || guard.X + 1 == Player.X + i) &&
                            (guard.Y == Player.Y + j || guard.Y + 1 == Player.Y + j))
                        {
                            SpeakToGuard();
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        protected virtual void SpeakToGuard()
        {
            TextArea.PrintLine("\n\nThe guard salutes.");
        }

    }
}

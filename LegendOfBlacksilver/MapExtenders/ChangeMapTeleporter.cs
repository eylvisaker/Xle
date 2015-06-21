using ERY.Xle.Services.Implementation;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders
{
    public class ChangeMapTeleporter : ChangeMap
    {
        protected LobStory Story { get { return GameState.Story(); } }

        protected override bool OnStepOnImpl(GameState state, ref bool cancel)
        {
            return ExecuteTeleportation();
        }

        protected bool ExecuteTeleportation()
        {
            TeleportAnimation();
            ExecuteMapChange();

            return true;
        }

        protected void TeleportAnimation()
        {
            SoundMan.PlaySound(LotaSound.Teleporter);

            Stopwatch watch = new Stopwatch();
            watch.Start();

            while (watch.ElapsedMilliseconds < 100)
                GameControl.Redraw();

            while (watch.ElapsedMilliseconds < 1800)
            {
                int index = ((int)watch.ElapsedMilliseconds % 80) / 50;

                if (index == 0)
                    Player.RenderColor = XleColor.Black;
                else
                    Player.RenderColor = XleColor.White;

                GameControl.Redraw();
            }

            Player.RenderColor = XleColor.White;

            while (watch.ElapsedMilliseconds < 2000)
            {
                GameControl.Redraw();
            }
        }
    }
}

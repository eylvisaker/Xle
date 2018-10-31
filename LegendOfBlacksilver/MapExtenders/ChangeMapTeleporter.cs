﻿using Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.Blacksilver.MapExtenders
{
    public class ChangeMapTeleporter : ChangeMap
    {
        protected LobStory Story { get { return GameState.Story(); } }

        protected override async Task<bool> OnStepOnImpl()
        {
            await ExecuteTeleportation();
            return true;
        }

        protected async Task ExecuteTeleportation()
        {
            await TeleportAnimation();
            await ExecuteMapChange();
        }

        protected async Task TeleportAnimation()
        {
            SoundMan.PlaySound(LotaSound.Teleporter);

            Stopwatch watch = new Stopwatch();
            watch.Start();

            await GameControl.WaitAsync(100);
            
            while (watch.ElapsedMilliseconds < 1800)
            {
                int index = ((int)watch.ElapsedMilliseconds % 80) / 50;

                if (index == 0)
                    Player.RenderColor = XleColor.Black;
                else
                    Player.RenderColor = XleColor.White;

                await GameControl.WaitAsync(1);
            }

            Player.RenderColor = XleColor.White;

            await GameControl.WaitAsync(200);
        }
    }
}

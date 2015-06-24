﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Castle.Core;

using ERY.Xle.Maps.Extenders;
using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Services.Implementation.Commands.MapSpecific;

namespace ERY.Xle.Services.Implementation.Commands
{
    [ServiceName("Dungeon")]
    public class DungeonClimb : Climb
    {
        public IXleGameControl GameControl { get; set; }
        public IMapChanger MapChanger { get; set; }

        Dungeon TheMap { get { return (Dungeon)GameState.Map; } }
        DungeonExtender Map { get { return (DungeonExtender)GameState.MapExtender; } }
        
        public override void Execute()
        {
            switch (TheMap[Player.X, Player.Y])
            {
                case 0x11:
                    if (Player.DungeonLevel == 0)
                    {
                        TextArea.PrintLine("\n\nYou climb out of the dungeon.");

                        OnPlayerExitDungeon();

                        GameControl.Wait(1000);

                        MapChanger.ReturnToPreviousMap();

                        return;
                    }
                    else
                    {
                        Player.DungeonLevel--;
                    }
                    break;

                case 0x12:
                    Player.DungeonLevel++;
                    break;

                default:
                    FailMessage();
                    return;

            }

            DungeonLevelText();
        }

        private void OnPlayerExitDungeon()
        {
            Map.OnPlayerExitDungeon(Player);
        }

        private void DungeonLevelText()
        {
            Map.CurrentLevel = Player.DungeonLevel;

            if (TheMap[Player.X, Player.Y] == 0x21) TheMap[Player.X, Player.Y] = 0x11;
            if (TheMap[Player.X, Player.Y] == 0x22) TheMap[Player.X, Player.Y] = 0x12;

            TextArea.PrintLine("\n\nYou are now at level " + (Player.DungeonLevel + 1).ToString() + ".", XleColor.White);
        }

    }
}

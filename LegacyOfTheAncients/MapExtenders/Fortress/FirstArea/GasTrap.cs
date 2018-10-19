﻿using Xle.Maps;
using System;
using System.Linq;

namespace Xle.Ancients.MapExtenders.Fortress.FirstArea
{
    public class GasTrap : LotaEvent
    {
        public Random Random { get; set; }

        public override bool StepOn()
        {
            if (Player.Y > TheEvent.Y + 2)
                return false;

            Enabled = false;

            MapData data = (this).GameState.Map.ReadMapData(TheEvent.Rectangle);

            TextArea.Clear(true);
            TextArea.PrintLine();
            TextArea.PrintLine("Doors slam shut...");
            AddDoors();

            GameControl.Wait(1000);

            TextArea.PrintLine();
            TextArea.PrintLine("Gas fills the room...");
            AddGas();

            GameControl.Wait(3500);

            TextArea.PrintLine();
            TextArea.PrintLine("You fall asleep.");
            TextArea.PrintLine();

            GameControl.Wait(3000);

            GameControl.Wait(4000, redraw: DrawBlankScreen);

            RemoveWeaponsAndArmor();

            Player.X = 25;
            Player.Y = 45;

            GameControl.Wait(3500);

            GameState.Map.WriteMapData(data, TheEvent.Rectangle.Location);

            return true;
        }

        private void RemoveWeaponsAndArmor()
        {
            Player.Weapons.Clear();
            Player.Armor.Clear();
        }

        private void DrawBlankScreen()
        {
            //Display.BeginFrame();
            //Display.Clear(XleColor.Gray);
            //Display.EndFrame();
            throw new NotImplementedException();
        }

        private void AddGas()
        {
            var gasGroup = GameState.Map.TileSet.TileGroups.FirstOrDefault(x => x.GroupType == Maps.GroupType.Special2);

            int startx = StartX();

            for (int j = TheEvent.Rectangle.Y + 1; j < TheEvent.Rectangle.Bottom - 1; j++)
            {
                for (int i = startx; i < startx + 3; i++)
                {
                    GameState.Map[i, j] = gasGroup.RandomTile(Random);
                }
            }
        }

        private void AddDoors()
        {
            var doorGroup = GameState.Map.TileSet.TileGroups.FirstOrDefault(x => x.GroupType == Maps.GroupType.Door);

            int startx = StartX();

            for (int i = startx; i < startx + 3; i++)
            {
                int tile = doorGroup.Tiles[(i - startx) % doorGroup.Tiles.Count];

                GameState.Map[i, TheEvent.Y] = tile;
                GameState.Map[i, TheEvent.Rectangle.Bottom - 1] = tile;
            }
        }

        private int StartX()
        {
            int x = TheEvent.X;

            if (Player.X > TheEvent.X + 4)
                x = TheEvent.X + 5;

            return x;
        }
    }
}

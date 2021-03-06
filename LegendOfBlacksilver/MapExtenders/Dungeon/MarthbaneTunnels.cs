﻿using AgateLib;
using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Maps;
using Xle;
using Xle.Commands;
using Xle.Menus;

namespace Xle.Blacksilver.MapExtenders.Dungeon
{
    [Transient("MarthbaneTunnels")]
    public class MarthbaneTunnels : LobDungeon
    {
        DungeonMonster king;

        public DungeonMonster King { get { return king; } }

        public override void SetCommands(ICommandList commands)
        {
            base.SetCommands(commands);

            commands.Items.Remove(commands.Items.First(x => x.Name == "Speak"));

            commands.Items.Add(CommandFactory.Speak("MarthbaneSpeak"));
        }

        public override void OnLoad()
        {
            base.OnLoad();

            if (Story.RescuedKing)
            {
                OpenEscapeRoute();
            }
            else
            {
                king = new DungeonMonster(Data.DungeonMonsters[18])
                    {
                        DungeonLevel = 7,
                        Location = new Point(10, 0),
                        HP = 400,
                        KillFlashImmune = true,
                    };

                Combat.Monsters.Add(king);
            }
        }
        protected override int MonsterGroup(int dungeonLevel)
        {
            if (dungeonLevel <= 2) return 0;
            if (dungeonLevel <= 6) return 1;

            return 2;
        }

        public override DungeonMonster GetMonsterToSpawn()
        {
            if (Player.DungeonLevel == 7)
                return null;

            return base.GetMonsterToSpawn();
        }

        public override bool SpawnMonsters()
        {
            if (Player.DungeonLevel == 7)
                return false;
            else
                return true;
        }

        public override Task UpdateMonsters()
        {
            // disable normal monster processing if we see the king.
            if (Player.DungeonLevel == 7)
                return Task.CompletedTask;

            return base.UpdateMonsters();
        }

        public void OpenEscapeRoute()
        {
            // 11, 0, 7 change to 17
            // 13, 0, 4 change to 18

            TheMap[11, 0, 7] = 17;
            TheMap[13, 0, 4] = 18;
        }

        public override Map3DSurfaces Surfaces()
        {
            return Lob3DSurfaces.MarthbaneTunnels;
        }
    }
}

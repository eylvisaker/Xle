using System;
using System.Threading.Tasks;
using Xle.Data;
using Xle.Maps;
using Xle.Maps.Dungeons;
using Xle.Commands;

namespace Xle.Blacksilver.MapExtenders.Dungeon
{
    public abstract class LobDungeon : DungeonExtender
    {
        protected LobStory Story { get { return GameState.Story(); } }

        public override void SetCommands(ICommandList commands)
        {
            commands.Items.Add(CommandFactory.Armor());
            commands.Items.Add(CommandFactory.Gamespeed());
            commands.Items.Add(CommandFactory.Inventory());
            commands.Items.Add(CommandFactory.Pass());
            commands.Items.Add(CommandFactory.Weapon());

            commands.Items.Add(CommandFactory.Climb("DungeonClimb"));
            commands.Items.Add(CommandFactory.End());
            commands.Items.Add(CommandFactory.Fight("LobDungeonFight"));
            commands.Items.Add(CommandFactory.Magic("LobDungeonMagic"));
            commands.Items.Add(CommandFactory.Open("DungeonOpen"));
            commands.Items.Add(CommandFactory.Speak());
            commands.Items.Add(CommandFactory.Use("LobUse"));
            commands.Items.Add(CommandFactory.Xamine("LobDungeonXamine"));
        }

        public override int GetTreasure(int dungeonLevel, int chestID)
        {
            if (chestID == 1)
                return (int)LobItem.SilverCoin;

            return 0;
        }

        public override string TrapName(int val)
        {
            switch (val)
            {
                case 0x11: return "ceiling hole";
                case 0x12: return "floor hole";
                case 0x13: return "poison dart";
                case 0x14: return "tendril colony";
                case 0x15: return "trip bar";
                case 0x16: return "gas vent";
                default: return base.TrapName(val);
            }
        }

        protected abstract int MonsterGroup(int dungeonLevel);

        public override DungeonMonster GetMonsterToSpawn()
        {
            if (Random.NextDouble() > 0.07)
                return null;

            int monsterID = Random.Next(6);

            monsterID += 6 * MonsterGroup(Player.DungeonLevel + 1);

            DungeonMonster monst = new DungeonMonster(
                Data.DungeonMonsters[monsterID]);

            monst.HP = (int)
                (TheMap.MonsterHealthScale * (.7 + .6 * Random.NextDouble()) * (1 + monsterID / 20.0));

            return monst;
        }

        public override async Task CastSpell(MagicSpell magic)
        {
            await TextArea.PrintLine("Cast " + magic.Name + ".", XleColor.White);

            if (magic.ID == 5)
                await ExecuteKillFlash();
        }

    }
}

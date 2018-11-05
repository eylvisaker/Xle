using AgateLib.Diagnostics;
using Xle.Data;
using Xle.Maps;
using Xle.Maps.Dungeons;
using Xle.Commands;
using Xle.Game;
using Xle.MapLoad;
using Xle.ScreenModel;
using Xle.XleEventTypes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using Xle.XleSystem;

namespace Xle.Diagnostics
{
    [Singleton]
    public class XleGameConsoleCommands : Vocabulary
    {
        private GameState GameState;
        private ITextArea TextArea;
        private XleData Data;
        private ICommandExecutor commandExecutor;
        private IMapLoader mapLoader;
        private XleOptions options;
        private IXleGameControl gameControl;
        private readonly XleSystemState systemState;
        private IMapChanger mapChanger;

        public XleGameConsoleCommands(
            GameState gameState,
            ITextArea textArea,
            ICommandExecutor commandExecutor,
            IMapLoader mapLoader,
            IXleGameControl gameControl,
            IMapChanger mapChanger,
            XleSystemState systemState,
            XleOptions options,
            XleData data)
        {
            this.mapLoader = mapLoader;
            this.GameState = gameState;
            this.TextArea = textArea;
            this.commandExecutor = commandExecutor;
            this.gameControl = gameControl;
            this.systemState = systemState;
            this.options = options;
            this.Data = data;
            this.mapChanger = mapChanger;
        }

        public override string Path => "";

        public override bool IsValid => true;

        [ConsoleCommand("Kills all the guards or monsters on the map.")]
        private void KillAll()
        {
            if (GameState.MapExtender is DungeonExtender)
            {
                var dung = GameState.MapExtender as DungeonExtender;

                dung.Combat.Monsters.Clear();
            }
            else
            {
                if (GameState.Map.HasGuards == false)
                {
                    throw new InvalidOperationException("There are no guards or monsters on this map.");
                }

                GameState.Map.Guards.Clear();
            }
        }

        //private string CommandProcessor_DescribeCommand(string command)
        //{
        //    StringBuilder b = new StringBuilder();

        //    switch (command)
        //    {
        //        case "goto":
        //            b.AppendLine("Jumps to the entrance of the specified map. Allowed map values are: ");

        //            ConsolePrintMapList(b);

        //            break;

        //        case "enter":
        //            b.AppendLine("Jumps to the specified map. You can specify the map name and optionally and entry point.");

        //            ConsolePrintMapList(b);

        //            break;
        //    }

        //    return b.ToString();
        //}

        //private void ConsolePrintMapList(StringBuilder b)
        //{
        //    bool comma = false;
        //    int count = 0;

        //    foreach (var map in Data.MapList)
        //    {
        //        if (comma)
        //            b.Append(", ");
        //        if (count == 0)
        //            b.Append("    ");

        //        b.Append(map.Value.Alias);
        //        comma = true;

        //        count++;
        //        if (count > 4)
        //        {
        //            b.AppendLine();
        //            count = 0;
        //        }
        //    }
        //}

        [ConsoleCommand("Moves to a specified place on the current map. Pass no arguments to see the current position. Pass two arguments to set x,y. Pass three arguments to set x,y,level for dungeons.")]
        private void Move(int x = -1, int y = -1, int level = -1)
        {
            Player player = GameState.Player;

            if (x == -1)
            {
                if (GameState.Map.IsMultiLevelMap)
                {
                    Shell.WriteLine($"Current Position: {player.X}, {player.Y}, level: {player.DungeonLevel + 1}");
                }
                else
                {
                    Shell.WriteLine($"Current Position: {player.X}, {player.Y}");
                }
                return;
            }
            else if (y == -1)
                throw new Exception("You must pass x and y to move.");

            if (x < 0) throw new Exception("x cannot be less than zero.");
            if (y < 0) throw new Exception("y cannot be less than zero.");
            if (x >= GameState.Map.Width) throw new Exception(string.Format("x cannot be {0} or greater.", GameState.Map.Width));
            if (y >= GameState.Map.Height) throw new Exception(string.Format("y cannot be {0} or greater.", GameState.Map.Height));

            if (level == -1)
            {
                player.X = x;
                player.Y = y;
            }
            else
            {
                if (GameState.Map.IsMultiLevelMap == false)
                    Shell.WriteLine("Cannot pass level on a map without levels.");
                else
                {
                    if (level < 1 || level > GameState.Map.Levels)
                        throw new Exception(string.Format("level cannot be less than 1 or greater than {0}", GameState.Map.Levels));

                    player.X = x;
                    player.Y = y;
                    player.DungeonLevel = level - 1;

                    var dungeon = GameState.MapExtender as DungeonExtender;
                    dungeon.CurrentLevel = player.DungeonLevel;
                }
            }
        }

        [ConsoleCommand("Enters a map.")]
        public void Enter(string mapName, int entryPoint = 0)
        {
            MapInfo mapInfo = FindMapByPartialName(mapName);

            if (mapInfo == null)
                return;

            ChangeMap(GameState.Player, mapInfo.ID, entryPoint);

            TextArea.Clear();
            commandExecutor.Prompt();
        }

        private void ChangeMap(Player player, int mapId, int entryPoint)
        {
            mapChanger.ChangeMap(mapId, entryPoint);
        }

        [ConsoleCommand("Enters a map.")]
        public void Goto(string mapName)
        {
            Player player = GameState.Player;

            MapInfo mapInfo = FindMapByPartialName(mapName);

            if (mapInfo == null)
                return;

            var map = mapLoader.LoadMap(mapInfo.ParentMapID);
            int targetX = 0, targetY = 0;

            if (map == null)
            {
                Shell.WriteLine("Map not found.");
                return;
            }

            foreach (ChangeMapEvent evt in from evt in map.TheMap.Events
                                           where evt is ChangeMapEvent
                                           select (ChangeMapEvent)evt)
            {
                if (evt.MapID == mapInfo.ID)
                {
                    targetX = evt.X;
                    targetY = evt.Y;
                }
            }

            if (map.CanPlayerStepIntoImpl(targetX + 2, targetY))
                targetX += 2;
            else if (map.CanPlayerStepIntoImpl(targetX - 2, targetY))
                targetX -= 2;
            else if (map.CanPlayerStepIntoImpl(targetX, targetY + 2))
                targetY += 2;
            else if (map.CanPlayerStepIntoImpl(targetX, targetY - 2))
                targetY -= 2;

            ChangeMap(player, map.MapID, new Point(targetX, targetY));

            TextArea.Clear();
            commandExecutor.Prompt();
        }

        private void ChangeMap(Player player, int mapId, Point targetPoint)
        {
            mapChanger.ChangeMap(mapId, targetPoint);
        }

        private MapInfo FindMapByPartialName(string mapName)
        {
            IEnumerable<MapInfo> matches = from m in Data.MapList.Values
                                           where m.Alias.ToUpperInvariant().Contains(mapName.ToUpperInvariant())
                                           select m;

            MapInfo exactMatch = matches.FirstOrDefault(x => x.Alias.ToUpperInvariant() == mapName.ToUpperInvariant());

            if (matches.Count() == 0)
            {
                Shell.WriteLine("Map name not found.");
                return null;
            }
            else if (matches.Count() > 1 && exactMatch == null)
            {
                Shell.WriteLine("Found multiple matches:");

                foreach (var m in matches)
                {
                    Shell.WriteLine($"    {m.Alias}");
                }

                return null;
            }

            return exactMatch ?? matches.First();
        }

        [ConsoleCommand("Makes you super powerful.")]
        public void God()
        {
            Player player = GameState.Player;

            player.Cheater = true;
            player.Gold = 9999;
            player.GoldInBank = 99999;
            player.Food = 5000;
            player.Attribute[Attributes.strength] = 300;
            player.Attribute[Attributes.dexterity] = 300;
            player.Attribute[Attributes.intelligence] = 300;
            player.Attribute[Attributes.charm] = 300;
            player.Attribute[Attributes.endurance] = 300;

            foreach (var spell in Data.MagicSpells.Values)
            {
                player.Items[spell.ItemID] = spell.MaxCarry;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using AgateLib.Diagnostics;
using AgateLib.Geometry;

using ERY.Xle.Data;
using ERY.Xle.Maps;
using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Services.Commands;
using ERY.Xle.Services.Game;
using ERY.Xle.Services.MapLoad;
using ERY.Xle.Services.ScreenModel;
using ERY.Xle.XleEventTypes;

namespace ERY.Xle.Services.XleSystem.Implementation
{
    public class XleConsole : IXleConsole
    {
        private GameState GameState;
        private ITextArea TextArea;
        private XleData Data;
        private ICommandExecutor commandExecutor;
        private IMapLoader mapLoader;
        private XleOptions options;
        private IXleGameControl gameControl;
        private IMapChanger mapChanger;

        public XleConsole(
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

            AgateConsole.Initialize();

            AgateConsole.Instance.CommandProcessor.DescribeCommand += CommandProcessor_DescribeCommand;

            AgateConsole.Commands.Add("gold", new Action<int>(CheatGiveGold));
            AgateConsole.Commands.Add("food", new Action<int>(CheatGiveFood));
            AgateConsole.Commands.Add("level", new Action<int>(CheatLevel));
            AgateConsole.Commands.Add("goto", new Action<string>(CheatGoto));
            AgateConsole.Commands.Add("enter", new Action<string, int>(CheatEnter));
            AgateConsole.Commands.Add("move", new Action<int, int, int>(CheatMove));
            AgateConsole.Commands.Add("godmode", new Action(CheatGod));
            AgateConsole.Commands.Add("killall", new Action(CheatKillAll));
            AgateConsole.Commands.Add("encounters", new Action<string>(CheatEncounters));
            AgateConsole.Commands.Add("coins", new Action<string>(CheatCoins));
        }

        public void Initialize()
        {
            
        }

        [Description("Turns encounters on or off.\nUsage: encounters [on|off]")]
        private void CheatEncounters(string action)
        {
            if (action != null)
                action = action.ToLowerInvariant();

            if (action == "on")
                options.DisableOutsideEncounters = false;
            else if (action == "off")
                options.DisableOutsideEncounters = true;
            else if (string.IsNullOrEmpty(action))
                options.DisableOutsideEncounters = !options.DisableOutsideEncounters;
            else
                throw new ArgumentException("Could not understand '" + action + "'");

            AgateConsole.WriteLine("Outside encounters are now " + (
                options.DisableOutsideEncounters ? "off." : "on."));
        }

        [Description("Turns on or off whether exhibits require coins.\nUsage: coins [on|off].")]
        private void CheatCoins(string action = null)
        {
            if (action == null)
                action = "";

            action = action.ToLowerInvariant();

            if (action == "on")
                options.DisableExhibitsRequireCoins = false;
            else if (action == "off")
                options.DisableExhibitsRequireCoins = true;
            else if (action != "")
                throw new ArgumentException("Could not understand '" + action + "'");

            AgateConsole.WriteLine("Exhibits {0}{1}require coins. ",
                (action != "") ? "now " : "",
                options.DisableExhibitsRequireCoins ? "do not " : "");
        }


        [Description("Kills all the guards or monsters on the map.")]
        private void CheatKillAll()
        {
            if (GameState.Map is Dungeon)
            {
                var dung = GameState.Map as Dungeon;

                dung.Monsters.Clear();
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

        string CommandProcessor_DescribeCommand(string command)
        {
            StringBuilder b = new StringBuilder();

            switch (command)
            {
                case "goto":
                    b.AppendLine("Jumps to the entrance of the specified map. Allowed map values are: ");

                    ConsolePrintMapList(b);

                    break;

                case "enter":
                    b.AppendLine("Jumps to the specified map. You can specify the map name and optionally and entry point.");

                    ConsolePrintMapList(b);

                    break;
            }

            return b.ToString();
        }

        private void ConsolePrintMapList(StringBuilder b)
        {
            bool comma = false;
            int count = 0;

            foreach (var map in Data.MapList)
            {
                if (comma)
                    b.Append(", ");
                if (count == 0)
                    b.Append("    ");

                b.Append(map.Value.Alias);
                comma = true;

                count++;
                if (count > 4)
                {
                    b.AppendLine();
                    count = 0;
                }
            }
        }


        [Description("Gives gold")]
        void CheatGiveGold(int amount = 1000)
        {
            GameState.Player.Gold += amount;
        }
        [Description("Gives food")]
        void CheatGiveFood(int amount = 500)
        {
            GameState.Player.Food += amount;
        }
        [Description("Sets the players level. Gives items and sets story variables to be consistent with the level chosen. Does not affect weapons or armor.")]
        public void CheatLevel(int level)
        {
            systemState.Factory.CheatLevel(GameState.Player, level);
        }

        [Description("Moves to a specified place on the current map. Pass no arguments to see the current position. Pass two arguments to set x,y. Pass three arguments to set x,y,level for dungeons.")]
        private void CheatMove(int x = -1, int y = -1, int level = -1)
        {
            Player player = GameState.Player;

            if (x == -1)
            {
                if (GameState.Map.IsMultiLevelMap)
                {
                    AgateConsole.WriteLine("Current Position: {0}, {1}, level: {2}", player.X, player.Y, player.DungeonLevel + 1);
                }
                else
                {
                    AgateConsole.WriteLine("Current Position: {0}, {1}", player.X, player.Y);
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
                    AgateConsole.WriteLine("Cannot pass level on a map without levels.");
                else
                {
                    if (level < 1 || level > GameState.Map.Levels)
                        throw new Exception(string.Format("level cannot be less than 1 or greater than {0}", GameState.Map.Levels));

                    player.X = x;
                    player.Y = y;
                    player.DungeonLevel = level - 1;
                }
            }
        }


        private void CheatEnter(string mapName, int entryPoint = 0)
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
            mapChanger.ChangeMap(player, mapId, entryPoint);
        }

        public void CheatGoto(string mapName)
        {
            Player player = GameState.Player;

            MapInfo mapInfo = FindMapByPartialName(mapName);

            if (mapInfo == null)
                return;

            var map = mapLoader.LoadMap(mapInfo.ParentMapID);
            int targetX = 0, targetY = 0;

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

            if (map.CanPlayerStepIntoImpl(player, targetX + 2, targetY))
                targetX += 2;
            else if (map.CanPlayerStepIntoImpl(player, targetX - 2, targetY))
                targetX -= 2;
            else if (map.CanPlayerStepIntoImpl(player, targetX, targetY + 2))
                targetY += 2;
            else if (map.CanPlayerStepIntoImpl(player, targetX, targetY - 2))
                targetY -= 2;

            ChangeMap(player, map.MapID, new Point(targetX, targetY));

            TextArea.Clear();
            commandExecutor.Prompt();
        }

        private void ChangeMap(Player player, int mapId, Point targetPoint)
        {
            mapChanger.ChangeMap(player, mapId, targetPoint);
        }

        private MapInfo FindMapByPartialName(string mapName)
        {
            IEnumerable<MapInfo> matches = from m in Data.MapList.Values
                                           where m.Alias.ToUpperInvariant().Contains(mapName.ToUpperInvariant())
                                           select m;

            MapInfo exactMatch = matches.FirstOrDefault(x => x.Alias.ToUpperInvariant() == mapName.ToUpperInvariant());

            if (matches.Count() == 0)
            {
                AgateConsole.WriteLine("Map name not found.");
                return null;
            }
            else if (matches.Count() > 1 && exactMatch == null)
            {
                AgateConsole.WriteLine("Found multiple matches:");

                foreach (var m in matches)
                {
                    AgateConsole.WriteLine("    {0}", m.Alias);
                }

                return null;
            }

            return exactMatch ?? matches.First();
        }

        [Description("Makes you super powerful.")]
        public void CheatGod()
        {
            Player player = GameState.Player;

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


        public XleSystemState systemState { get; set; }
    }
}

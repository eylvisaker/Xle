using AgateLib.Diagnostics;
using Xle.Data;
using Xle.Maps;
using Xle.Maps.Dungeons;
using Xle.Services.Commands;
using Xle.Services.Game;
using Xle.Services.MapLoad;
using Xle.Services.ScreenModel;
using Xle.XleEventTypes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using Xle.Services.XleSystem;

namespace Xle.Diagnostics
{
    [Singleton]
    public class XleGlobalConsoleCommands : Vocabulary
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

        public XleGlobalConsoleCommands(
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

        [ConsoleCommand("Turns encounters on or off.\nUsage: encounters [on|off]")]
        private void Encounters(string action)
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

            Shell.WriteLine("Outside encounters are now " + (
                options.DisableOutsideEncounters ? "off." : "on."));
        }

        [ConsoleCommand("Turns on or off whether exhibits require coins.\nUsage: coins [on|off].")]
        private void Coins(string action = null)
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

            var now = (action != "") ? "now " : "";
            var require = options.DisableExhibitsRequireCoins ? "do not require" : "require";

            Shell.WriteLine($"Exhibits {now}{require} coins. ");
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


        //private void ChangeMap(Player player, int mapId, int entryPoint)
        //{
        //    mapChanger.ChangeMap(mapId, entryPoint);
        //}
    }
}

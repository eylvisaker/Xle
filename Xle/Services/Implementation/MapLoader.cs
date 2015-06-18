using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Rendering;

namespace ERY.Xle.Services.Implementation
{
    public class MapLoader : IMapLoader
    {
        private GameState gameState;
        private IXleLegacyCore legacyCore;
        private IXleRenderer renderer;

        public MapLoader(
            IXleLegacyCore legacyCore, 
            IXleRenderer renderer,
            GameState gameState)
        {
            this.legacyCore = legacyCore;
            this.renderer = renderer;
            this.gameState = gameState;
        }

        public void LoadMap(int mapId)
        {
            gameState.Map = legacyCore.LoadMap(gameState.Player.MapID);
            gameState.Map.OnLoad(gameState.Player);

            legacyCore.SetTilesAndCommands();

            renderer.FontColor = gameState.Map.ColorScheme.TextColor;
        }


        public void ChangeMap(Player player, int mapId, int entryPoint)
        {
            XleCore.ChangeMap(player, mapId, entryPoint);
        }
    }
}

using AgateLib;
using Xle.Maps;
using Xle.Game;
using Xle.XleSystem;
using Microsoft.Xna.Framework;
using System;
using System.Threading.Tasks;

namespace Xle.ScreenModel
{
    public interface IStatsDisplay
    {
        Color HPColor { get; set; }
        int HP { get; }
        int Gold { get; }
        int Food { get; }

        /// <summary>
        /// Resets the value of HPColor to the map's default color scheme.
        /// </summary>
        void ResetColor();
    }

    [Singleton, InjectProperties]
    public class StatsDisplay : IStatsDisplay
    {
        private bool overrideHPColor;
        private Color mHPColor;

        public GameState GameState { get; set; }

        private Player Player { get { return GameState.Player; } }
        private XleMap Map { get { return GameState.Map; } }
        private IMapExtender MapExtender { get { return GameState.MapExtender; } }

        public Color HPColor
        {
            get
            {
                if (overrideHPColor)
                    return mHPColor;
                else
                    return Map.ColorScheme.TextColor;
            }
            set
            {
                mHPColor = value;
                overrideHPColor = true;
            }
        }

        public int HP { get { return Player.HP; } }
        public int Gold { get { return Player.Gold; } }
        public int Food { get { return (int)Player.Food; } }

        public void ResetColor()
        {
            overrideHPColor = false;
        }
    }
}

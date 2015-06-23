using ERY.Xle.Maps;
using ERY.Xle.Maps.XleMapTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Services.Implementation
{
    public class PlayerDeathHandler : IPlayerDeathHandler
    {
        public PlayerDeathHandler(IMapChanger mapChanger, IStatsDisplay statsDisplay)
        {
            this.MapChanger = mapChanger;
            this.StatsDisplay = statsDisplay;
        }
        public ITextArea TextArea { get; set; }
        public ISoundMan SoundMan { get; set; }
        IStatsDisplay StatsDisplay { get; set; }
        public GameState GameState { get; set; }
        IMapChanger MapChanger { get; set; }
        public IXleGameControl GameControl { get; set; }
        public Random Random { get; set; }

        protected Player Player { get { return GameState.Player; } }
        protected XleMap Map { get { return GameState.Map; } }

        public virtual void PlayerIsDead()
        {
            TextArea.PrintLine();
            TextArea.PrintLine();
            TextArea.PrintLine("            You died!");
            TextArea.PrintLine();
            TextArea.PrintLine();

            SoundMan.PlaySound(LotaSound.VeryBad);

            StatsDisplay.FlashHPWhileSound(XleColor.Red, XleColor.Yellow);

            LoadOutsideMap();

            Outside map = (Outside)GameState.Map;
            TerrainType t;

            do
            {
                Player.X = Random.Next(Map.Width);
                Player.Y = Random.Next(Map.Height);

                t = map.TerrainAt(Player.X, Player.Y);

            } while (t != TerrainType.Grass && t != TerrainType.Forest);

            Player.Rafts.Clear();

            Player.HP = Player.MaxHP;
            Player.Food = 30 + Random.Next(10);
            Player.Gold = 25 + Random.Next(30);
            Player.BoardedRaft = null;

            while (SoundMan.IsPlaying(LotaSound.VeryBad))
                GameControl.Wait(40);

            TextArea.PrintLine("The powers of the museum");
            TextArea.PrintLine("resurrect you from the grave!");
            TextArea.PrintLine();

            SoundMan.PlaySoundSync(LotaSound.VeryGood);
        }

        protected virtual void LoadOutsideMap()
        {
            MapChanger.ChangeMap(GameState.Player, 1, -1);
        }

    }
}

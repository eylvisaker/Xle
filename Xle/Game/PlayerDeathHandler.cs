﻿using AgateLib;
using System;
using System.Threading.Tasks;
using Xle.Maps;
using Xle.Maps.Outdoors;
using Xle.Maps.XleMapTypes;
using Xle.MapLoad;
using Xle.ScreenModel;
using Xle.XleSystem;

namespace Xle.Game
{
    public interface IPlayerDeathHandler
    {
        Task PlayerIsDead();
    }

    [Singleton, InjectProperties]
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

        public virtual async Task PlayerIsDead()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine();
            await TextArea.PrintLine("            You died!");
            await TextArea.PrintLine();
            await TextArea.PrintLine();

            SoundMan.PlaySound(LotaSound.VeryBad);

            await GameControl.FlashHPWhileSound(XleColor.Red, XleColor.Yellow);

            await LoadOutsideMap();

            IOutsideExtender extender = (IOutsideExtender)GameState.MapExtender;
            Outside map = (Outside)extender.TheMap;

            extender.CancelEncounter();

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
            Player.LastAttackedMapID = 0;

            while (SoundMan.IsPlaying(LotaSound.VeryBad))
                await GameControl.WaitAsync(40);

            await TextArea.PrintLine("The powers of the museum");
            await TextArea.PrintLine("resurrect you from the grave!");
            await TextArea.PrintLine();

            await GameControl.PlaySoundWait(LotaSound.VeryGood);
        }

        protected virtual async Task LoadOutsideMap()
        {
            await MapChanger.ChangeMap(1, -1);
        }

    }
}

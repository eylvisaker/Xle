using AgateLib.DisplayLib;
using AgateLib.Serialization.Xle;
using ERY.Xle.Maps.XleMapTypes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services;
using ERY.Xle.Services.Game;
using ERY.Xle.Services.ScreenModel;

namespace ERY.Xle
{
    public abstract class XleGameFactory : IXleGameFactory
    {
        public ITextArea TextArea { get; set; }

        public abstract string GameTitle { get; }

        public abstract void LoadSurfaces();

        public abstract IXleSerializable CreateStoryData();

        // Surfaces
        public FontSurface Font { get; protected set; }					// returns the handle to the font resource
        public Surface Character { get; protected set; }		// returns the handle to the character resource
        public Surface Monsters { get; protected set; }				// returns the handle to the monsters resource

        public virtual void CheatLevel(Player player, int level)
        {
            throw new NotImplementedException();
        }

        public abstract Maps.Map3DSurfaces GetMap3DSurfaces(Map3D map3D);

        public virtual void SetGameSpeed(GameState state, int speed)
        {
            state.GameSpeed.CastleOpenChestTime = 500 + 200 * speed;
            state.GameSpeed.AfterSetGamespeedTime = 300 + 200 * speed;
            state.GameSpeed.CastleOpenChestSoundTime = 750;
            state.GameSpeed.DungeonOpenChestSoundTime = 500;

            state.GameSpeed.GeneralStepTime = 150;
            state.GameSpeed.OutsideStepTime = 350;
            state.GameSpeed.DungeonStepTime = 333;

            if (speed == 1)
            {
                state.GameSpeed.GeneralStepTime /= 2;
                state.GameSpeed.OutsideStepTime = state.GameSpeed.GeneralStepTime;
                state.GameSpeed.DungeonStepTime = 200;
            }
        }

        public abstract int MailItemID { get; }
        public abstract int HealingItemID { get; }
        public abstract int ClimbingGearItemID { get; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services;
using ERY.Xle.Data;
using ERY.Xle.Services.MapLoad;

namespace ERY.Xle.XleEventTypes.Extenders
{
    public class ChangeMap : EventExtender
    {
        public new ChangeMapEvent TheEvent { get { return (ChangeMapEvent)base.TheEvent; } }

        public IMapChanger MapChanger { get; set; }
        public XleData Data { get; set; }

        protected bool VerifyMapExistence()
        {
            try
            {
                string mapName = GetMapName();
            }
            catch
            {
                SoundMan.PlaySound(LotaSound.Medium);

                TextArea.PrintLine();
                TextArea.PrintLine("Map ID " + TheEvent.MapID + " not found.");
                TextArea.PrintLine();

                GameControl.Wait(1500);

                return false;
            }

            return true;
        }

        public override bool StepOn(GameState state)
        {
            var player = state.Player;

            if (player.X < TheEvent.X) return false;
            if (player.Y < TheEvent.Y) return false;
            if (player.X >= TheEvent.X + TheEvent.Width) return false;
            if (player.Y >= TheEvent.Y + TheEvent.Height) return false;

            if (TheEvent.MapID != 0 && VerifyMapExistence() == false)
                return false;

            bool cancel = false;

            return OnStepOnImpl(state, ref cancel);
        }

        protected virtual bool OnStepOnImpl(GameState state, ref bool cancel)
        {
            ExecuteMapChange();

            return true;
        }


        public string GetMapName()
        {
            return Data.MapList[TheEvent.MapID].Name;
        }

        public void ExecuteMapChange()
        {
            try
            {
                MapChanger.ChangeMap(GameState.Player, TheEvent.MapID, TheEvent.TargetEntryPoint);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                SoundMan.PlaySound(LotaSound.Bad);

                TextArea.Print("Failed to load ", XleColor.White);
                TextArea.Print(GetMapName(), XleColor.Red);
                TextArea.Print(".", XleColor.White);
                TextArea.PrintLine();
                TextArea.PrintLine();

                GameControl.Wait(1500);
            }
        }

        [Obsolete("Use overload with no parameters instead.")]
        public void ExecuteMapChange(GameState state)
        {
            ExecuteMapChange();
        }
        [Obsolete("Use overload with no parameters instead.")]
        public void ExecuteMapChange(Player player)
        {
            ExecuteMapChange();
        }

    }
}

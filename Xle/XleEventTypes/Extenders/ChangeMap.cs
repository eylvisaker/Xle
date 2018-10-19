using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xle.Services;
using Xle.Data;
using Xle.Services.MapLoad;

namespace Xle.XleEventTypes.Extenders
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

        public override bool StepOn()
        {
            if (Player.X < TheEvent.X) return false;
            if (Player.Y < TheEvent.Y) return false;
            if (Player.X >= TheEvent.X + TheEvent.Width) return false;
            if (Player.Y >= TheEvent.Y + TheEvent.Height) return false;

            if (TheEvent.MapID != 0 && VerifyMapExistence() == false)
                return false;

            bool cancel = false;

            return OnStepOnImpl(ref cancel);
        }

        protected virtual bool OnStepOnImpl(ref bool cancel)
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
                MapChanger.ChangeMap(TheEvent.MapID, TheEvent.TargetEntryPoint);
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

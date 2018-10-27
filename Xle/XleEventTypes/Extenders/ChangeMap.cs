using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xle.Services;
using Xle.Data;
using Xle.Services.MapLoad;
using System.Threading.Tasks;

namespace Xle.XleEventTypes.Extenders
{
    public class ChangeMap : EventExtender
    {
        public new ChangeMapEvent TheEvent { get { return (ChangeMapEvent)base.TheEvent; } }

        public IMapChanger MapChanger { get; set; }
        public XleData Data { get; set; }

        protected async Task<bool> VerifyMapExistence()
        {
            try
            {
                string mapName = GetMapName();
            }
            catch
            {
                SoundMan.PlaySound(LotaSound.Medium);

               await TextArea.PrintLine();
               await TextArea.PrintLine("Map ID " + TheEvent.MapID + " not found.");
               await TextArea.PrintLine();

                await GameControl.WaitAsync(1500);

                return false;
            }

            return true;
        }

        public override async Task<bool> StepOn()
        {
            if (Player.X < TheEvent.X) return false;
            if (Player.Y < TheEvent.Y) return false;
            if (Player.X >= TheEvent.X + TheEvent.Width) return false;
            if (Player.Y >= TheEvent.Y + TheEvent.Height) return false;

            if (TheEvent.MapID != 0 && await VerifyMapExistence() == false)
                return false;

            return await OnStepOnImpl();
        }

        protected virtual async Task<bool> OnStepOnImpl()
        {
            await ExecuteMapChange();

            return true;
        }


        public string GetMapName()
        {
            return Data.MapList[TheEvent.MapID].Name;
        }

        public async Task ExecuteMapChange()
        {
            try
            {
                MapChanger.ChangeMap(TheEvent.MapID, TheEvent.TargetEntryPoint);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                SoundMan.PlaySound(LotaSound.Bad);

                await TextArea.Print("Failed to load ", XleColor.White);
                await TextArea.Print(GetMapName(), XleColor.Red);
                await TextArea.Print(".", XleColor.White);
                await TextArea.PrintLine();
                await TextArea.PrintLine();

                await GameControl.WaitAsync(1500);
            }
        }
    }
}

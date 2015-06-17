using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services.Implementation;

namespace ERY.Xle.XleEventTypes.Extenders
{
	public class ChangeMapExtender : EventExtender
	{
		public new ChangeMapEvent TheEvent { get { return (ChangeMapEvent)base.TheEvent; } }

		protected bool VerifyMapExistence()
		{
			try
			{
				string mapName = GetMapName();
			}
			catch
			{
				SoundMan.PlaySound(LotaSound.Medium);

				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("Map ID " + TheEvent.MapID + " not found.");
				XleCore.TextArea.PrintLine();

				XleCore.Wait(1500);

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
			ExecuteMapChange(state.Player);

			return true;
		}


		public string GetMapName()
		{
			return XleCore.Data.MapList[TheEvent.MapID].Name;
		}

		public void ExecuteMapChange(GameState state)
		{
			try
			{
				XleCore.ChangeMap(state.Player, TheEvent.MapID, TheEvent.TargetEntryPoint);
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.WriteLine(e.Message);

				SoundMan.PlaySound(LotaSound.Bad);

				XleCore.TextArea.Print("Failed to load ", XleColor.White);
				XleCore.TextArea.Print(GetMapName(), XleColor.Red);
				XleCore.TextArea.Print(".", XleColor.White);
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine();

				XleCore.Wait(1500);
			}
		}
		[Obsolete("Use GameState overload instead.")]
		public void ExecuteMapChange(Player player)
		{
			ExecuteMapChange(XleCore.GameState);
		}

	}
}

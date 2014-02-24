using AgateLib.Geometry;
using ERY.Xle.XleMapTypes;
using ERY.Xle.XleMapTypes.Extenders;
using ERY.Xle.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Museum
{
	class MuseumExtender : NullMuseumExtender
	{
		public override void OnLoad(GameState state)
		{
			CheckExhibitStatus(state);

			CheckInformationMessage(state);
		}

		public override void CheckExhibitStatus(GameState state)
		{
			// lost displays
			if (state.Player.museum[0xa] > 0)
			{
				for (int i = 0; i < state.Map.Width; i++)
				{
					for (int j = 0; j < state.Map.Height; j++)
					{
						if (state.Map[i, j] == 0x5a)
							state.Map[i, j] = 0x10;
					}
				}
			}

			// welcome exhibit
			if (state.Player.museum[1] == 0)
			{
				state.Map[4, 1] = 0;
				state.Map[3, 10] = 0;
			}
			else if (state.Player.museum[1] == 1)
			{
				state.Map[4, 1] = 0;
				state.Map[3, 10] = 16;
			}
			else
			{
				state.Map[4, 1] = 16;
				state.Map[3, 10] = 16;
			}
		}

		private void CheckInformationMessage(GameState state)
		{
			// check to see if the caretaker wants to see the player
			var info = Information;

			if (info.ShouldLevelUp(state.Player))
			{
				XleCore.TextArea.Clear();
				XleCore.TextArea.PrintLine("The caretaker wants to see you!");

				SoundMan.PlaySound(LotaSound.Good);

				while (SoundMan.IsPlaying(LotaSound.Good))
					XleCore.Wait(50);
			}
		}

		private Information Information
		{
			get { return (Information)GetExhibitByTile(0x50); }
		}

		public override void BeforeEntry(GameState state, ref int targetEntryPoint)
		{
			if (targetEntryPoint < 3)
				targetEntryPoint = state.Story().MuseumEntryPoint;
		}

		public override void PlayerUse(GameState state, int item, ref bool handled)
		{
			// twist gold armband
			if (item == (int)LotaItem.GoldArmband)
			{
				UseGoldArmband(state);

				handled = true;
				return;
			}
		}
		public override void PlayerStep(GameState state)
		{
			if (state.Player.X == 12 && state.Player.Y == 13)
			{
				if (state.Player.museum[1] < 3)
				{
					var welcome = (Welcome)GetExhibitByTile(0x51);
					welcome.PlayGoldArmbandMessage(state.Player);
					state.Player.museum[1] = 3;

					CheckExhibitStatus(state);
				}
			}
		}
		private void UseGoldArmband(GameState state)
		{
			Point faceDir = Map3D.StepDirection(state.Player.FaceDirection);
			Point test = new Point(state.Player.X + faceDir.X, state.Player.Y + faceDir.Y);

			// door value
			if (TheMap[test.X, test.Y] == 0x02)
			{
				XleCore.Wait(1000);

				foreach (var entry in state.Map.EntryPoints)
				{
					if (entry.Location == state.Player.Location)
					{
						state.Story().MuseumEntryPoint = state.Map.EntryPoints.IndexOf(entry);
					}
				}

				TheMap.LeaveMap(state.Player);

			}
			else
			{
				XleCore.TextArea.PrintLine("The gold armband hums softly.");
			}
		}
	}
}

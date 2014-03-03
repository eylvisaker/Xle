using AgateLib.Geometry;
using ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays;
using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Maps.XleMapTypes.Extenders;
using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Museum
{
	class MuseumExtender : NullMuseumExtender
	{
		Dictionary<int, Exhibit> mExhibits = new Dictionary<int, Exhibit>();

		public MuseumExtender()
		{
			mExhibits.Add(0x50, new MuseumDisplays.Information());
			mExhibits.Add(0x51, new MuseumDisplays.Welcome());
			mExhibits.Add(0x52, new MuseumDisplays.Weaponry());
			mExhibits.Add(0x53, new MuseumDisplays.Thornberry());
			mExhibits.Add(0x54, new MuseumDisplays.Fountain());
			mExhibits.Add(0x55, new MuseumDisplays.PirateTreasure());
			mExhibits.Add(0x56, new MuseumDisplays.HerbOfLife());
			mExhibits.Add(0x57, new MuseumDisplays.NativeCurrency());
			mExhibits.Add(0x58, new MuseumDisplays.StonesWisdom());
			mExhibits.Add(0x59, new MuseumDisplays.Tapestry());
			mExhibits.Add(0x5A, new MuseumDisplays.LostDisplays());
			mExhibits.Add(0x5B, new MuseumDisplays.KnightsTest());
			mExhibits.Add(0x5C, new MuseumDisplays.FourJewels());
			mExhibits.Add(0x5D, new MuseumDisplays.Guardian());
			mExhibits.Add(0x5E, new MuseumDisplays.Pegasus());
			mExhibits.Add(0x5F, new MuseumDisplays.AncientArtifact());
		}
		public override void SetCommands(Commands.CommandList commands)
		{
			commands.Items.AddRange(LotaProgram.CommonLotaCommands);

			commands.Items.Add(new Commands.Rob());
			commands.Items.Add(new Commands.Take());
			commands.Items.Add(new Commands.Speak());
		}

		public override Exhibit GetExhibitByTile(int tile)
		{
			if (mExhibits.ContainsKey(tile) == false)
				return null;

			return mExhibits[tile];
		}
		public override void OnLoad(GameState state)
		{
			CheckExhibitStatus(state);
		}

		public override void OnAfterEntry(GameState state)
		{
			CheckInformationMessage(state);
		}

		public override void CheckExhibitStatus(GameState state)
		{
			// lost displays
			if (state.Story().Museum[0xa] > 0)
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
			if (state.Story().Museum[1] == 0)
			{
				state.Map[4, 1] = 0;
				state.Map[3, 10] = 0;
			}
			else if (state.Story().Museum[1] == 1)
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
				if (state.Story().Museum[1] < 3)
				{
					var welcome = (Welcome)GetExhibitByTile(0x51);
					welcome.PlayGoldArmbandMessage(state.Player);
					state.Story().Museum[1] = 3;

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

		public override void NeedsCoinMessage(Player player, Exhibit ex)
		{
			var lotaex = (LotaExhibit)ex;

			XleCore.TextArea.PrintLine("You'll need a " + lotaex.Coin.ToString() + " coin.");
		}

		public override void PrintUseCoinMessage(Player player, Exhibit ex)
		{
			var lotaex = (LotaExhibit)ex;

			XleCore.TextArea.PrintLine();
		}
	}
}

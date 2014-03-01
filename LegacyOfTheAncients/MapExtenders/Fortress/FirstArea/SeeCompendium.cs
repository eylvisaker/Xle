using ERY.Xle.XleEventTypes;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Fortress.FirstArea
{
	class SeeCompendium : NullEventExtender
	{
		bool paralyzed = false;
		bool ran = false;

		public override void StepOn(GameState state, ref bool handled)
		{
			// make sure the player is entirely contained by the event.
			if (state.Player.X != TheEvent.Location.X)
				return;

			handled = true;

			if (paralyzed)
				return;

			paralyzed = true;

			var gd = (IHasGuards)state.Map;

			Guard warlord = new Guard();
			warlord.Location = new AgateLib.Geometry.Point(106, 47);
			warlord.Color = XleColor.LightGreen;

			gd.Guards.Add(warlord);

			PrintSeeCompendiumMessage(state);
			DoSonicMagic(state, warlord);

			XleCore.TextArea.PrintLine("The warlord appears at the wall.");
			XleCore.TextArea.PrintLine();

			MoveWarlordToCompendium(warlord);
			HitPlayer(state);
			WarlordSpeech();
			RemoveCompendium(state);


			MoveWarlordOut(warlord);

			gd.Guards.Remove(warlord);
		}

		private void RemoveCompendium(GameState state)
		{
			TreasureChestEvent evt = state.Map.Events.OfType<TreasureChestEvent>()
				.First(x => x.ExtenderName.Equals("CompendiumFirst", StringComparison.InvariantCultureIgnoreCase));

			evt.Enabled = false;
			evt.SetOpenTilesOnMap(state.Map);
		}

		public override void TryToStepOn(GameState state, int dx, int dy, ref bool allowStep)
		{
			if (state.Player.HP > 30 && paralyzed)
			{
				paralyzed = false;
				TheEvent.Enabled = false;
			}
			else if (paralyzed)
			{
				XleCore.TextArea.PrintLine("Legs paralyzed.");
				allowStep = false;
			}
		}
		private void DoSonicMagic(GameState state, Guard warlord)
		{
			XleCore.TextArea.PrintLine("Sonic magic...");
			XleCore.TextArea.PrintLine();
			XleCore.Wait(3000);
			XleCore.TextArea.PrintLine("You can't move.");
			XleCore.TextArea.PrintLine();
			XleCore.Wait(3000);
			
		}

		private void MoveWarlordOut(Guard warlord)
		{
			for (int i = 0; i < 6; i++)
			{
				MoveWarlord(warlord, 1, 0);
			}

			for (int i = 0; i < 2; i++)
			{
				MoveWarlord(warlord, 0, 1);
			}

			XleCore.Wait(1000);
		}

		private void MoveWarlordToCompendium(Guard warlord)
		{

			for (int i = 0; i < 5; i++)
			{
				MoveWarlord(warlord, -1, 0);
			}

			for (int i = 0; i < 2; i++)
			{
				MoveWarlord(warlord, 0, -1);
			}

			for (int i = 0; i < 2; i++)
			{
				MoveWarlord(warlord, -1, 0);
			}

			warlord.Facing = Direction.South;
		}

		private void WarlordSpeech()
		{
			XleCore.TextArea.PrintSlow("You fool!  ", XleColor.Yellow);
			XleCore.Wait(1500);
			XleCore.TextArea.PrintSlow("You can't stop me!  ", XleColor.Yellow);
			XleCore.Wait(1500);
			XleCore.TextArea.PrintLineSlow("as you", XleColor.Yellow);
			XleCore.TextArea.PrintLineSlow("stand helpless, I'll use this scroll", XleColor.Yellow);
			XleCore.TextArea.PrintSlow("to cast the ", XleColor.Yellow);
			XleCore.TextArea.PrintSlow("spell of death. ");
			XleCore.TextArea.PrintLineSlow("All life", XleColor.Yellow);
			XleCore.TextArea.PrintLineSlow("outside this fortress will cease.", XleColor.Yellow);

			XleCore.TextArea.SetLineColor(XleColor.Red, 0, 1, 2, 3, 4);

			XleCore.Wait(2000);
		}

		private void HitPlayer(GameState state)
		{
			state.Player.HP = 28;

			XleCore.FlashHPWhile(XleColor.Red, XleColor.Yellow, new CountdownTimer(1500).StillRunning);
			
		}

		private void MoveWarlord(Guard warlord, int dx, int dy)
		{
			warlord.X += dx;
			warlord.Y += dy;
			warlord.Facing = XleMap.DirectionFromPoint(new AgateLib.Geometry.Point(dx, dy));

			SoundMan.PlaySound(LotaSound.WalkOutside);
			XleCore.Wait(750);
		}

		private void PrintSeeCompendiumMessage(GameState state)
		{
			XleCore.TextArea.Clear(true);
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("    you see the compendium!");
			XleCore.TextArea.PrintLine();
			
			SoundMan.PlaySound(LotaSound.VeryGood);
			XleCore.Wait(1500);

			XleCore.TextArea.FlashLinesWhile(() => SoundMan.IsPlaying(LotaSound.VeryGood), XleColor.Yellow, XleColor.Cyan, 100);

			XleCore.Wait(1000);
		}
	}
}

using AgateLib.Geometry;
using ERY.Xle.LotA.MapExtenders.Fortress.FirstArea;
using ERY.Xle.LotA.MapExtenders.Fortress.SecondArea;
using ERY.Xle.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services.Implementation;
using ERY.Xle.XleEventTypes;

namespace ERY.Xle.LotA.MapExtenders.Fortress
{
	public class FortressFinal : FortressEntry
	{
		ExtenderDictionary extenders = new ExtenderDictionary();
		Guard warlord;

		double compendiumStrength = 140;

		public FortressFinal()
		{
			WhichCastle = 2;
			CastleLevel = 2;
			GuardAttack = 3.5;

			extenders.Add("DoorShut", new DoorShut());
			extenders.Add("Compendium", new Compendium(this));
			extenders.Add("MagicIce", new FinalMagicIce(this));
		}

		public override XleEventTypes.Extenders.EventExtender CreateEventExtender(XleEvent evt, Type defaultExtender)
		{
			return extenders.Find(evt.ExtenderName) ?? base.CreateEventExtender(evt, defaultExtender);
		}

		public override int GetOutsideTile(AgateLib.Geometry.Point playerPoint, int x, int y)
		{
			if (y >= TheMap.Height)
				return 0;
			else
				return 11;
		}

		public bool CompendiumAttacking { get; set; }

		public override void AfterExecuteCommand(GameState state, AgateLib.InputLib.KeyCode cmd)
		{
			if (warlord != null)
			{
				WarlordAttack(state);
			}
			else if (CompendiumAttacking)
			{
				CompendiumAttack(state);
			}
		}

		private void CompendiumAttack(GameState state)
		{
			int damage = XleCore.random.Next((int)compendiumStrength / 2, (int)compendiumStrength);

			XleCore.Wait(75);
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Compendium attack - blow " + damage + " H.P.", XleColor.Green);

			SoundMan.PlayMagicSound(LotaSound.MagicBolt, LotaSound.MagicBoltHit, 2);
			XleCore.Wait(250, FlashBorder);
			TheMap.ColorScheme.FrameColor = XleColor.Gray;

			state.Player.HP -= damage;

			XleCore.Wait(75);

			if (state.Player.Items[LotaItem.HealingHerb] > 24)
			{
				int amount = (int)(state.Player.Items[LotaItem.HealingHerb] / 9);

				amount = (int)(amount * (1 + XleCore.random.NextDouble()) * 0.5);

				XleCore.TextArea.PrintLine("** " + amount.ToString() + " healing herbs destroyed! **", XleColor.Yellow);

				state.Player.Items[LotaItem.HealingHerb] -= amount;

				XleCore.Wait(75);
			}
		}

		int borderIndex;
		Color flashColor = XleColor.LightGreen;

		private void FlashBorder()
		{
			borderIndex++;

			if (borderIndex % 4 < 2)
				TheMap.ColorScheme.FrameColor = flashColor;
			else
				TheMap.ColorScheme.FrameColor = XleColor.Gray;

			XleCore.Redraw();
		}

		private void WarlordAttack(GameState state)
		{
			int damage = (int)(99 * XleCore.random.NextDouble() + 80);

			TheMap.ColorScheme.FrameColor = XleColor.Pink;
			flashColor = XleColor.Red;

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Warlord attack - blow " + damage.ToString() + " H.P.", XleColor.Yellow);

			SoundMan.PlayMagicSound(LotaSound.MagicFlame, LotaSound.MagicFlameHit, 2);
			XleCore.Wait(250, FlashBorder);
			TheMap.ColorScheme.FrameColor = XleColor.Gray;

			XleCore.Wait(150);
		}


		public void CreateWarlord(GameState state)
		{
			warlord = new Guard
			{
				X = 5,
				Y = 45,
				HP = 420,
				Color = XleColor.LightGreen,
				Name = "Warlord",
				OnGuardDead = WarlordDead,
				SkipAttacking = true,
				SkipMovement = true,
			};

			TheMap.Guards.Add(warlord);
		}

		private bool WarlordDead(GameState state, Guard unused)
		{
			this.warlord = null;

			XleCore.TextArea.Clear(true);
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("        ** warlord killed **");

			for (int i = 0; i < 5; i++)
			{
				SoundMan.PlaySound(LotaSound.Good);
				XleCore.Wait(750);
			}
			XleCore.Wait(1000);
			
			SoundMan.PlaySoundSync(LotaSound.VeryGood);

			PrintSecurityAlertMessage();

			return true;
		}

		private void PrintSecurityAlertMessage()
		{
		}
	}
}

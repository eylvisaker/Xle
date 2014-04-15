using ERY.Xle.XleEventTypes;
using ERY.Xle.Maps.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERY.Xle.Maps;

namespace ERY.Xle.LotA.MapExtenders.Castle
{
	class CastleGround : CastleExtender
	{
		public CastleGround()
		{

		}
		public override void OnLoad(GameState state)
		{
			Lota.SetMuseumCoinOffers(state);
		}

		public override void SetCommands(Commands.CommandList commands)
		{
			commands.Items.AddRange(LotaProgram.CommonLotaCommands);

			commands.Items.Add(new Commands.Magic());
			commands.Items.Add(new Commands.Open());
			commands.Items.Add(new Commands.Take());
			commands.Items.Add(new Commands.Speak());
		}
		public override XleEventTypes.Extenders.EventExtender CreateEventExtender(XleEvent evt, Type defaultExtender)
		{
			string name = evt.ExtenderName.ToLowerInvariant();

			if (evt is Door)
				return new CastleDoor();
			if (name == "magicice")
				return new MagicIce();
			if (name == "seeds")
				return new SeedPlant();
			if (name == "casandra")
				return new Casandra();
			if (evt is TreasureChestEvent)
				return new Chest { CastleLevel = 1 };

			return base.CreateEventExtender(evt, defaultExtender);
		}

		public override int GetOutsideTile(AgateLib.Geometry.Point playerPoint, int x, int y)
		{
			if (y >= TheMap.Height)
				return 16;
			else
				return base.GetOutsideTile(playerPoint, x, y);
		}

		public override void PlayerUse(GameState state, int item, ref bool handled)
		{
			switch (item)
			{
				case (int)LotaItem.MagicSeed:
					handled = UseMagicSeeds(state.Player);
					break;
			}
		}
		private bool UseMagicSeeds(Player player)
		{
			XleCore.Wait(150);

			Lota.Story.Invisible = true;
			XleCore.TextArea.PrintLine("You're invisible.");
			XleCore.Renderer.PlayerColor = XleColor.DarkGray;

			TheMap.Guards.IsAngry = false;

			XleCore.Wait(500);

			player.Items[LotaItem.MagicSeed]--;

			return true;
		}

		public override void SpeakToGuard(GameState state)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();

			if (Lota.Story.Invisible)
			{
				XleCore.TextArea.PrintLine("The guard looks startled.");
			}
			else
			{
				XleCore.TextArea.PrintLine("The guard ignores you.");
			}
		}

		protected override void OnSetAngry(bool value)
		{
			var state = XleCore.GameState;

			Lota.Story.Invisible = false;
			XleCore.Renderer.PlayerColor = XleColor.White;
		}

		protected int WhichCastle = 1;
		protected double CastleLevel = 1;
		protected double GuardAttack = 1;

		public override double ChanceToHitGuard(Player player, Guard guard, int distance)
		{
			int weaponType = player.CurrentWeapon.ID;
			double GuardDefense = 1;

			if (WhichCastle == 2)
				GuardDefense = player.Attribute[Attributes.dexterity] / 26.0;

			return (player.Attribute[Attributes.dexterity] + 13)
				* (99 + weaponType * 11) / 7500.0 / GuardDefense;
		}


		public override int RollDamageToGuard(Player player, Guard guard)
		{
			int weaponType = player.CurrentWeapon.ID;

			double damage = player.Attribute[Attributes.strength] *
					   (weaponType / 2 + 1) / 7;

			damage *= 1 + 2 * XleCore.random.NextDouble();

			return (int)Math.Round(damage);
		}


		public override double ChanceToHitPlayer(Player player, Guard guard)
		{
			return 1 - (player.Attribute[Attributes.dexterity] / 99.0);
		}


		public override int RollDamageToPlayer(Player player, Guard guard)
		{
			int armorType = player.CurrentArmor.ID;

			double damage =
				Math.Pow(CastleLevel, 1.8) * GuardAttack * (300 + XleCore.random.NextDouble() * 600) /
				(armorType + 2) / Math.Pow(player.Attribute[Attributes.endurance], 0.9) + 2;

			return (int)Math.Round(damage);
		}
	}
}

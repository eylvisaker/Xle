using ERY.Xle.LoB.MapExtenders.Citadel.EventExtenders;
using ERY.Xle.XleEventTypes;
using ERY.Xle.Maps.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERY.Xle.Maps;

namespace ERY.Xle.LoB.MapExtenders.Citadel
{
	class CitadelUpper  : CastleExtender
	{
		CastleDamageCalculator cdc = new CastleDamageCalculator
		{ v5 = 1.6, v6 = 5.5, v7 = 2.3 };

		public override void SetColorScheme(ColorScheme scheme)
		{
			scheme.TextColor = XleColor.White;

			scheme.FrameColor = XleColor.Orange;
			scheme.FrameHighlightColor = XleColor.Yellow;
		}

		public override void SetCommands(Commands.CommandList commands)
		{
			commands.Items.AddRange(LobProgram.CommonLobCommands);

			commands.Items.Add(new Commands.Open());
			commands.Items.Add(new Commands.Magic());
			commands.Items.Add(new Commands.Take());
			commands.Items.Add(new Commands.Speak());
		}

		public override double ChanceToHitGuard(Player player, Guard guard, int distance)
		{
			return cdc.ChanceToHitGuard(player, distance);
		}
		public override double ChanceToHitPlayer(Player player, Guard guard)
		{
			return cdc.ChanceToHitPlayer(player);
		}
		public override int RollDamageToGuard(Player player, Guard guard)
		{
			return cdc.RollDamageToGuard(player);
		}
		public override int RollDamageToPlayer(Player player, Guard guard)
		{
			return cdc.RollDamageToPlayer(player);
		}

		public override int GetOutsideTile(AgateLib.Geometry.Point playerPoint, int x, int y)
		{
			return TheMap.OutsideTile;
		}

		public override XleEventTypes.Extenders.IEventExtender CreateEventExtender(XleEvent evt, Type defaultExtender)
		{
			string name = evt.ExtenderName.ToLowerInvariant();

			if (name == "mantrek")
			{
				var mantrek = new Mantrek();
				
				if (Lob.Story.MantrekKilled)
					mantrek.EraseMantrek(TheMap);

				return mantrek;
			}
			if (name == "staffportal")
				return new StaffPortal();
			if (name == "elf")
				return new Elf();
			if (name == "tattoo")
				return new Tattoo();

			if (evt is ChangeMapEvent)
				return new ChangeMapTeleporter();

			return base.CreateEventExtender(evt, defaultExtender);
		}
	}
}

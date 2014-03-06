using AgateLib.Geometry;
using ERY.Xle.LoB.MapExtenders.Castle.EventExtenders;
using ERY.Xle.XleEventTypes;
using ERY.Xle.Maps.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Castle
{
	class DurekCastle : NullCastleExtender
	{
		CastleDamageCalculator cdc = new CastleDamageCalculator { v5 = 0.9, v6 = 0.95, v7 = 0.95 };

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

		public override XleEventTypes.Extenders.IEventExtender CreateEventExtender(XleEvent evt, Type defaultExtender)
		{
			if (evt is TreasureChestEvent)
				return new Chest { ChestArrayIndex = 0 };
			if (evt is Door)
			{
				if ((evt as Door).RequiredItem == (int)LobItem.FalconFeather)
					return new FeatherDoor();
				else
					return new CastleDoor();
			}

			switch (evt.ExtenderName)
			{
				case "King": return new King();
				case "Seravol": return new Seravol();
				case "Arman": return new Arman();
				case "DaisMessage": return new DaisMessage();
				case "AngryOrcs": return new AngryOrcs(this);
				case "AngryCastle": return new AngryCastle(this);
				case "SingingCrystal": return new SingingCrystal();
			}


			return base.CreateEventExtender(evt, defaultExtender);
		}
		public override int GetOutsideTile(AgateLib.Geometry.Point playerPoint, int x, int y)
		{
			if (playerPoint.X < 12 && playerPoint.Y < 12)
				return 0;

			if (playerPoint.X < 45 && playerPoint.Y < 22)
				return 17;

			if (playerPoint.X > 50 && playerPoint.Y > 75)
				return 16;

			return 32;
		}

		public override void SetCommands(Commands.CommandList commands)
		{
			commands.Items.AddRange(LobProgram.CommonLobCommands);

			commands.Items.Add(new Commands.Leave());
			commands.Items.Add(new Commands.Open());
			commands.Items.Add(new Commands.Magic());
			commands.Items.Add(new Commands.Take());
			commands.Items.Add(new Commands.Speak());
		}

		public override void SetColorScheme(ColorScheme scheme)
		{
			scheme.TextColor = XleColor.White;

			scheme.FrameColor = XleColor.LightGray;
			scheme.FrameHighlightColor = XleColor.Yellow;
		}

		public override void OnLoad(GameState state)
		{
			if (state.Player.Items[LobItem.FalconFeather] == 0)
			{
				RemoveFalconFeatherDoor(state);
			}
			if (state.Story().ClearedRockSlide)
			{
				RemoveRockSlide(state);
			}

			if (state.Story().DefeatedOrcs == false)
			{
				ColorOrcs(state);
			}
		}

		private void RemoveRockSlide(GameState state)
		{
			var evt = TheMap.Events.FirstOrDefault(x => x.ExtenderName == "SingingCrystal");

			SingingCrystal.RemoveRockSlide(TheMap, evt.Rectangle);
		}

		private void ColorOrcs(GameState state)
		{
			if (state.Story().DefeatedOrcs == false)
			{
				Rectangle area = Rectangle.FromLTRB(66, 0, TheMap.Width, 68);

				foreach (var guard in TheMap.Guards)
				{
					if (area.Contains(guard.Location))
						guard.Color = XleColor.Blue;
				}
			}
		}

		private void RemoveFalconFeatherDoor(GameState state)
		{
			var door = TheMap.Events.OfType<Door>().First(
				x => x is Door && (x as Door).RequiredItem == (int)LobItem.FalconFeather);

			door.RemoveDoor(state);
		}

		public override void SpeakToGuard(GameState state, ref bool handled)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();

			if (state.Player.Items[LobItem.FalconFeather] > 0)
			{
				XleCore.TextArea.PrintLine("I see you have the feather,");
				XleCore.TextArea.PrintLine("why not use it?");
			}
			else
			{
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("I should not converse, sir.");
			}

			XleCore.Wait(2500);
			handled = true;
		}

		public override void PlayerUse(GameState state, int item, ref bool handled)
		{
			if (item == (int)LobItem.FalconFeather)
			{
				XleCore.TextArea.PrintLine("You're not by a door.");
				handled = true;
			}
		}

		public bool StoredAngryFlag { get; set; }

		public bool InOrcArea { get; set; }
	}
}

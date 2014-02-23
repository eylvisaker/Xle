using ERY.Xle.LoB.MapExtenders.Castle.EventExtenders;
using ERY.Xle.XleEventTypes;
using ERY.Xle.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Castle
{
	class DurekCastle : NullCastleExtender
	{
		public override XleEventTypes.Extenders.IEventExtender CreateEventExtender(XleEvent evt, Type defaultExtender)
		{
			if (evt is Door)
			{
				if ((evt as Door).RequiredItem == (int)LobItem.FalconFeather)
					return new FeatherDoor();
				else
					return new CastleDoor();
			}
			if (evt is SpeakToPerson)
			{
				switch(evt.ExtenderName)
				{
					case "King": return new King();
					case "Seravol": return new Seravol();
				}
			}
			if (evt is Script)
			{
				if (evt.ExtenderName == "DaisMessage")
					return new DaisMessage();
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

		public override void GetBoxColors(out AgateLib.Geometry.Color boxColor, out AgateLib.Geometry.Color innerColor, out AgateLib.Geometry.Color fontColor, out int vertLine)
		{
			base.GetBoxColors(out boxColor, out innerColor, out fontColor, out vertLine);

			boxColor = XleColor.LightGray;
		}


		public override void OnLoad(GameState state)
		{
			if (state.Player.Items[LobItem.FalconFeather] == 0)
			{
				RemoveFalconFeatherDoor(state);
			}
			
		}

		private void RemoveFalconFeatherDoor(GameState state)
		{
			var door = TheMap.Events.OfType<Door>().First(
				x => x is Door && (x as Door).RequiredItem == (int)LobItem.FalconFeather);

			door.RemoveDoor(state);
		}

		public override void SpeakToGuard(GameState gameState, ref bool handled)
		{
			if (gameState.Player.Items[LobItem.FalconFeather] > 0)
			{
				g.AddBottom("");
				g.AddBottom("I see you have the feather,");
				g.AddBottom("why not use it?");

			}
			else
			{
				g.AddBottom("");
				g.AddBottom("I should not converse, sir.");
			}

			XleCore.Wait(2500);
			handled = true;
		}
	}
}

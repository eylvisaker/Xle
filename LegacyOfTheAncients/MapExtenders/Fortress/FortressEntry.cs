using ERY.Xle.LotA.MapExtenders.Castle;
using ERY.Xle.LotA.MapExtenders.Fortress.FirstArea;
using ERY.Xle.Maps.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Fortress
{
	class FortressEntry : CastleGround
	{
		ExtenderDictionary extenders = new ExtenderDictionary();

		public FortressEntry()
		{
			WhichCastle = 2;
			CastleLevel = 1;
			GuardAttack = 3.5;

			extenders.Add("MagicIce", new MagicIce());
			extenders.Add("Elevator", new Elevator());
			extenders.Add("GasTrap", new GasTrap());
			extenders.Add("SpeakGuard", new SpeakGuard());
			extenders.Add("Armor", new ArmorBox());
			extenders.Add("GuardWarning", new GuardWarning());
			extenders.Add("SeeCompendium", new SeeCompendium());
		}

		public override void SetColorScheme(ColorScheme scheme)
		{
			scheme.TextColor = XleColor.White;

			scheme.FrameColor = XleColor.Gray;
			scheme.FrameHighlightColor = XleColor.Yellow;
		}
		public override XleEventTypes.Extenders.IEventExtender CreateEventExtender(XleEvent evt, Type defaultExtender)
		{
			return extenders.Find(evt.ExtenderName) ??
					base.CreateEventExtender(evt, defaultExtender);
		}

		public override void OnSetAngry(bool value)
		{
			base.OnSetAngry(value);

			XleCore.Renderer.PlayerColor = XleColor.White;
		}

		public override void SpeakToGuard(GameState state, ref bool handled)
		{
			XleCore.TextArea.PrintLine();
			handled = true;

			if (TheMap.IsAngry)
			{
				XleCore.TextArea.PrintLine("The guard ignores you.");
			}
			else
				XleCore.TextArea.PrintLine("Greetings soldier.");
		}

		public override int GetOutsideTile(AgateLib.Geometry.Point playerPoint, int x, int y)
		{
			if (x >= 0) 
				return 11;
			else
				return 0;
		}

	}
}

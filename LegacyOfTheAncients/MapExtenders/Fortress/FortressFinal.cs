using ERY.Xle.LotA.MapExtenders.Fortress.FirstArea;
using ERY.Xle.LotA.MapExtenders.Fortress.SecondArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Fortress
{
	class FortressFinal : FortressEntry
	{
		ExtenderDictionary extenders = new ExtenderDictionary();

		public FortressFinal()
		{
			WhichCastle = 2;
			CastleLevel = 2;
			GuardAttack = 3.5;

			extenders.Add("DoorShut", new DoorShut());
			extenders.Add("Compendium", new Compendium(this));
			extenders.Add("FinalMagicIce", new FinalMagicIce(this));
		}

		public override XleEventTypes.Extenders.IEventExtender CreateEventExtender(XleEvent evt, Type defaultExtender)
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
			Guard warlord = FindWarlord(state);

			if (warlord != null)
				WarlordAttack(state);
			else if (CompendiumAttacking)
				CompendiumAttack(state);
		}

		private void CompendiumAttack(GameState state)
		{
			throw new NotImplementedException();
		}

		private void WarlordAttack(GameState state)
		{
			throw new NotImplementedException();
		}

		private Guard FindWarlord(GameState state)
		{
			IHasGuards gd = (IHasGuards)state.Map;

			return gd.Guards.FirstOrDefault(x => x.Color == XleColor.LightGreen);
		}
		
	}
}

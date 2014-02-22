using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using AgateLib;
using AgateLib.InputLib;
using AgateLib.Geometry;
using AgateLib.Serialization.Xle;
using ERY.Xle.XleMapTypes.Extenders;



namespace ERY.Xle.XleMapTypes
{
	public class Castle : Town
	{
		public Castle() { }

		public override IEnumerable<string> AvailableTileImages
		{
			get
			{
				yield return "CastleTiles.png";
			}
		}

		double lastAnim;
		int cycles;

		protected override IMapExtender CreateExtenderImpl()
		{
			if (XleCore.Factory == null)
			{
				Extender = new NullCastleExtender();
			}
			else
			{
				Extender = XleCore.Factory.CreateMapExtender(this);
			}

			base.Extender = Extender;

			return Extender;
		}
		
		public override bool PlayerOpen(Player player)
		{
			XleEvent evt = this.GetEvent(player, 1);

			if (evt == null)
				return false;

			return evt.Open(new GameState(player, this));

		}

		public override bool PlayerTake(Player player)
		{
			XleEvent evt = this.GetEvent(player, 1);

			if (evt == null)
				return false;

			return evt.Take(new GameState(player, this));
		}

		protected override void OpenRoof(Roof roof)
		{
		}
		protected override void CloseRoof(Roof roof)
		{
		}

		public override string[] MapMenu()
		{
			List<string> retval = new List<string>();

			retval.Add("Armor");
			retval.Add("Fight");
			retval.Add("Gamespeed");
			retval.Add("Hold");
			retval.Add("Inventory");
			retval.Add("Magic");
			retval.Add("Open");
			retval.Add("Pass");
			retval.Add("Speak");
			retval.Add("Take");
			retval.Add("Use");
			retval.Add("Weapon");
			retval.Add("Xamine");

			return retval.ToArray();
		}

		public new ICastleExtender Extender { get; set; }
	}
}

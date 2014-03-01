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
		

		protected override void PlayOpenRoofSound(Roof roof)
		{
			// do nothing here
		}
		protected override void PlayCloseRoofSound(Roof roof)
		{
			// do nothing here
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

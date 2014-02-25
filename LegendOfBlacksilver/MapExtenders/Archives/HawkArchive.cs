﻿using ERY.Xle.LoB.MapExtenders.Archives.Exhibits;
using ERY.Xle.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Archives
{
	class HawkArchive : NullMuseumExtender
	{
		Dictionary<int, LobExhibit> mExhibits = new Dictionary<int, LobExhibit>();

		public HawkArchive()
		{
			mExhibits.Add(0x52, new MorningStar());
			mExhibits.Add(0x53, new MarthbaneTunnels());

			mExhibits.Add(0x55, new UnderwaterPort());
			mExhibits.Add(0x56, new DarkWand());

			mExhibits.Add(0x50, new Blacksmith());
			mExhibits.Add(0x51, new FlaxtonIncense());
			mExhibits.Add(0x5e, new KloryksCage());

			mExhibits.Add(0x5a, new CrystalTears());
		}
	}
}

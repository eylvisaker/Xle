﻿using ERY.Xle.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
	class Guardian : LotaExhibit
	{
		public Guardian() : base("Guardian", Coin.Turquoise) { }
		public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.Guardian; } }
	}
}
